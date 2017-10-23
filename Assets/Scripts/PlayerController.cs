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
    public Slider game;
    public SliderController pretzelController;
    public SliderController gameController;
    public GameObject pretzelModel;
    public GameObject doughModel;
    public GameObject TV;
    public GameObject PersonalPretzel;
    public GameObject CPretzel;
    public GameObject EPretzel;
    public GameObject KPretzel;
    public GameObject NPretzel;
    public Material playing, idle, off, pressed;
    public Renderer TelevisionModel;
    public GameObject oldTV;

    public GameObject nearPretzelText;
    public GameObject nearFinishedPretzelText;
    public GameObject nearTVText;
    public GameObject pretzelMakingText;
    public GameObject stopMakingText;
    public GameObject pretzelEatingText;
    public GameObject endText;
    public Text dialogue;
    public Text gameText;
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
    float gameTimer = 5f;
    int score;
    int pretzelsMade = 0;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        pretzelController = pretzels.GetComponent<SliderController>();
        pretzelList = new List<GameObject>();
        gameController = game.GetComponent<SliderController>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Vector3.Distance(transform.position, E.transform.position) < 15f && !enteredDoor)
        {
            dialogue.text = "I'm glad we could all finally get together for this, we've been planning it for so long. We already made the dough, now we just have to shape the pretzels.";
            enteredDoor = true;
            C.waypoint = cPoint1;
            C.focus = doughModel.GetComponent<Transform>();
            E.waypoint = ePoint1;
            E.focus = doughModel.GetComponent<Transform>();
            K.waypoint = kaPoint1.GetComponent<Transform>();
            K.focus = doughModel.GetComponent<Transform>();
            N.waypoint = nPoint1;
            N.focus = doughModel.GetComponent<Transform>();
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

        if(Vector3.Distance(transform.position, TV.transform.position) < 5f && madePretzels && !playedGame && !showPartyScore) //Same as above, but with the TV
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

        if (Input.GetMouseButton(0) && nearTV) //If 2 is pressed when near the TV, start the party game minigame
        {
            dialogue.text = "";
            gameText.gameObject.SetActive(true);
            game.gameObject.SetActive(true);
            partyGame = true;
            gameTimer = 5f;
            //cam.TogglePartyGame(true);
        }

        if (partyGame)
        {
            TelevisionModel.material = playing;
            if (Input.GetKeyDown(KeyCode.Space)) //Eating pretzels is the same as making them, but activated differently. Add particle effects that show eating.
            {
                gameController.progress();
                TelevisionModel.material = pressed;
            }

            gameTimer -= Time.deltaTime;
            gameText.text = "Mash space to play game\nTime Left:\n" + gameTimer;

            if (gameTimer <= 0 && !showPartyScore)
            {
                score = gameController.timeUp();
                gameController.reset();
                PartyGameIsDone();
                gameText.text = "Score: " + score;
                partyGame = false;
                game.gameObject.SetActive(false);
                TelevisionModel.material = idle;
            }
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
            else if (madePretzels && playedGame && !atePretzels)
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

        if(pretzelMake && pretzelsMade >= 5 && Input.GetMouseButton(0)) //Stops making pretzels once you have enough
        {
            dialogue.text = "Now the pretzels have to bake for a while. Why don't we play a game on the tv while we wait?";
            C.waypoint = cPoint2;
            C.focus = oldTV.GetComponent<Transform>();
            E.waypoint = ePoint2;
            E.focus = oldTV.GetComponent<Transform>();
            K.waypoint = kaPoint2;
            K.focus = oldTV.GetComponent<Transform>();
            N.waypoint = nPoint2;
            N.focus = oldTV.GetComponent<Transform>();
            pretzelMakingText.SetActive(false);
            pretzelMake = false;
            pretzels.gameObject.SetActive(false);
            madePretzels = true;
            doughModel.SetActive(false);
            stopMakingText.SetActive(false);
            TelevisionModel.material = idle;
        }

        if(pretzelMake && Input.GetKeyDown(KeyCode.Space)) //If pretzels are being made, mash space to make pretzels. If pretzels are done making, stop making pretzels.
        {
            if(pretzelController.progress())
            {
                GameObject p = Instantiate(pretzelModel, pretzelModel.transform.position, Quaternion.identity);
                pretzelList.Add(p);
                p.SetActive(true);
                pretzelsMade++;
                if (pretzelsMade == 5)
                {
                    dialogue.text = "Okay, that's enough pretzels for everyone. You can make more if you want, I guess.";
                    stopMakingText.SetActive(true);
                }
                if (pretzelsMade == 8)
                {
                    dialogue.text = "That seems like enough.";
                }
                if (pretzelsMade == 12)
                {
                    dialogue.text = "I hope you're going to eat all of those.";
                }
                if (pretzelsMade == 16)
                {
                    dialogue.text = "Stop making pretzels.";
                }
                if (pretzelsMade == 20)
                {
                    dialogue.text = "Oh look, we're out of dough. Now the pretzels have to bake for a while. Why don't we play a game on the tv while we wait?";
                    C.waypoint = cPoint2;
                    C.focus = oldTV.GetComponent<Transform>();
                    E.waypoint = ePoint2;
                    E.focus = oldTV.GetComponent<Transform>();
                    K.waypoint = kaPoint2;
                    K.focus = oldTV.GetComponent<Transform>();
                    N.waypoint = nPoint2;
                    N.focus = oldTV.GetComponent<Transform>();
                    pretzelMakingText.SetActive(false);
                    pretzelMake = false;
                    pretzels.gameObject.SetActive(false);
                    madePretzels = true;
                    doughModel.SetActive(false);
                    stopMakingText.SetActive(false);
                }
                pretzelController.reset();
            }
        }
        if(pretzelEat && Input.GetKeyDown(KeyCode.Space)) //Eating pretzels is the same as making them, but activated differently. Add particle effects that show eating.
        {
            if(pretzelController.progress())
            {
                pretzelEatingText.SetActive(false);
                pretzelEat = false;
                pretzelController.reset();
                PersonalPretzel.SetActive(false);
                CPretzel.SetActive(false);
                EPretzel.SetActive(false);
                KPretzel.SetActive(false);
                NPretzel.SetActive(false);
                pretzels.gameObject.SetActive(false);
                atePretzels = true;
                C.focus = GetComponent<Transform>();
                E.focus = GetComponent<Transform>();
                K.focus = GetComponent<Transform>();
                N.focus = GetComponent<Transform>();
                dialogue.text = "This was a lot of fun, but it's time to go. Hopefully we can see each other again soon.";
            }
        }

        if(atePretzels && Vector3.Distance(transform.position, E.transform.position) > 20f) //Player has done everything and is leaving the house
        {
            dialogue.text = "Bye!";
            endFade.gameObject.SetActive(true);
            //Debug.Log("Starting to fade");
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
            if(scoreTimer >= 3)
            {
                showPartyScore = false;
                partyGame = false;
                gameText.gameObject.SetActive(false);
                //cam.TogglePartyGame(false);
                playedGame = true;
                TelevisionModel.material = off;
                dialogue.text = "That was fun! I think the pretzels are ready, we can eat them before everyone has to leave.";
                C.waypoint = cPoint3;
                E.waypoint = ePoint3;
                K.waypoint = kaPoint3;
                N.waypoint = nPoint3;
                C.focus = N.GetComponent<Transform>();
                E.focus = K.GetComponent<Transform>();
                K.focus = C.GetComponent<Transform>();
                N.focus = E.GetComponent<Transform>();
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
