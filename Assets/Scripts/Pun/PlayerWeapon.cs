using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private int damage;

    [SerializeField] private Camera camera;

    [SerializeField] private float fireRate;

    [Header("VFX")]
    [SerializeField] private GameObject vfxHit;

    private float nextFire;

    void Update()
    {
        if (nextFire > 0)
            nextFire -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            nextFire = 1 / fireRate;

            Fire();
        }
    }

    private void Fire()
    {
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f))
        {
            PhotonNetwork.Instantiate(vfxHit.name, hit.point, Quaternion.identity);

            if (hit.transform.gameObject.GetComponent<PlayerHealth>())
                hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);

        }
    }
}
