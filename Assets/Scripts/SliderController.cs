using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour {

    private Slider slide;
    private int effort = 0;

    public bool timed;

	// Use this for initialization
	void Start () {
        slide = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slide.value = effort;
	}

    public bool progress()
    {
        if (timed)
        {
            effort += 2;
            return false;
        }
        else
        {
            effort += 6;
            if (effort >= 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public int timeUp()
    {
        return effort;
    }

    public void reset()
    {
        effort = 0;
        slide.value = effort;
    }
}
