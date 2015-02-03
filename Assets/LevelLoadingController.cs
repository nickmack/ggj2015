using UnityEngine;
using System.Collections;

public class LevelLoadingController : MonoBehaviour {

	public GameObject playerPrefab;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded(int level) {
		if (level == 1) // Main
		{
			if (Network.isServer)
			{
				SpawnPlayer();
			}
		}
		
	}

	private void SpawnPlayer()
	{
		GameObject player = (GameObject) Network.Instantiate(playerPrefab, new Vector2(0f, 0f), Quaternion.identity, 0);
	}
}
