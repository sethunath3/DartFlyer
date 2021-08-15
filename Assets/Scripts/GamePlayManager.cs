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
    [SerializeField] GameObject pauseUi;
    private int[] throwResultArray;

    private int exitScreen = 1;//BidSelectScreen

    private bool isGameScreenInteractable;

    private Vector3 CAMERA_MEAN_POS = new Vector3(2.55f,18.5f,0.0f);
    private Vector3 CAMERA_INIT_POS = new Vector3(-5.0f,18.5f, -10.0f);

    


    

    int noOfDarts = 0;
    int noOfDartsThrown = 0;

    float xForce = 7.0f;
    float yForce = 8.0f;
    float zForce = 7.0f;

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
        pauseUi.SetActive(false);

        if (GameManager.GetInstance().lobbyEntryTime)
        {
            //CreateScreenToast("",1);
            FindObjectOfType<GenericPopup>().HidePopup();
            toastBg.SetActive(false);
            GetComponent<Camera>().transform.position = CAMERA_INIT_POS;
            transform.DOMove(CAMERA_MEAN_POS, 4);
            GameManager.GetInstance().lobbyEntryTime = false;
            Invoke("InitGame", 4);
        }
        else{
            FindObjectOfType<GenericPopup>().HidePopup();
            GetComponent<Camera>().transform.position = CAMERA_MEAN_POS;
            InitGame();
        }
	}

    private void InitGame()
    {
        isDartThrown =false;
        isHold = false;
        hudCanvas.GetComponent<CommentaryManager>().ResetIndicatorPanel();

        string toasterMsg = "";
        noOfDarts = 3;
        noOfDartsThrown = 0;

        if (GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
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
        {
            return;
        }
            


// ..............codes for testing puurpose
            if(Input.GetButtonDown("Fire1"))
            {
                StartSwipe();
                createDart();
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                EndSwipe();
                throwDart();
            }
            else if(isHold)
            {
                if(Input.mousePosition.x == currentPoint.x && Input.mousePosition.y==currentPoint.y)//stationary
                {
                    holdingCounter++;
                    if(holdingCounter >= 5)
                    {
                        StartSwipe();
                    }
                        
                }
                else{
                    Camera camera = GetComponent<Camera>();
                    Vector3 p = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,camera.nearClipPlane + 5.5f));
                    currentDart.transform.position = new Vector3(p.x,p.y,p.z);
                    if (Input.mousePosition.y <= currentPoint.y)
                    {
                        StartSwipe();
                    }
                    else
                    {
                        holdingCounter = 0;
                        currentPoint = Input.mousePosition;
                    }
                }

            }

            return;
             
	}

    private void StartSwipe()
    {
        startPos = Input.mousePosition;
        currentPoint = Input.mousePosition;
        swipeStartTime = Time.time;
        holdingCounter = 0;
        isHold = true;
    }

    private void EndSwipe()
    {
        endPos = Input.mousePosition;
        swipeEndTime = Time.time;
        isHold = false;
    }

	private void createDart()
    {
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

		// calculate swipe time interval 
		swipeDuration = swipeEndTime - swipeStartTime;
        

		// calculating swipe direction in 2D space
		direction = startPos - endPos;

		// add force to balls rigidbody in 3D space depending on swipe time, direction and throw forces
		currentDart.GetComponentInChildren<Rigidbody>().isKinematic = false;
        if(direction.x != 0 || direction.y != 0)
        {
            currentDart.GetComponentInChildren<Rigidbody>().AddForce (-direction.x * xForce,  -direction.y * yForce, (zForce / swipeDuration)*200);
        }

        currentDart.SetActive(true);    
        currentDart.GetComponent<Rigidbody>().useGravity = true;
        isDartThrown = true;
        noOfDartsThrown++;

        currentDart = null;

        // Invoke("ProcessNonHitThrowResult",3);
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

    public void HitOnBoard()
    {
        if(throwResultArray[noOfDartsThrown-1]==2)
        {
            // throwResultArray[noOfDartsThrown-1] = 2;
            // hudCanvas.GetComponent<CommentaryManager>().ToggleIndicator(noOfDartsThrown,2);
            // noOfDartsThrown--;
            throwResultArray[noOfDartsThrown-1] = 0;
            hudCanvas.GetComponent<CommentaryManager>().ToggleIndicator(noOfDartsThrown,2);

            //dart didnt hit on the board
            // isDartThrown = false;
        }
        ProcessResultAndProceedToNext();
    }

    public void ProcessNonHitThrowResult()
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
                if (throwResultArray[i] == 2)
                {
                    throwResultArray[i] = 0;
                }
            }
            
            if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.SinglebetMode)
            {
                Debug.Log("This is happening 2");
                StartCoroutine(EndGameCall());
            }
            else
            {
                Debug.Log("Working fine");
                if (score == 3)
                {
                    CreateScreenToast("EXCELLENT", 4);
                }
                else if (score == 2)
                {
                    CreateScreenToast("NICE", 4);
                }
                else
                {
                    CreateScreenToast("TRAIN HARDER!!", 4);
                }
                Invoke("RefreshScene", 4);
            }
        }
        isDartThrown = false;
    }

    private void RefreshScene()
    {
        Debug.Log("Yes it is practice");
        if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
        {
            GameSceneManager.LoadScene("GamePlayScene");
        }
    }

    string GetGameResultArray()
    {
        /*string gameResultArray = "[{\"id\":\"" + GameManager.currentGameBetId +
        "\",\"t1\":\"" + throwResultArray[0].ToString() +
        "\",\"t2\":\"" + throwResultArray[1].ToString() +
        "\",\"t3\":\"" + throwResultArray[2].ToString() +
        "\"}]";*/

        for(int i =0; i<noOfDarts; i++)
        {
            if(throwResultArray[i] == 2)
            {
                throwResultArray[i] = 0;
            }
        }

        string gameResultArray = throwResultArray[0].ToString() +
        "-" + throwResultArray[1].ToString() +
        "-" + throwResultArray[2].ToString();
        return gameResultArray;
    }

    IEnumerator EndGameCall() {
        Debug.Log("This is happening 1");
        WWWForm form = new WWWForm();
        //form.AddField("user_id", GameManager.userInfo.id);
        form.AddField("gameId", GameManager.currentGameId);
    	form.AddField("result", GetGameResultArray());

        //UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/gameComplete", form);
        UnityWebRequest www = UnityWebRequest.Post("https://dartbet.io/index.php/api/gameComplete", form);
        www.SetRequestHeader("token", GameManager.userToken);
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

			JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);
			if(jsonNode["status"].Value == "Success")
			{

                JSONNode  userDataResponse = ServerCalls.GetUserInfo();
				if(userDataResponse["status"].Value == "Success")
				{
					long  gain = int.Parse(jsonNode["winningAmount"].ToString());
                    FindObjectOfType<GenericPopup>().ShowPopup();
                    if(gain > 0)
                    {
                        FindObjectOfType<GenericPopup>().SetTextTo("hurreeyyy.....You Won " + gain.ToString() + " Montero");
                    }
                    else{
                        FindObjectOfType<GenericPopup>().SetTextTo("Better Luck Next time");
                    }

                    Debug.Log("This is happening");
				}
				//else{
					//Destroy(loader);
					//errorMsg.text = userDataResponse["message"].ToString();
				//}
			}
        }
    }

    public void ExitToMenu()
    {
        if(exitScreen == 1)
        {
            GameSceneManager.LoadScene("BetSelector");
        }
        else
        {
            GameSceneManager.LoadScene("MenuScreen");
        }
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

    public void OnExitGameClick(int _exitScreen)
	{
        exitScreen = _exitScreen;
        noOfDartsThrown = noOfDarts;
        ProcessResultAndProceedToNext();
        pauseUi.SetActive(false);

    }

    public void OnBackClick()
    {
        if (GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.PracticeMode)
        {
            GameSceneManager.LoadScene("MenuScreen");
        }
        else
        {
            pauseUi.SetActive(true);
        }
    }
}
