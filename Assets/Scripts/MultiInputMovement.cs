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
		if (Network.isServer) {
			if (resolveInput.IsTriggered) {
					ResolveInput ();
			}
		}
	}

	[RPC]
	void SendMovementInput(NetworkPlayer player, int input)
	{ 
		if (Network.isServer) {
			InputAction inputAction = (InputAction)input;
			Debug.Log ("Player " + player.ToString () + " move: " + inputAction);
			lastInput [int.Parse (player.ToString ())] = inputAction;
		}
	}

	// Aqui é o carai de como vai mover
	void ResolveInput() {
		if (Network.isServer) {
			Debug.Log("Resolving action: " + lastInput[0] + lastInput[1] + lastInput[2]);
			if (lastInput[0] == lastInput[1] && lastInput[1] == lastInput[2]) {
				CurrentAction = lastInput[0];
			} else {
				CurrentAction = InputAction.NoAction;
			}
			Debug.Log("Resulting action: " + CurrentAction);
		}
	}
}
