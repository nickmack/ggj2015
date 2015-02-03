using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 10f;

	private InputAction lastClientInput = InputAction.NoAction;

    //The input values the server will execute on this object
	private InputAction serverCurrentInput = InputAction.NoAction;

	private Every checkInput;
	private float checkInputInterval = 0.1f;

	void Start()
	{
		checkInput = new Every (checkInputInterval);
	}

    void Update()
    {
		if (checkInput.IsTriggered) {
			CheckInput();
		}

		ExecuteAction();
    }

	void CheckInput()
    {
		Debug.Log ("CheckInput");
        // Client movement code
		Debug.Log ("Client");
		InputAction desiredAction = InputAction.NoAction;
	    if (Input.GetKey(KeyCode.W)) {
			desiredAction = InputAction.MoveUp;
			//newPos = newPos + Vector2.up * speed * Time.deltaTime;
		}

    	if (Input.GetKey(KeyCode.S)) {
			desiredAction = InputAction.MoveDown;
			//newPos = newPos - Vector2.up * speed * Time.deltaTime;
		}

    	if (Input.GetKey(KeyCode.D)) {
			desiredAction = InputAction.MoveRight;
			//newPos = newPos + Vector2.right * speed * Time.deltaTime;
		}

    	if (Input.GetKey(KeyCode.A)) {
			desiredAction = InputAction.MoveLeft;
			//newPos = newPos - Vector2.right * speed * Time.deltaTime;
		}

		if (Input.GetKey(KeyCode.Space)) {
			desiredAction = InputAction.Attack;
		}

	    //TODO Is our input different? Do we need to update the server?
		//TODO improve this like before
	    Debug.Log ("Client Intention changed from: " + lastClientInput + " to " + desiredAction);
	    lastClientInput = desiredAction;
		if (Network.isClient) {
			networkView.RPC ("SendMovementInput", RPCMode.Server, Network.player, (int)desiredAction);
		} else {
			this.GetComponent<MultiInputMovement>().SendMovementInput(Network.player, (int) desiredAction);
		}
        
    }

	void ExecuteAction()
	{
		//Server movement code
        if(Network.isServer){

			InputAction currentAction = this.gameObject.GetComponent<MultiInputMovement>().CurrentAction;
			Debug.Log ("Server moving player: " + currentAction);

			switch (currentAction) 
			{
				case InputAction.MoveUp:
					transform.position = (Vector2) transform.position + Vector2.up * speed * Time.deltaTime;
					break;
				case InputAction.MoveDown:
					transform.position = (Vector2) transform.position - Vector2.up * speed * Time.deltaTime;
					break;
				case InputAction.MoveRight:
					transform.position = (Vector2) transform.position + Vector2.right * speed * Time.deltaTime;
					break;
				case InputAction.MoveLeft:
					transform.position = (Vector2) transform.position - Vector2.right * speed * Time.deltaTime;
					break;
				case InputAction.NoAction:
					transform.position = (Vector2) transform.position;
                    break;
				default:
					break;
			}
        }
	}
    
}

public enum InputAction {
	MoveUp,
	MoveDown,
	MoveLeft,
	MoveRight,
	Attack,
	NoAction
}