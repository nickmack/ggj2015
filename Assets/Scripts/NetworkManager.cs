using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
   
    #region Private attributes

    private const string typeName = "GGJ2015";
    private const string gameName = "mansion";

    private HostData[] hostList;

    private const int PORT = 22222;

    #endregion

    #region Public attributes

    public GameObject playerPrefab;
	public GameObject directorPrefab;

    #endregion


    void OnServerInitialized()
    {
        Debug.Log("Server Initializied");
		SpawnPlayer(SpawnDirector());
    }

	private GameObject SpawnDirector()
	{
		return (GameObject)Network.Instantiate(directorPrefab, new Vector2(0f, 0f), Quaternion.identity, 0);
	}

    private void SpawnPlayer(GameObject director)
    {
        GameObject player = (GameObject) Network.Instantiate(playerPrefab, new Vector2(0f, 0f), Quaternion.identity, 0);
		player.GetComponent<PlayerScript>().director = director;
    }


    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
                StartServer();

            if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
                RefreshHostList();

            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }


    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }


    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }


    private void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    private void StartServer()
    {
        Network.InitializeServer(4, PORT, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

}
