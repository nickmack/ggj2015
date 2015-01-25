using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
   
    #region Private attributes

    private const string typeName = "GGJ2015";
    private string gameName = "mansion";

    private HostData[] hostList;

    private const int PORT = 22222;

    #endregion

    #region Public attributes

    public GameObject playerPrefab;

    #endregion

	void Awake() 
	{
		DontDestroyOnLoad (this);
	}

    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
		//SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject player = (GameObject) Network.Instantiate(playerPrefab, new Vector2(0f, 0f), Quaternion.identity, 0);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		if (Network.isServer)
        {
            Debug.Log("DISCONNECTED: Local server connection disconnected");
        }
		else
        {
			if (info == NetworkDisconnection.LostConnection)
            {
				Debug.Log ("DISCONNECTED: Lost connection to the server");
            } 
            else 
            {
				Debug.Log ("DISCONNECTED: Successfully diconnected from the server");
            }
		}
	}

    void OnPlayerDisconnected(NetworkPlayer player) {
        //Debug.Log("Clean up after player " + player);
        //Network.RemoveRPCs(player);
        //Network.DestroyPlayerObjects(player);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }


    public void RefreshHostList()
    {
		Debug.Log ("Requesting host list");
        MasterServer.RequestHostList(typeName);
    }

    public void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    public void StartServer(string serverName)
    {
		if (string.IsNullOrEmpty(serverName)) {
			serverName = gameName;
		}

        Network.InitializeServer(4, PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, serverName);
    }

	void OnApplicationQuit()
	{
		Network.Disconnect ();
		MasterServer.UnregisterHost ();
	}

}
