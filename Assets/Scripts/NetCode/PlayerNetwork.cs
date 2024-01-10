using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestClientRpc();
        }
    }

    [ServerRpc]
    private void TestServerRpc() // runs a method to the server
    {
        Debug.Log("Test Server Rpc " + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc() // runs a method to all the clients from the server or host
    {
        
    }
}
