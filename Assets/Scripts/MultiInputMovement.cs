using UnityEngine;
using System.Collections;

public class MultiInputMovement : MonoBehaviour {

	private InputAction[] lastInput;
	private Every resolveInput;
	public InputAction CurrentAction;

	// Use this for initialization
	void Awake () 
	{
		Debug.Log ("AWAKE DO SCRIPT");
		lastInput = new InputAction[]{InputAction.NoAction, InputAction.NoAction, InputAction.NoAction};
		CurrentAction = InputAction.NoAction;
		Debug.Log ("currentAction inicial" + CurrentAction);

		// Resolve the input every 10seconds
		resolveInput = new Every (0.1f);
		resolveInput.IsTriggered = true;

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (resolveInput.IsTriggered) {
			ResolveInput();
		}
	}

	[RPC]
	void SendMovementInput(NetworkPlayer player, InputAction input)
	{ 
		Debug.Log ("Player " + player.ToString() + " move: " + input);
		lastInput [int.Parse(player.ToString())] = input;
	}

	// Aqui é o carai de como vai mover
	void ResolveInput() {
		Debug.Log("Resolving movement: " + CurrentAction);
		CurrentAction = lastInput[0];
	}
}
