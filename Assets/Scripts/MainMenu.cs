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
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) 
		{
			if (waitingForPlayersPanel.activeSelf) 
			{
				DisconnectFromGame();
				waitingForPlayersPanel.SetActive(false);
				createJoinPanel.SetActive(true);
			} else if (serverListPanel.activeSelf) 
			{
				serverListPanel.SetActive(false);
				createJoinPanel.SetActive(true);
			}
		}
	}

	void DisconnectFromGame() 
	{
		if (Network.isServer) 
		{
			Network.Disconnect ();
			MasterServer.UnregisterHost ();
		} else 
		{
			Network.Disconnect ();
		}
	}

	/* *
	 * Event Handlers
	 * 
	 * */

	public void CreateGameButton_Click()
	{
		createJoinPanel.SetActive (false);
		networkManager.StartServer (Network.player.externalIP);
		waitingForPlayersPanel.SetActive (true);
		GameObject.Find ("ServerAddressLabel").GetComponentInChildren<Text>().text = Network.player.externalIP;
	}

	public void JoinGameButton_Click()
	{
		createJoinPanel.SetActive (false);
		serverListPanel.SetActive (true);
		networkManager.RefreshHostList ();
	}
	
	public void StartGameButton_Click()
	{
		Application.LoadLevel ("Main");
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
		Debug.Log ("Player connected.");
		if (Network.isServer) 
		{
			TriggerUpdateConnectedPlayers();
		}
	}

	// Only called by server
	void TriggerUpdateConnectedPlayers() 
	{
		if (Network.isServer) 
		{
			int playersConnected = Network.connections.Length + 1;
			UpdateConnectedPlayers(playersConnected);
			networkView.RPC("UpdatePlayers", RPCMode.Server, playersConnected);
		}
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
		for (int i = 1; i <= connectedPlayers; i++)
		{
			Debug.Log ("Label to change: Player" + i + "StatusLabel. Found GO?: " + GameObject.Find ("Player" + i + "StatusLabel"));
			GameObject.Find ("Player" + i + "StatusLabel").GetComponent<Text>().text = "OK";
			GameObject.Find ("Player" + i + "StatusLabel").GetComponent<Text>().color = Color.green;
		}
	}

	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
		{
			Debug.Log ("Received Host List");
			HostData[] hostList = MasterServer.PollHostList();
			Debug.Log("Number of hosts: " + hostList.Length);

			for (int i = 0; i < hostList.Length; i++)
			{
				serverButton.SetActive (true);
				serverButton.GetComponentInChildren<Text>().text = hostList[i].gameName;
				int hostIndex = i;
				serverButton.GetComponent<Button>().onClick.AddListener (() => {
					networkManager.JoinServer(hostList[hostIndex]);
					serverListPanel.SetActive(false);
					waitingForPlayersPanel.SetActive(true);
				});
			}
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		if (Network.isServer) 
		{
			TriggerUpdateConnectedPlayers();
		}
	}
}
