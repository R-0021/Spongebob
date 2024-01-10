using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSkin : MonoBehaviour
{
    public GameObject[] skins;

	[PunRPC]
    public void ChangeSkin(int index)
    {
        foreach (var skin in skins)
        {
            skin.SetActive(false);
        }

        skins[index].SetActive(true);
    }
}
