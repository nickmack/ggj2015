using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed = 10f;

    private Vector2 lastClientPos = Vector2.zero;

    //The input values the server will execute on this object
    private Vector2 serverCurrentPos = Vector2.zero;

    void Update()
    {
    	InputMovement();
    }

    void InputMovement()
    {
        // Client movement code
        if (Network.isClient)
        {
            Vector2 newPos = transform.position;
            if (Input.GetKey(KeyCode.W)) {
				newPos = newPos + Vector2.up * speed * Time.deltaTime;
			}

            if (Input.GetKey(KeyCode.S)) {
				newPos = newPos - Vector2.up * speed * Time.deltaTime;
			}

            if (Input.GetKey(KeyCode.D)) {
				newPos = newPos + Vector2.right * speed * Time.deltaTime;
			}

            if (Input.GetKey(KeyCode.A)) {
				newPos = newPos - Vector2.right * speed * Time.deltaTime;
			}

            //Is our input different? Do we need to update the server?
            if (lastClientPos.x != newPos.x || lastClientPos.y != newPos.y)
			{
				Debug.Log ("Client move from " + lastClientPos.ToString() + " to " + newPos.ToString());
                lastClientPos = newPos;
                networkView.RPC("SendMovementInput", RPCMode.Server, newPos.x, newPos.y);
            }
        }

        //Server movement code
        if(Network.isServer){//Also enable this on the client itself: "|| Network.player==owner){|"
            //Actually move the player using his/her input
			transform.position = serverCurrentPos;
        }
    }

    [RPC]
    void SendMovementInput(float x, float y){ 
        //Called on the server
        serverCurrentPos = new Vector2(x, y);
    }

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody2D.position;
			stream.Serialize(ref syncPosition);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			transform.position = syncPosition;
		}
	}
    
}