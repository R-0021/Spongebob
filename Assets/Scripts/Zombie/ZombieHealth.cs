using Photon.Pun;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
	[SerializeField] private int health = 100;

	[PunRPC]
	public void TakeZombieDamage(int damage)
	{
		health -= damage;

		if (health <= 0)
			PhotonNetwork.Destroy(gameObject);
	}
}
