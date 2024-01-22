using Photon.Pun;
using System.Collections;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
	[SerializeField] private int health = 100;

	PhotonView view;

	private void Start()
	{
		view = GetComponent<PhotonView>();
	}

	[PunRPC]
	public void TakeZombieDamage(int damage)
	{
		health -= damage;

		if (health <= 0 && view && view.IsMine)
		{
			PhotonNetwork.Destroy(view);
			ZombieCount.Decrease(1);
		}
	}
}
