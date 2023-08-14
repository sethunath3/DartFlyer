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
	private bool saveUser;
	void Start () {
		saveUser = false;
		errorMsg.text = "";
		loader =  null;
		TouchScreenKeyboard.hideInput = true;

		if(PlayerPrefs.GetString("SAVED_EMAIL", "") != "")
		{
			email.text = PlayerPrefs.GetString("SAVED_EMAIL");
			password.text = PlayerPrefs.GetString("SAVED_PASSWORD");
			StartCoroutine(DoLogin());
		}
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
		StartCoroutine(DoLogin());
	}

	public class CertificateWhore : CertificateHandler
	{
		protected override bool ValidateCertificate(byte[] certificateData)
		{
			return true;
		}
	}

	IEnumerator DoLogin()
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
			
			WWWForm form = new WWWForm();
        	form.AddField("email", email.text);
        	form.AddField("password", password.text);

            UnityWebRequest www = UnityWebRequest.Post("https://dartbet.io/index.php/Api/login", form);
			www.certificateHandler = new CertificateWhore();
			yield return www.SendWebRequest();

        	if(www.isNetworkError || www.isHttpError) {
            	Debug.Log(www.error);
				Destroy(loader);
        	}
        	else {
            	Debug.Log("Form upload complete!");
				StringBuilder sb = new StringBuilder();
            	foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www.GetResponseHeaders())
            	{
                	sb.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
            	}
            	// Print Headers
            	Debug.Log("header" + sb.ToString());

            	// Print Body
            	Debug.Log("Body:" + www.downloadHandler.text);
			
				JSONNode loginResponse = SimpleJSON.JSON.Parse(www.downloadHandler.text);


                if (loginResponse["status"].ToString() == "true")
				{
					GameManager.userInfo = JsonUtility.FromJson<UserInfo>(loginResponse["userData"].ToString());
					GameManager.userToken  = GameManager.userInfo.token;

                    Debug.Log("user token: " + GameManager.userToken);
					JSONNode  userDataResponse = ServerCalls.GetUserInfo();
                    Debug.Log("userDataResponse:: " + userDataResponse.ToString());
					string response = userDataResponse["status"].Value;
					if(response == "Success")
					{
						if(saveUser)
						{
							PlayerPrefs.SetString("SAVED_EMAIL", email.text);
							PlayerPrefs.SetString("SAVED_PASSWORD", password.text);
							PlayerPrefs.Save();
						}

						UnityWebRequest www2 = UnityWebRequest.Get("https://dartbet.io/countadmin/myapi.php");
						//www.SetRequestHeader("Token", GameManager.userToken);
						yield return www2.SendWebRequest();

						if (www2.isNetworkError || www2.isHttpError)
						{
							Debug.Log(www2.error);
						}
						else
						{
							Debug.Log("Form upload complete!");
							StringBuilder sb1 = new StringBuilder();
							foreach (System.Collections.Generic.KeyValuePair<string, string> dict in www2.GetResponseHeaders())
							{
								sb1.Append(dict.Key).Append(": \t[").Append(dict.Value).Append("]\n");
							}

							// Print Headers
							Debug.Log(sb1.ToString());

							// Print Body
							Debug.Log(www2.downloadHandler.text);

							//ResponseVO info = JsonUtility.FromJson<ResponseVO>(www.downloadHandler.text);
							JSONNode jsonNode = SimpleJSON.JSON.Parse(www2.downloadHandler.text);
							GameManager.maxBet = jsonNode["count"];
							GameManager.betamountList = new int[GameManager.maxBet];
							for(int i = 1; i <= GameManager.maxBet; i++)
                            {
								GameManager.betamountList[i-1] = i;
                            }
							/*if (jsonNode["status"].Value == "Success")
							{
								Destroy(FindObjectOfType<AudioManager>());
								GameSceneManager.LoadScene("GamePlayScene");
							}*/
						}
						GameSceneManager.LoadScene("HomeScreen");
					}
					else{
						Destroy(loader);
						errorMsg.text = userDataResponse["message"].ToString();
					}
				}
				else
				{
					Destroy(loader);
					errorMsg.text = loginResponse["message"].ToString();
				}
			}
		}
	}

	public void ToggleSaveUser()
	{
		saveUser = !saveUser;
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

	public void NewPlayerClicked()
    {
        Application.OpenURL("https://dartbet.io/index.php/home/joinus");
        //GameSceneManager.LoadScene("SignUpScene");
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
