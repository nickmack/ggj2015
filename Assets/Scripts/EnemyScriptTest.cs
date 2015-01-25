using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EnemyScriptTest : MonoBehaviour
{

    private Transform target;

    private Vector2 movement;

    public Vector2 speed;

    public double range;

    // Use this for initialization
    void Start()
    {
        this.target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(target.position, transform.position) <= range)
        {

            Vector3 dir = target.position - transform.position;

            // Normalize it so that it's a unit direction vector
            dir.Normalize();
            Debug.Log(dir.x);

            int inputX = 0;
            int inputY = 0; 

            if (dir.x != 0)
            {
                inputX = dir.x < 0 ? -1 : 1;
            }

            if (dir.y != 0)
            {
                inputY = dir.y < 0 ? -1 : 1;
            }

            movement = new Vector2(
                speed.x * inputX,
                speed.y * inputY);
        }
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = movement;
    }
}