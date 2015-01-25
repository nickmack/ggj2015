using UnityEngine;
using System.Collections;

public class ZombieScript : MonoBehaviour {

    public Transform target;
    private NavMeshAgent navComponent;

	// Use this for initialization
	void Start () {
        this.navComponent = this.transform.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

        if (target)
        {
            this.navComponent.SetDestination(target.position);
        }
	
	}
}
