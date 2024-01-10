using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;

    public bool isLocalPlayer;

    [Header("UI")]
    public Slider[] healthBars;


    [PunRPC]
    public void TakeDamage(int damage)
    {
        health -= damage;

        foreach(var healthBar in healthBars)
            healthBar.value = health / 100f;

        if (health <= 0 && isLocalPlayer)
        {
			RoomManager.Instance.SpawnPlayer();
			PhotonNetwork.Destroy(gameObject);
		}
    }
}
