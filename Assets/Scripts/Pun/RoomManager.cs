using UnityEngine;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    [SerializeField] private GameObject player;

    [Space]
    [SerializeField] private Transform[] spawnPoints;

    [Space]
    [SerializeField] private GameObject roomCam;

    [Header("UI"), Space]
    [SerializeField] private GameObject nameUI;
    [SerializeField] private GameObject connectingUI;

    
    private string nickName = "unnamed";

    private void Awake() => Instance = this;

    void Start()
    {

    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("Connected to server.");

        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        Debug.Log("We're in the lobby");

        PhotonNetwork.JoinOrCreateRoom("Spongebob Playground", null, null);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.Log("We're connected and in a room now");

        roomCam.SetActive(false);

        SpawnPlayer();
        StartCoroutine(ZombieManager.Instance.SpawnZombies());
    }

    public void SpawnPlayer()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];


        GameObject _player = PhotonNetwork.Instantiate(player.name, spawnPoint.position, Quaternion.identity);

        _player.GetComponent<PlayerSetup>().IsLocalPlayer();

        _player.GetComponent<PlayerHealth>().isLocalPlayer = true;

        _player.GetComponent<PhotonView>().RPC("SetNickname", RpcTarget.AllBuffered, nickName);

        _player.GetComponent<PhotonView>().RPC("ChangeSkin", RpcTarget.AllBuffered, Random.Range(0, _player.GetComponent<PlayerSkin>().skins.Length));
    }

    public void ChangeNickname(string name)
    {
        nickName = name;
    }

    public void JoinRoomButtonPressed()
    {
        Debug.Log("Connecting...");

        PhotonNetwork.ConnectUsingSettings();

        nameUI.SetActive(false);
        connectingUI.SetActive(true);
    }
}
