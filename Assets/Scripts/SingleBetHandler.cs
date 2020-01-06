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
	private DartGameUtils gameUtil;
	
	void Start () {
		userName.text = GameManager.userInfo.name;
		gameUtil = new DartGameUtils();
		betAmount = 10;
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
	}
	
	// Update is called once per frame
	void Update () {
		betcolorIndicator.GetComponent<Image>().color = gameUtil.colourMap[GameManager.playerBets[0].BetColour];
		betAmountDisplay.text = GameManager.playerBets[0].BetAmount.ToString();
	}

	public void OnPlaceBetBtnClick()
	{ 
		GameObject bettingPopup = Instantiate (popupPrefab, popupPrefab.transform.position, popupPrefab.transform.rotation) as GameObject;
 
	}

	public void OnGameenterButtonClick()
	{
		if(GameManager.userInfo.wallet_balance >= betAmount)
		{
			GameManager.GetInstance().betAmount = betAmount;
			GameManager.GetInstance().betColour = betColour;
			GameManager.GetInstance().SetCurrentGameMode(DartGameUtils.GameMode.SinglebetMode);
			GameManager.GetInstance().lobbyEntryTime = true;

			
			StartCoroutine(StartGameCall(DartGameUtils.GameMode.SinglebetMode,gameUtil.GetBetStructureJSON()));
			GameObject loader = Instantiate (loaderPrefab, loaderPrefab.transform.position, loaderPrefab.transform.rotation) as GameObject;
		}
	}

	IEnumerator StartGameCall(DartGameUtils.GameMode gameType, string betData) {

		WWWForm form = new WWWForm();
        form.AddField("user_id", GameManager.userInfo.id);
        form.AddField("game_type", ((int)gameType).ToString());
    	form.AddField("game_data", betData);

        UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143:8282/public/webresources/app/api/v1/game/start", form);
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
			info.data = jsonNode["game"].ToString();
			GameManager.currentGameId = jsonNode["game"]["id"];
			GameManager.currentGameBetId = jsonNode["game"]["bets"][0];
			if(info.status == "true")
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
