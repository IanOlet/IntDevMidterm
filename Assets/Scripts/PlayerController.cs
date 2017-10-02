using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    Vector3 inputVector;
    Rigidbody rb;
    public CameraController cam;
    public Slider pretzels;
    public SliderController pretzelController;
    public GameObject pretzelModel;
    public GameObject doughModel;
    public GameObject TV;

    public GameObject nearPretzelText;
    public GameObject nearTVText;
    public GameObject pretzelMakingText;

    public float moveSpeed = 5f;

    public bool partyGame;
    public bool pretzelMake;
    bool nearTV = false;
    bool nearDough = false;

    bool showPartyScore;
    int partyPerformance;
    float scoreTimer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        pretzelController = pretzels.GetComponent<SliderController>();
	}
	
	// Update is called once per frame
	void Update () {

        if(Vector3.Distance(transform.position, doughModel.transform.position) < 2f) //Checks if the player is close to the dough, if so, allow the player to start the pretzel minigame
        {
            nearDough = true;
            nearPretzelText.SetActive(true);
        }
        else if (nearDough)
        {
            nearDough = false;
            nearPretzelText.SetActive(false);
        }

        if(Vector3.Distance(transform.position, TV.transform.position) < 5f) //Same as above, but with the TV
        {
            nearTV = true;
            nearTVText.SetActive(true);
        }
        else if (nearTV)
        {
            nearTV = false;
            nearTVText.SetActive(false);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        inputVector = transform.right * horizontal + transform.forward * vertical;

        if(inputVector.magnitude > 1f)
        {
            inputVector = Vector3.Normalize(inputVector);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && nearTV) //If 2 is pressed when near the TV, start the party game minigame
        {
            if (partyGame)
            {
                partyGame = false;
                cam.TogglePartyGame(false);
            }
            else
            {
                partyGame = true;
                cam.TogglePartyGame(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && nearDough) //If 1 is pressed when near pretzels, start making pretzels or stop the minigame
        {
            if(pretzelMake)
            {
                pretzelMake = false;
                pretzelMakingText.SetActive(false);
                pretzelController.resetBaking();
                pretzels.gameObject.SetActive(false);
            }
            else
            {
                pretzelMake = true;
                pretzelMakingText.SetActive(true);
                pretzels.gameObject.SetActive(true);
                doughModel.SetActive(true);
                pretzelModel.SetActive(false);
            }
        }
        if(pretzelMake && Input.GetKeyDown(KeyCode.Space)) //If pretzels are being made, mash space to make pretzels. If pretzels are done making, stop making pretzels.
        {
            if(pretzelController.makePretzels())
            {
                pretzelMakingText.SetActive(false);
                pretzelMake = false;
                pretzelController.resetBaking();
                pretzels.gameObject.SetActive(false);
                pretzelModel.SetActive(true);
                doughModel.SetActive(false);
            }
        }

        if(showPartyScore) //After the party game is over, shows the score and comments on performance
        {
            scoreTimer += Time.deltaTime;
            //Show message based on performance
            if(scoreTimer >= 3)
            {
                showPartyScore = false;
                partyGame = false;
                cam.TogglePartyGame(false);
            }
        }

	}

    void FixedUpdate()
    {
        if(inputVector.magnitude > 0.01f)
        {
            rb.velocity = inputVector * 5f;
        }
    }

    public void PartyGameIsDone (int performance)
    {
        scoreTimer = 0;
        partyPerformance = performance;
        //Set messages to active based on partyPerformance
        //0: Subtle nuanced humor was lost on these philistines.
        //1: You did okay.
        //2: You managed to tryhard in a fun party game. Great job.
        showPartyScore = true;
    }

}
