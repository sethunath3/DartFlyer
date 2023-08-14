using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.Networking;
using System;
using SimpleJSON;
public class SingleBetHandler : MonoBehaviour {

	private int betAmount;
	private int betColour;
	public Text userName;
	public Image betcolorIndicator;
	public Text betAmountDisplay;

	public GameObject popupPrefab;
	public GameObject loaderPrefab;
    [SerializeField] GameObject genericInfoPopup;
	private DartGameUtils gameUtil;
    bool isInitialized = false;
	
	void Start () {
        genericInfoPopup.gameObject.SetActive(false);
        userName.text = GameManager.userInfo.name;
		gameUtil = new DartGameUtils();
		betAmount = GameManager.betamountList[0];
		betColour = UnityEngine.Random.Range(1,7);
		GameManager.playerBets.Clear();
		DartGameUtils.BetStructure betStruct = new DartGameUtils.BetStructure
		{
			BetAmount = betAmount,
			BetColour = betColour
		};
		GameManager.playerBets.Add(betStruct);
		GameManager.GetInstance().SetCurrentGameMode(DartGameUtils.GameMode.SinglebetMode);

        betcolorIndicator.GetComponent<Image>().color = gameUtil.colourMap[betColour];
		betAmountDisplay.text = betAmount.ToString();
        isInitialized = true;

    }
	
	// Update is called once per frame
	void Update () {
        if(isInitialized)
        {
            betcolorIndicator.GetComponent<Image>().color = gameUtil.colourMap[GameManager.playerBets[0].BetColour];
            betAmountDisplay.text = GameManager.playerBets[0].BetAmount.ToString();
        }
	}

	public void OnPlaceBetBtnClick()
	{ 
		GameObject bettingPopup = Instantiate (popupPrefab, popupPrefab.transform.position, popupPrefab.transform.rotation) as GameObject;
 
	}

	public void OnGameenterButtonClick()
	{
		if(GameManager.userInfo.walletAmount >= GameManager.playerBets[0].BetAmount)
		{
			GameManager.GetInstance().betAmount = (int)GameManager.playerBets[0].BetAmount;
			GameManager.GetInstance().betColour = GameManager.playerBets[0].BetColour;
			GameManager.GetInstance().SetCurrentGameMode(DartGameUtils.GameMode.SinglebetMode);
			GameManager.GetInstance().lobbyEntryTime = true;

			
			//StartCoroutine(StartGameCall(DartGameUtils.GameMode.SinglebetMode,gameUtil.GetBetStructureJSON()));
			StartCoroutine(StartGameCall(DartGameUtils.GameMode.SinglebetMode, GameManager.GetInstance().betAmount, GameManager.GetInstance().betColour));
			GameObject loader = Instantiate (loaderPrefab, loaderPrefab.transform.position, loaderPrefab.transform.rotation) as GameObject;
		}
        else
        {
            genericInfoPopup.gameObject.SetActive(true);
        }
	}

	//IEnumerator StartGameCall(DartGameUtils.GameMode gameType, string betData) {
	IEnumerator StartGameCall(DartGameUtils.GameMode gameType, int _betAmount, int _betColour) {

		WWWForm form = new WWWForm();
        form.AddField("betAmount", _betAmount);
        form.AddField("gameMode", ((int)gameType));
    	form.AddField("betColor", _betColour);

        Debug.Log("betAmount:" + _betAmount);

        //UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/createGameRoom", form);
        UnityWebRequest www = UnityWebRequest.Post("https://dartbet.io/index.php/api/createGameRoom", form);
        www.SetRequestHeader("Token", GameManager.userToken);
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

			//ResponseVO info = JsonUtility.FromJson<ResponseVO>(www.downloadHandler.text);
			JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);
			//info.data = jsonNode["game"].ToString();
			GameManager.currentGameId = jsonNode["gameId"];
			//GameManager.currentGameBetId = jsonNode["game"]["bets"][0];
			if(jsonNode["status"].Value == "Success")
			{
				Destroy(FindObjectOfType<AudioManager>());
				GameSceneManager.LoadScene("GamePlayScene");
			}
        }
    }

	public void OnBackButtonClick()
	{
		GameSceneManager.LoadScene("MenuScreen");
	}
}
