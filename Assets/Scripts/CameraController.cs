using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour {

    float sensitivity = 100f;
    float verticalLook = 0f;
    public float totalSpaz = 0f;
    public bool measureSpaz = false;
    float timer = 0;

    public Text gameScore;
    PlayerController player;

	// Use this for initialization
	void Start () {
        player = transform.parent.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if(measureSpaz)//if the game wants to measure how much you spaz your mouse, measure mouse movements
        {
            totalSpaz += (Mathf.Abs(mouseX) + Mathf.Abs(mouseY));
            gameScore.text = "Spaz the mouse for artistic expression. \nScore " + totalSpaz;
            timer += Time.deltaTime;
            if(timer >= 4) //Tell the playercontroller that the game is done, and send a number based on score
            {
                measureSpaz = false;
                if (totalSpaz < 2750f) //Low score
                {
                    player.PartyGameIsDone();
                    gameScore.text += "\nYou didn't do very well...";
                }
                else if (totalSpaz < 5000f) //Medium score
                {
                    player.PartyGameIsDone();
                    gameScore.text += "\nYou did okay.";
                }
                else
                {
                    player.PartyGameIsDone();
                    gameScore.text += "\nYou won the party game. Tryhard.";
                }
                
            }
        }
        else
        {
            totalSpaz = 0f;
        }

        transform.parent.Rotate(0f, mouseX * Time.deltaTime * sensitivity, 0f);

        verticalLook -= mouseY * Time.deltaTime * sensitivity;
        verticalLook = Mathf.Clamp(verticalLook, -85f, 85f);

        transform.localEulerAngles = new Vector3(verticalLook, transform.localEulerAngles.y, 0f);

        if(Input.GetMouseButtonDown(0))
        {
            if(!Cursor.visible)
            {
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
            }

            Cursor.lockState = CursorLockMode.Locked;

        }
	}

    public void TogglePartyGame(bool state) //Toggles whether or not the game measures camera spaz
    {
        if (!state)
        {
            measureSpaz = false;
            gameScore.gameObject.SetActive(false);
        }
        else
        {
            measureSpaz = true;
            gameScore.gameObject.SetActive(true);
            timer = 0;
            totalSpaz = 0;
        }
    }
}
