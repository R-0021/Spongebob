using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerGunsNetwork : NetworkBehaviour
{
	[System.Serializable]
	public class Gun
	{
		public GameObject gameObject;
		public int damage = 10;
		public int fireRate = 10;
		public int clipSize = 10;
		public float reloadTime = 1;
		public float recoilForce = 1;
		public float recoilTime = 1;
	}

	public Gun[] guns = null;

	public float shootingRange = 100f;
	public LayerMask shootableLayer;

	private PlayerRecoil recoil;
	private bool canShoot = true;
	private int currentGun = 0;
	private int clip = 0;

	private int CurrentGun
	{
		get { return currentGun; }
		set
		{
			currentGun = Mathf.Clamp(value, 0, guns.Length - 1);

			if(IsOwner)
				SwitchGunServerRpc(currentGun);
		}
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
		recoil = transform.root.GetComponentInChildren<PlayerRecoil>();
		CurrentGun = 0;
	}

	void Update()
	{
		if (!IsOwner) return;

		if (canShoot && Input.GetMouseButton(0))
		{
			StartCoroutine(HandleFireRate());
			Shoot();
		}

		if (Input.GetKeyDown(KeyCode.E))
			CurrentGun++;
		if (Input.GetKeyDown(KeyCode.Q))
			CurrentGun--;
	}

	void Shoot()
	{
		recoil.Apply();

		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, shootingRange, shootableLayer))
			HandleHitObject(hit.collider.gameObject);

		clip--;
		if (clip <= 0)
			Reload();
	}

	void HandleHitObject(GameObject hitObject)
	{
		Destroy(hitObject);
	}

	[ServerRpc]
	private void SwitchGunServerRpc(int index)
	{
		SwitchGunClientRpc(index);
	}

	[ClientRpc]
	private void SwitchGunClientRpc(int index)
	{
		SwitchGun(index);
	}

	void SwitchGun(int index)
	{
		foreach (var gun in guns) 
		{
			gun.gameObject.SetActive(false);
		}

		guns[index].gameObject.SetActive(true);

		recoil.SetForce(guns[index].recoilForce);
		recoil.SetTime(guns[index].recoilTime);
		clip = guns[index].clipSize;
		StopAllCoroutines();
		canShoot = true;
	}

	void Reload()
	{
		StopAllCoroutines();
		StartCoroutine(HandleReload());
	}

	IEnumerator HandleReload()
	{
		canShoot = false;
		yield return new WaitForSeconds(guns[currentGun].reloadTime);
		clip = guns[currentGun].clipSize;
		canShoot = true;
	}

	IEnumerator HandleFireRate()
	{
		canShoot = false;
		yield return new WaitForSeconds(1f / guns[currentGun].fireRate);
		canShoot = true;
	}
}
