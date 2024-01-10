using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    private string nickname;

    [SerializeField] TextMeshPro nicknameText;
    [SerializeField] TMP_Text uiNameText;

    [SerializeField] private Transform TPWeaponHolder;

    public void IsLocalPlayer()
    {
        TPWeaponHolder.gameObject.SetActive(false);

        GetComponent<PlayerGuns>().enabled = true;
		GetComponent<PlayerMovement>().enabled = true;
		GetComponentInChildren<Camera>(true).gameObject.SetActive(true);
    }

    [PunRPC]
    public void SetTPWeapon(int _wIndex)
    {
        foreach (Transform weapon in TPWeaponHolder)
            weapon.gameObject.SetActive(false);

        TPWeaponHolder.GetChild(_wIndex).gameObject.SetActive(true);
    }

    [PunRPC]
    public void SetNickname(string _name)
    {
        nickname = _name;
        nicknameText.text = nickname;
        uiNameText.text = nickname;
    }
}
