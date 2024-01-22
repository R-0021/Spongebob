using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance { get; private set; }

    [SerializeField] private int limit = 10;


    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    [SerializeField] private GameObject zombie;

    [Space]
    [SerializeField] private Transform[] spawnPoints;

    public void SpawnZombie()
    {
        //if (ZombieCount.Count() >= limit) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject _zombie = PhotonNetwork.Instantiate(zombie.name, spawnPoint.position, Quaternion.identity);

    }

    public IEnumerator SpawnZombies()
    {
        while(true)
        {
			yield return new WaitForSeconds(3);
            if (ZombieCount.Count() <= limit)
            {
			    SpawnZombie();
			    SpawnZombie();
			    SpawnZombie();  
                ZombieCount.Increase(3);
            } 
		}
    }
}
