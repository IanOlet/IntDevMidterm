using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    private Slider pretzels;
    private int effort;

	// Use this for initialization
	void Start () {
        pretzels = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        pretzels.value = effort;
	}

    public bool makePretzels()
    {
        effort += 6;
        if(effort >= 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void resetBaking()
    {
        effort = 0;
        pretzels.value = effort;
    }
}
