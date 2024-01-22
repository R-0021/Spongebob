using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerGuns : MonoBehaviour
{
	[System.Serializable]
	public class Gun
	{
		public GameObject gameObject;
		public GameObject gunPoint;
		public int damage = 10;
		public int fireRate = 10;
		public int clipSize = 10;
		public float reloadTime = 1;
		public float recoilForce = 1;
		public float recoilTime = 1;
		public AudioClip gunSound;
	}

	public Gun[] guns = null;

	public float shootingRange = 100f;
	public LayerMask shootableLayer;

	public TMPro.TMP_Text clipText;

	private PlayerRecoil recoil;
	private bool canShoot = true;
	private int currentGun = 0;
	private int clip = 0;

	[SerializeField] private PhotonView playerSetupView;

    [SerializeField] private GameObject vfxHit;
    [SerializeField] private GameObject vfxGunpoint;

	private int CurrentGun
	{
		get { return currentGun; }
		set
		{
			currentGun = Mathf.Clamp(value, 0, guns.Length - 1);

			SwitchGun(currentGun);

        }
	}

	private void Start()
	{
		recoil = GetComponentInChildren<PlayerRecoil>();
		CurrentGun = 0;
	}


	void Update()
	{
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

		if (Physics.Raycast(ray, out hit, shootingRange))
		{
			HandleHitObject(hit.collider.gameObject);
			PhotonNetwork.Instantiate(vfxHit.name, hit.point, Quaternion.identity);
		}	

		GameObject vfx = Instantiate(vfxGunpoint, guns[currentGun].gunPoint.transform.position, Quaternion.identity);
		vfx.transform.SetParent(guns[currentGun].gunPoint.transform);
		Destroy(vfx, 0.5f);

		clip--;
		if (clip <= 0)
			Reload();

		AudioManager.PlayGunSFX(guns[currentGun].gunSound);

		UpdateClipText();
	}

	void HandleHitObject(GameObject hitObject)
	{
		if(hitObject.GetComponent<ZombieHealth>())
			hitObject.GetComponent<PhotonView>()?.RPC("TakeZombieDamage", RpcTarget.AllBuffered, guns[currentGun].damage);
	}

	void SwitchGun(int index)
	{
		playerSetupView.RPC("SetTPWeapon", RpcTarget.All, index);

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
		UpdateClipText();
	}

	void Reload()
	{
		StopAllCoroutines();
		StartCoroutine(HandleReload());
	}

	void UpdateClipText() => clipText.text = $"{clip} / {guns[currentGun].clipSize}";

	IEnumerator HandleReload()
	{
		canShoot = false;
		yield return new WaitForSeconds(guns[currentGun].reloadTime);
		clip = guns[currentGun].clipSize;
		canShoot = true;

		UpdateClipText();
	}

	IEnumerator HandleFireRate()
	{
		canShoot = false;
		yield return new WaitForSeconds(1f / guns[currentGun].fireRate);
		canShoot = true;
	}
}
