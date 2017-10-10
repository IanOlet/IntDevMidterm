using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

    public bool opening = true;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (opening && transform.localEulerAngles.y >= 10)
        {
            transform.Rotate(0, -1, 0);
        }
	}

    public void openDoor() //Call this to open the door
    {
        opening = true;
    }
}
