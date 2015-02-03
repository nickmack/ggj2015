using UnityEngine;

public class PlayerScriptTest : MonoBehaviour
{
    public Vector3 speed;

    private Vector3 movement;
    private Animator anim;

    void start()
    {
        rigidbody.freezeRotation = true;

    }

    void Update()
    {
        float inputX = 0;
        float inputZ = 0;
        float rotation = transform.eulerAngles.y;

        
        // 4 - Movement per direction
        if (Input.GetKey(KeyCode.W))
        {
            inputZ = 1;
            rotation = 0;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputZ = -1;
            rotation = 180;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputX = 1;
            rotation = 90;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputX = -1;
            rotation = 270;
        }


        movement = new Vector3(
            speed.x * inputX,
            0, speed.z * inputZ);
        transform.rotation = Quaternion.Euler(90, rotation, 0);


		anim = gameObject.GetComponentInChildren<Animator> ();
        //if (movement.x != 0 || movement.z != 0)
        //{
        //    anim.SetBool("isWalking", true);
        //}
        //else
        //{
        //    anim.SetBool("isWalking", false);
        //}
    }

    void FixedUpdate()
    {
        rigidbody.velocity = movement;
        transform.eulerAngles = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
}