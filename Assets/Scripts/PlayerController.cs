using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    //test to check door opening

    Vector3 inputVector;
    Rigidbody rb;
    public CameraController cam;
    public Slider pretzels;
    public SliderController pretzelController;
    public GameObject pretzelModel;
    public GameObject doughModel;
    public GameObject TV;
    public GameObject PersonalPretzel;
    public GameObject CPretzel;
    public GameObject EPretzel;
    public GameObject KPretzel;
    public GameObject NPretzel;

    public GameObject nearPretzelText;
    public GameObject nearFinishedPretzelText;
    public GameObject nearTVText;
    public GameObject pretzelMakingText;
    public GameObject pretzelEatingText;
    public GameObject endText;
    public Text dialogue;
    public Image endFade;

    bool endTime = false;
    float endTimer = 0;

    public ActorController C;
    public ActorController E;
    public ActorController K;
    public ActorController N;

    public Transform cPoint1;
    public Transform cPoint2;
    public Transform cPoint3;
    public Transform ePoint1;
    public Transform ePoint2;
    public Transform ePoint3;
    public Transform kaPoint1;
    public Transform kaPoint2;
    public Transform kaPoint3;
    public Transform nPoint1;
    public Transform nPoint2;
    public Transform nPoint3;

    public float moveSpeed = 5f;

    List<GameObject> pretzelList;

    public bool partyGame;
    public bool pretzelMake;
    public bool pretzelEat; //Set this to true at the end, when they're eating pretzels
    bool nearTV = false;
    bool nearDough = false;

    bool enteredDoor = false;
    bool madePretzels = false;
    bool playedGame = false;
    bool atePretzels = false;

    bool showPartyScore;
    int partyPerformance;
    float scoreTimer;
    int pretzelsMade = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        pretzelController = pretzels.GetComponent<SliderController>();
        pretzelList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, E.transform.position) < 15f && !enteredDoor)
        {
            dialogue.text = "I'm glad we could all finally get together for this, we've been planning it for so long. We already made the dough, now we just have to shape the pretzels.";
            enteredDoor = true;
            C.waypoint = cPoint1;
            E.waypoint = ePoint1;
            K.waypoint = kaPoint1;
            N.waypoint = nPoint1;
        }


        if(Vector3.Distance(transform.position, doughModel.transform.position) < 2f && enteredDoor && !pretzelMake && !pretzelEat && (!madePretzels || (madePretzels && playedGame && !atePretzels))) //Checks if the player is close to the dough, if so, allow the player to start the pretzel minigame
        {
            nearDough = true;
            if (!madePretzels)
            {
                nearPretzelText.SetActive(true);
            }
            else
            {
                nearFinishedPretzelText.SetActive(true);
            }
        }
        else if (nearDough)
        {
            nearDough = false;
            nearPretzelText.SetActive(false);
            nearFinishedPretzelText.SetActive(false);
        }

        if(Vector3.Distance(transform.position, TV.transform.position) < 5f && madePretzels && !playedGame) //Same as above, but with the TV
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

        if (Input.GetMouseButton(0) && nearTV && madePretzels && !playedGame) //If 2 is pressed when near the TV, start the party game minigame
        {
            dialogue.text = "";
            partyGame = true;
            cam.TogglePartyGame(true);
        }
        if (Input.GetMouseButton(0) && nearDough) //If left click is pressed when near pretzels, start making pretzels or stop the minigame
        {
            /*if(pretzelMake)
            {
                pretzelMake = false;
                pretzelMakingText.SetActive(false);
                pretzelController.resetBaking();
                pretzels.gameObject.SetActive(false);
            }*/
            if (!madePretzels)
            {
                pretzelMake = true;
                pretzelMakingText.SetActive(true);
                pretzels.gameObject.SetActive(true);
                dialogue.text = "We need to make five pretzels, one for each of us.";
            }
            else
            {
                pretzelEat = true;
                pretzels.gameObject.SetActive(true);
                pretzelEatingText.SetActive(true);
                PersonalPretzel.SetActive(true);
                CPretzel.SetActive(true);
                EPretzel.SetActive(true);
                KPretzel.SetActive(true);
                NPretzel.SetActive(true);
                foreach(GameObject g in pretzelList)
                {
                    g.SetActive(false);
                }
                dialogue.text = "";
            }
        }
        if(pretzelMake && Input.GetKeyDown(KeyCode.Space)) //If pretzels are being made, mash space to make pretzels. If pretzels are done making, stop making pretzels.
        {
            if(pretzelController.makePretzels())
            {
                GameObject p = Instantiate(pretzelModel, pretzelModel.transform.position, Quaternion.identity);
                pretzelList.Add(p);
                p.SetActive(true);
                pretzelsMade++;
                if (pretzelsMade == 5)
                {
                    dialogue.text = "Now the pretzels have to bake for a while. Why don't we play one of those drawing games on the tv while we wait?";
                    C.waypoint = cPoint2;
                    E.waypoint = ePoint2;
                    K.waypoint = kaPoint2;
                    N.waypoint = nPoint2;
                    pretzelMakingText.SetActive(false);
                    pretzelMake = false;
                    pretzels.gameObject.SetActive(false);
                    madePretzels = true;
                    doughModel.SetActive(false);
                }
                pretzelController.resetBaking();
            }
        }
        if(pretzelEat && Input.GetKeyDown(KeyCode.Space)) //Eating pretzels is the same as making them, but activated differently. Add particle effects that show eating.
        {
            if(pretzelController.makePretzels())
            {
                pretzelEatingText.SetActive(false);
                pretzelEat = false;
                pretzelController.resetBaking();
                PersonalPretzel.SetActive(false);
                pretzels.gameObject.SetActive(false);
                atePretzels = true;
                dialogue.text = "This was a lot of fun, but it's time to go. Hopefully we can see each other again soon.";
            }
        }

        if(atePretzels && Vector3.Distance(transform.position, E.transform.position) > 20f) //Player has done everything and is leaving the house
        {
            dialogue.text = "Bye!";
            endFade.gameObject.SetActive(true);
            Debug.Log("Starting to fade");
            //endFade.canvasRenderer.SetAlpha(0f);
            //endFade.CrossFadeAlpha(1f, 3f, true);
            //endFade.CrossFadeColor(Color.black, 3f, true, true);
            endTime = true;
        }

        if(Input.GetKey(KeyCode.C)) //Cheats to the end, used for testing
        {
            atePretzels = true;
        }

        if(endTime && endTimer < 5.5) //Handles the endgame text showing up after the fade
        {
            endTimer += Time.deltaTime;
            //endFade.color = Color.Lerp(endFade.color, Color.black, Time.deltaTime * 1.5f);
            endFade.color += Color.black * Time.deltaTime * 0.2f;
        }
        else if (endTime)
        {
            endText.SetActive(true);
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
                playedGame = true;
                dialogue.text = "That was fun! I think the pretzels are ready, we can eat them before everyone has to leave.";
                C.waypoint = cPoint3;
                E.waypoint = ePoint3;
                K.waypoint = kaPoint3;
                N.waypoint = nPoint3;
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

    public void PartyGameIsDone ()
    {
        showPartyScore = true;
        scoreTimer = 0;
    }

}
