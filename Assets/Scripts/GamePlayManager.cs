using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Networking;
using System.Text;
using SimpleJSON;

public class GamePlayManager : MonoBehaviour {

	// Use this for initialization
	private bool isHold, isDartThrown;

	public GameObject dart;
    // public TextMesh  HUDToastText;

    private GameObject currentDart;
    public GameObject hudCanvas;
    public GameObject toastBg;
    private int[] throwResultArray;

    private bool isGameScreenInteractable;

    private Vector3 CAMERA_MEAN_POS = new Vector3(2.55f,18.5f,0.0f);
    private Vector3 CAMERA_INIT_POS = new Vector3(-5.0f,18.5f, -10.0f);

    


    

    int noOfDarts = 0;
    int noOfDartsThrown = 0;

    float xyForce = 6.0f;
    float zForce = 9.5f;

    int holdingCounter = 0;
    Vector2 startPos;
    Vector2 endPos;
    Vector2 currentPoint;
    Vector2 direction;

	float swipeStartTime;
    float swipeEndTime;
    float swipeDuration;

    int currentBetColor;
	
	void Start () {
        //cheat

        isGameScreenInteractable = false;

        if(GameManager.GetInstance().lobbyEntryTime)
        {
            CreateScreenToast("",1);
            GetComponent<Camera>().transform.position = CAMERA_INIT_POS;
            transform.DOMove(CAMERA_MEAN_POS, 4);
            GameManager.GetInstance().lobbyEntryTime = false;
            Invoke("InitGame", 4);
        }
        else{
            GetComponent<Camera>().transform.position = CAMERA_MEAN_POS;
            InitGame();
        }
	}

    private void InitGame()
    {
        isDartThrown =false;
        hudCanvas.GetComponent<CommentaryManager>().ResetIndicatorPanel();

        string toasterMsg = "";
        noOfDarts = 3;

        if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
        {
            currentBetColor = Random.Range(1,7);
        }
        else if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.SinglebetMode)
        {
            currentBetColor =  GameManager.playerBets[0].BetColour;
        }

        switch(currentBetColor)
        {
            case 1:
                toasterMsg = "Aim for GREEN";
            break;
            case 2:
            toasterMsg = "Aim for YELLOW";
            break;
            case 3:
            toasterMsg = "Aim for BLUE";
            break;
            case 4:
            toasterMsg = "Aim for BLACK";
            break;
            case 5:
            toasterMsg = "Aim for WHITE";
            break;
            case 6:
            toasterMsg = "Aim for RED";
            break;
        }
        CreateScreenToast(toasterMsg,2);
        throwResultArray = new int[noOfDarts];

        for(int i =0; i< noOfDarts; i++)
        {
            throwResultArray[i] = 2;// 0-miss 1-hit 2-ResultUnknown
        }

        isGameScreenInteractable = true;        
    }
	
	// Update is called once per frame
	void Update () {


        if(!isGameScreenInteractable)
        {
            return;
        }


        if(noOfDartsThrown >= noOfDarts)
        {
            return;
        }

        if(isDartThrown)
            return;


//..............codes for testing puurpose
            if(Input.GetButtonDown("Fire1"))
            {
                startPos = Input.mousePosition;
                currentPoint = Input.mousePosition;
                holdingCounter = 0;
                createDart();
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                endPos = Input.mousePosition;
                throwDart();
            }
            else{
                if(Input.mousePosition.x == currentPoint.x && Input.mousePosition.y==currentPoint.y)//stationary
                {
                    holdingCounter++;
                    if(holdingCounter >= 5)
                    {
                        
                    }
                    startPos = Input.mousePosition;
                        swipeStartTime = Time.time;
                }
                else{
                    Camera camera = GetComponent<Camera>();
                    Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,camera.nearClipPlane + 5.5f));
                    currentDart.transform.position = new Vector3(p.x,p.y,p.z);
                    holdingCounter = 0;
                    currentPoint = Input.mousePosition;
                }

            }

            return;

//..............codes for testing puurpose ends here


            // if (Input.touchCount > 0) 
            // {
            //     Touch touch = Input.GetTouch(0);
            //     switch (touch.phase)
            //     {
            //     //When a touch has first been detected, change the message and record the starting position
            //         case TouchPhase.Began:
            //             // Record initial touch position.
            //             startPos = touch.position;
            //             createDart();
            //             break;

            //         case TouchPhase.Ended:
            //             // Report that the touch has ended when it ends
            //             endPos = touch.position;
            //             throwDart();
            //             break;

            //         //Determine if the touch is a moving touch
            //         case TouchPhase.Moved:
            //             // Determine direction by comparing the current touch position with the initial one
            //             Camera camera = GetComponent<Camera>();
            //             Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y,camera.nearClipPlane + 5.5f));
            //             currentDart.transform.position = new Vector3(p.x,p.y,p.z);
            //             break;


            //         case TouchPhase.Stationary:
            //             // Report that the touch has ended when it ends
            //             startPos = touch.position;
            //             swipeStartTime = Time.time;
            //             break;

            //         default :
            //             // Report that the touch has ended when it ends
            //             startPos = touch.position;
            //             swipeStartTime = Time.time;
            //             break;
            //     }

                
            // }
             
	}

	private void createDart()
    {
        swipeStartTime = Time.time;

        Camera camera = GetComponent<Camera>();
		Vector3 p = camera.ScreenToWorldPoint(new Vector3(startPos.x, startPos.y,camera.nearClipPlane + 5.5f ));
        currentDart = (GameObject)Instantiate(dart,new Vector3(p.x,p.y,p.z),Quaternion.identity);
        // currentDart = (GameObject)PhotonNetwork.Instantiate("DartPrefab",new Vector3(p.x,p.y,p.z),Quaternion.identity,0);
        currentDart.transform.Rotate(0,270,10);
        currentDart.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        currentDart.SetActive(true);
        currentDart.GetComponent<Rigidbody>().useGravity = false;
    }

	private void throwDart()
    {
        // marking time when you release it
		swipeEndTime = Time.time;

		// calculate swipe time interval 
		swipeDuration = swipeEndTime - swipeStartTime;
        

		// calculating swipe direction in 2D space
		direction = startPos - endPos;

		// add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
		currentDart.GetComponentInChildren<Rigidbody>().isKinematic = false;
        if(direction.x != 0 || direction.y != 0)
        {
            currentDart.GetComponentInChildren<Rigidbody>().AddForce (-direction.x * xyForce,  -direction.y * xyForce, (zForce / swipeDuration)*200);
        }

        currentDart.SetActive(true);    
        currentDart.GetComponent<Rigidbody>().useGravity = true;
        isDartThrown = true;
        noOfDartsThrown++;

        currentDart = null;

        Invoke("ProcessNonHitThrowResult",3);
    }

    public void HitOnDartBoard(GameObject dartCell)
    {
        FindObjectOfType<AudioEffectsController>().Play("HitSound");

        // Debug.Log("Collided with dart Cube");
        Material m_Material = dartCell.GetComponent<Renderer>().material;
        Color cellColor = (Color)(m_Material.color);
        DartGameUtils gameUti = new DartGameUtils();
        if(cellColor.Equals(gameUti.colourMap[currentBetColor]))
        {
            throwResultArray[noOfDartsThrown-1] = 1;
            hudCanvas.GetComponent<CommentaryManager>().ToggleIndicator(noOfDartsThrown,1);
        }
        else{
            throwResultArray[noOfDartsThrown-1] = 0;
            hudCanvas.GetComponent<CommentaryManager>().ToggleIndicator(noOfDartsThrown,2);
        }
        ProcessResultAndProceedToNext();
    }

    private void ProcessNonHitThrowResult()
    {
        if(throwResultArray[noOfDartsThrown-1]==2)
        {
            // throwResultArray[noOfDartsThrown-1] = 2;
            // hudCanvas.GetComponent<CommentaryManager>().ToggleIndicator(noOfDartsThrown,2);
            noOfDartsThrown--;

            //dart didnt hit on the board
            isDartThrown = false;
        }
    }

    private void ProcessResultAndProceedToNext()
    {
        if(noOfDartsThrown >= noOfDarts)
        {
            Vector3 v3 = transform.rotation.eulerAngles;
            Sequence mySequence = DOTween.Sequence();
            mySequence.Append(transform.DOMove(new Vector3(-7.0f,CAMERA_MEAN_POS.y, 10.0f), 1));
            mySequence.Insert(0,transform.DORotate(new Vector3(0, 50, 0), 1));
            mySequence.Append(transform.DORotate(new Vector3(0, 0, 0), 1).SetDelay(1));
            mySequence.Insert(1,transform.DOMove(CAMERA_MEAN_POS, 1).SetDelay(1));
            int score = 0;
            for(int i =0; i<noOfDarts; i++)
            {
                if(throwResultArray[i] == 1)
                {
                    score++;
                }
            }
            if(score ==3)
            {
                CreateScreenToast("EXCELLENT", 4);
            }
            else if(score == 2)
            {
                CreateScreenToast("NICE", 4);
            }
            else{
                CreateScreenToast("TRAIN HARDER!!", 4);
            }
            
            if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.SinglebetMode)
            {
                StartCoroutine(EndGameCall());
            }
            else
            {
                Invoke("RefreshScene", 4);
            }
        }
        isDartThrown = false;
    }

    private void RefreshScene()
    {
        if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
        {
            GameSceneManager.LoadScene("GamePlayScene");
        }
    }

    string GetGameResultArray()
    {
        string gameResultArray = "[{\"id\":\"" + GameManager.currentGameBetId +
        "\",\"t1\":\"" + throwResultArray[0].ToString() +
        "\",\"t2\":\"" + throwResultArray[1].ToString() +
        "\",\"t3\":\"" + throwResultArray[2].ToString() +
        "\"}]";
        return gameResultArray;
    }

    IEnumerator EndGameCall() {

		WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.userInfo.id);
        form.AddField("game_id", GameManager.currentGameId);
    	form.AddField("game_result", GetGameResultArray());

        UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143:8282/public/webresources/app/api/v1/game/exit", form);
        www.SetRequestHeader("Authorization", "Bearer "+GameManager.userToken);
		yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
						StringBuilder sb = new StringBuilder();
            foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
            {
                sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
            }

            // Print Headers
            Debug.Log(sb.ToString());

            // Print Body
            Debug.Log(www.downloadHandler.text);

			ResponseVO info = JsonUtility.FromJson<ResponseVO>(www.downloadHandler.text);
			JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);
			info.data = jsonNode["data"].ToString();
			if(info.status == "true")
			{
                long prevBalance = GameManager.userInfo.wallet_balance;
                GameManager.userInfo = JsonUtility.FromJson<UserInfo>(info.data);
                long  gain = GameManager.userInfo.wallet_balance - prevBalance;
                FindObjectOfType<GenericPopup>().ShowPopup();
                if(gain > 0)
                {
                    FindObjectOfType<GenericPopup>().SetTextTo("hurreeyyy.....You Won " + gain.ToString() + " Montero");
                }
                else{
                    FindObjectOfType<GenericPopup>().SetTextTo("Better Luck Next time");
                }
			}
        }
    }

    public void ExitToMenu()
    {
        GameSceneManager.LoadScene("BetSelector");
    }

    private void CreateScreenToast(string toastMsg, float duration)
    {
        hudCanvas.GetComponent<CommentaryManager>().SetText(toastMsg);
        toastBg.SetActive(true);
        Invoke("RemoveToast", duration);
    }
    private void RemoveToast()
    {
        hudCanvas.GetComponent<CommentaryManager>().SetText("");
        toastBg.SetActive(false);
    }

    public void OnBackButtonClick()
	{
        if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
        {
            GameSceneManager.LoadScene("MenuScreen");
        }
	}
}
