using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private GameObject waitingForPlayersPanel;
	private GameObject createJoinPanel;
	private GameObject serverListPanel;
	private GameObject createGameButton;
	private GameObject joinGameButton;
	private GameObject startGameButton;
	private GameObject serverButton;
	private HostData[] host;
	private NetworkManager networkManager;

	private int playerCount;

	// Use this for initialization
	void Start () {
		// Keep references to what can be changed
		waitingForPlayersPanel = GameObject.Find ("WaitingForPlayersPanel");
		createJoinPanel = GameObject.Find ("CreateJoinPanel");
		serverListPanel = GameObject.Find ("ServerListPanel");
		createGameButton = GameObject.Find ("CreateGameButton");
		joinGameButton = GameObject.Find ("JoinGameButton");
		networkManager = GameObject.Find ("Controller").GetComponent<NetworkManager> ();
		startGameButton = GameObject.Find ("StartGameButton");
		serverButton = GameObject.Find ("ServerButton");

		// Wire events
		// Note: Remember our buttons are not components, but CHILDREN of the GameObjects
		createGameButton.GetComponentInChildren<Button> ().onClick.AddListener (() => CreateGameButton_Click());
		joinGameButton.GetComponentInChildren<Button> ().onClick.AddListener (() => JoinGameButton_Click());
		startGameButton.GetComponent<Button>().onClick.AddListener (() => StartGameButton_Click());


		// Hide everything initially
		waitingForPlayersPanel.SetActive (false);
		serverListPanel.SetActive (false);
		serverButton.SetActive (false);
		startGameButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) 
		{
			if (waitingForPlayersPanel.activeSelf) 
			{
				DisconnectFromGame();
				InitCreateJoinPanel();
			} else if (serverListPanel.activeSelf) 
			{
				InitCreateJoinPanel();
			}
		}
	}

	/* *
	 * Screen changes
	 * 
	 * */

	void InitCreateJoinPanel()
	{
		waitingForPlayersPanel.SetActive(false);
		serverListPanel.SetActive(false);
		createJoinPanel.SetActive(true);
    }

	void InitWaitingForPlayersPanel() 
	{
		serverListPanel.SetActive (false);
		createJoinPanel.SetActive(false);
		waitingForPlayersPanel.SetActive (true);
		GameObject.Find ("ServerAddressLabel").GetComponent<Text>().text = Network.player.ipAddress;
		GameObject.Find ("YourGameLabel").GetComponent<Text>().text = Network.isServer ? "Your game:" : "Server:";
		TriggerUpdateConnectedPlayers ();
	}

	void InitServerListPanel()
	{
		waitingForPlayersPanel.SetActive (false);
		createJoinPanel.SetActive(false);
		serverListPanel.SetActive (true);
		ResetServerButton ();
    }

	void ResetServerButton()
	{
		Debug.Log (serverButton);
		serverButton.SetActive (true);
		serverButton.GetComponentInChildren<Text>().text = "";
		serverButton.GetComponent<Button> ().onClick.RemoveAllListeners ();
		serverButton.SetActive (false);
    }

	void EnableServerButon(HostData host)
	{
		serverButton.SetActive (true);
		serverButton.GetComponentInChildren<Text>().text = host.gameName; // Just the first host
		serverButton.GetComponent<Button>().onClick.AddListener (() => ServerButton_Click(host));
    }

	/* *
	 * Event Handlers
	 * 
	 * */

	public void CreateGameButton_Click()
	{
		networkManager.StartServer (Network.player.ipAddress);
		InitWaitingForPlayersPanel ();
	}

	public void JoinGameButton_Click()
	{
		InitServerListPanel();
		networkManager.RefreshHostList ();
    }
	
	public void StartGameButton_Click()
	{
		Application.LoadLevel ("Main");
	}

	public void ServerButton_Click(HostData host)
	{
		networkManager.JoinServer(host);
		InitWaitingForPlayersPanel ();
    }
    
    /* *
	 * Network Events
	 * 
	 * */

	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}

	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log ("Player connected. IP: " + player.ipAddress);
		playerCount++;
		TriggerUpdateConnectedPlayers();
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived && serverListPanel.activeSelf)
		{
			HostData[] hostList = MasterServer.PollHostList();
			Debug.Log ("Received Host List. Number of hosts: " + hostList.Length);
			if (hostList.Length > 0)
			{
				EnableServerButon(hostList[0]); // Just the first one
			} else
			{
				networkManager.RefreshHostList();
			}
		}
	}
	
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Debug.Log ("OnPlayerDisconnected. IP: " + player.ipAddress);
		Network.CloseConnection (Network.connections [0], false);
		Network.RemoveRPCs (player);
		playerCount--;
		Debug.Log ("First connection: " + Network.connections[0].ipAddress);
		if (waitingForPlayersPanel.activeSelf)
		{
			TriggerUpdateConnectedPlayers();
		}
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
        
        waitingForPlayersPanel.SetActive (false);
        serverListPanel.SetActive (false);
        createJoinPanel.SetActive (true);
    }
    
    void OnServerInitialized()
    {
        Debug.Log("Server Initializied. Connection count = " + Network.connections.Length);
		playerCount++; //The host
        //SpawnPlayer();
    }


	/* *
	 * Methods
	 * 
	 * */
    
    // Only called by server
	void TriggerUpdateConnectedPlayers() 
	{
        UpdateConnectedPlayers(playerCount);
        networkView.RPC("UpdatePlayers", RPCMode.Server, playerCount);
    }
    
	[RPC]
	void UpdatePlayers(int playersConnected) 
	{
		if (Network.isClient) 
		{
			UpdateConnectedPlayers (playersConnected);
		}
	}

	void UpdateConnectedPlayers(int connectedPlayers)
	{
		for (int i = 1; i <= 3; i++)
		{
			Text playerText = GameObject.Find ("Player" + i + "StatusLabel").GetComponent<Text>();

			if (i <= connectedPlayers)
			{
				playerText.text = "OK";
				playerText.color = Color.green;
			} else
			{
				playerText.text = "Waiting...";
				playerText.color = Color.black;
			}
		}

		if (connectedPlayers == 3) 
		{
			startGameButton.SetActive(true);
		}
	}

	void DisconnectFromGame() 
	{
		if (Network.isServer) 
		{
			Debug.Log("Ranayana eu te amo");
			Network.Disconnect ();
			MasterServer.UnregisterHost ();
		} else 
        {
            Network.Disconnect ();
        }
    }

}
