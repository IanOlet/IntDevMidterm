using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {

    public Transform waypoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (waypoint != null)
        {
            Vector3 moveDirection = waypoint.transform.position - transform.position;
            if (moveDirection.magnitude > 1)
            {
                moveDirection = Vector3.Normalize(moveDirection);
            }

            transform.position += moveDirection * Time.deltaTime * 2f;
        }
    }
}
