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

        // 4 - Movement per direction
        if (Input.GetKey(KeyCode.W))
        {
            inputZ = 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputZ = -1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputX = 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputX = -1;
        }


        movement = new Vector3(
            speed.x * inputX,
            0, speed.z * inputZ);

        anim = GetComponent<Animator>();
        if (movement.x != 0 || movement.z != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = movement;
        transform.eulerAngles = Vector3.zero;
        rigidbody2D.angularVelocity = 0f;
    }
}