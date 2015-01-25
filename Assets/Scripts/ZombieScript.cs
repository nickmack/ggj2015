using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {

    private Transform target;
    private NavMeshAgent navComponent;

	// Use this for initialization
	void Start () {
        this.navComponent = this.transform.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        this.target = GameObject.FindWithTag("Player").transform;

        if (this.target)
        {
            this.navComponent.SetDestination(this.target.position);
        }
	
	}
}
