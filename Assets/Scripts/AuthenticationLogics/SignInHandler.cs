using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using System.Text;

public class SignInHandler : MonoBehaviour {
	public GameObject loaderPrefab;
	public InputField email;
	public Text errorMsg;
    public InputField password;
	private GameObject loader;
	void Start () {
		errorMsg.text = "";
		loader =  null;
		TouchScreenKeyboard.hideInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private bool ValidateUserWithServer()
	{
		return true;
	}

	public void OnSigninBtnClick()
	{	
		//cheat
		if(email.text == "")
		{
			errorMsg.text = "Email Field Cannot be Empty";
		}
		else if(password.text == "")
		{
			errorMsg.text = "Password Field Cannot be Empty";
		}
		else                                                                                                      
		{
			loader = Instantiate (loaderPrefab, loaderPrefab.transform.position, loaderPrefab.transform.rotation) as GameObject;
			ServerCalls.ResponseVO loginResponse = ServerCalls.ValidateUserWithEmail(email.text, password.text);
			if(loginResponse.status == "true")
			{
				UserInfo userDetails = new UserInfo();
				GameManager.userInfo = JsonUtility.FromJson<UserInfo>(loginResponse.data);
				GameManager.userToken  = loginResponse.token;
				// StartCoroutine(UpdateUserLeaderBoard());
				GameSceneManager.LoadScene("HomeScreen");
			}
			else{
				Destroy(loader);
				errorMsg.text = loginResponse.message;
			}
		}
	}

	public void ToggleInputType() 
	{
    if (this.password != null) {
      if (this.password.contentType == InputField.ContentType.Password) 
			{
        this.password.contentType = InputField.ContentType.Standard;
      } else
			{
        this.password.contentType = InputField.ContentType.Password;
      }

    	this.password.ForceLabelUpdate ();
    }
  }

	public void BackButtonPressed()
    {
		GameSceneManager.LoadScene("LoginSignup");
    }

    public void ForgotButtonPressed()
    {
        
    }

  IEnumerator UpdateUserLeaderBoard() 
	{
		WWWForm form = new WWWForm();
    form.AddField("uid", GameManager.userInfo.id);

    UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143:8282/public/webresources/app/api/v1/account/synopsis", form);
    www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
		www.chunkedTransfer = false;
		yield return www.SendWebRequest();
 
    if(www.isNetworkError || www.isHttpError) 
		{
      Debug.Log(www.error);
    }
    else 
		{
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

			// ResponseVO info = JsonUtility.FromJson<ResponseVO>(www.downloadHandler.text);
			// JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);
			// info.data = jsonNode["game"].ToString();
			// GameManager.currentGameId = jsonNode["game"]["id"];
			// GameManager.currentGameBetId = jsonNode["game"]["bets"][0];
			// if(info.status == "true")
			// {
			// 	GameSceneManager.LoadScene("GamePlayScene");
			// }
    }
  }
}
