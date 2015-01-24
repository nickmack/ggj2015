using UnityEngine;
using System.Collections;

public class MultiInputMovement : MonoBehaviour {

	private InputAction[] lastInput;
	private Every resolveInput;
	private InputAction currentAction;

	// Use this for initialization
	void Start () 
	{
		lastInput = new InputAction[]{InputAction.NoAction, InputAction.NoAction, InputAction.NoAction};
		currentAction = InputAction.NoAction;

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
		lastInput [int.Parse(player.ToString())] = input;
	}

	// Aqui é o carai de como vai mover
	void ResolveInput() {
		currentAction = InputAction.MoveRight;
	}

	public InputAction CurrentActionForPlayer 
	{
		get { return currentAction; }
	}
}
