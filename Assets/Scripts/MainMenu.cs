using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {
	private GameObject waitingForPlayersPanel;
	private GameObject createJoinPanel;
	private GameObject serverListPanel;
	private GameObject createGameButton;
	private GameObject joinGameButton;
	private NetworkManager networkManager;

	// Use this for initialization
	void Start () {
		// Keep references to what can be changed
		waitingForPlayersPanel = GameObject.Find ("WaitingForPlayersPanel");
		createJoinPanel = GameObject.Find ("CreateJoinPanel");
		serverListPanel = GameObject.Find ("ServerListPanel");
		createGameButton = GameObject.Find ("CreateGameButton");
		joinGameButton = GameObject.Find ("JoinGameButton");
		networkManager = Camera.main.GetComponent<NetworkManager> ();

		// Wire events
		// Note: Remember our buttons are not components, but CHILDREN of the GameObjects
		createGameButton.GetComponentInChildren<Button> ().onClick.AddListener (() => CreateGameButton_Click());
		joinGameButton.GetComponentInChildren<Button> ().onClick.AddListener (() => JoinGameButton_Click());

		// Hide everything initially
		waitingForPlayersPanel.SetActive (false);
		serverListPanel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateGameButton_Click()
	{
		createJoinPanel.SetActive (false);
		networkManager.StartServer (Network.player.externalIP);
		waitingForPlayersPanel.SetActive (true);
		Debug.Log (GameObject.Find ("ServerAddressLabel"));
		GameObject.Find ("ServerAddressLabel").GetComponentInChildren<Text>().text = Network.player.externalIP;
	}

	void OnConnectedToServer()
	{
		Debug.Log("Server Joined");
	}

	public void JoinGameButton_Click()
	{
		createJoinPanel.SetActive (false);
		serverListPanel.SetActive (true);
	}
}
