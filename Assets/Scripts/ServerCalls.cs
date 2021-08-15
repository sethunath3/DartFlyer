using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Net;
using System;
using SimpleJSON;
using UnityEngine.Networking;
using System.Text;

public class ServerCalls : MonoBehaviour {

	[System.Serializable]
					public class ResponseVO
					{
						public string status;
						public string token;
						public string data;
						public string message;
						public string auth_header_format;
					};


	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public ServerCalls()
	{

	}


public static JSONNode ValidateUserWithEmail(string email, string password)
{
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/login?email={0}&password={1}", email, password));
    //request.Timeout = 9000;
	request.Method = "POST";
    //request.ReadWriteTimeout = 9000;
	HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
					ResponseVO info = JsonUtility.FromJson<ResponseVO>(jsonResponse);
					JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);
					//info.data = jsonNode["data"].ToString();
					//Debug.Log ("Department 0 "+ jsonNode[0].ToString());
					//UserInfo userDetails = JsonUtility.FromJson<UserInfo>(jsonNode["data"].ToString());
          
          return jsonNode;
  }

	public static JSONNode SignUpUser(string userName,string email, string password)
	{
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/register?name={0}&username={1}&email={2}&password={3}", userName, email, email, password));
          //request.Timeout = 5000;
					request.Method = "POST";
    			//request.ReadWriteTimeout = 5000;
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
					ResponseVO info = JsonUtility.FromJson<ResponseVO>(jsonResponse);
					JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);
					//info.data = jsonNode["data"].ToString();
					//Debug.Log ("Department 0 "+ jsonNode[0].ToString());
					//UserInfo userDetails = JsonUtility.FromJson<UserInfo>(jsonNode["data"].ToString());
          
          return jsonNode;
	}

public static JSONNode GetUserInfo()
{
        //string requestString = String.Format("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/getUserInfo");
        string requestString = String.Format("https://dartbet.io/index.php/api/getUserInfo");
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
	//request.Headers.Add("Authorization", "Bearer "+GameManager.userToken);
	request.Headers.Add("token", GameManager.userToken);
    request.Timeout = 9000;
	request.Method = "POST";

	HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    StreamReader reader = new StreamReader(response.GetResponseStream());
    string jsonResponse = reader.ReadToEnd();
	JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);    

	string status = jsonNode["status"].Value;
	if(status == "Success")
	{
		GameManager.userInfo = JsonUtility.FromJson<UserInfo>(jsonNode["userData"].ToString());
		List<GameHistory> tempGameHistory = new List<GameHistory>();
		for(int i =0; i< 3; i++)
		{
			GameHistory gh = JsonUtility.FromJson<GameHistory>(jsonNode["gameData"][i].ToString());
			tempGameHistory.Add(gh);
		}
		GameManager.gameHistory = tempGameHistory;
	}

    return jsonNode;
}

	public static ResponseVO EnterGameRoom(DartGameUtils.GameMode gameType, string betData)
	{
		string requestString = String.Format("http://182.18.139.143:8282/public/webresources/app/api/v1/game/start?user_id={0}&game_type={1}&game_data={2}", GameManager.userInfo.id, ((int)gameType).ToString(),betData);
		// requestString = "http://182.18.139.143:8282/public/webresources/app/api/v1/game/start?user_id=102452&game_type=1&game_data=[]";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
		request.Headers.Add("Authorization", "Bearer "+GameManager.userToken);
    // request.Timeout = 9000;
		request.Method = "POST";
    // request.ReadWriteTimeout = 9000;
		
		
		
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    StreamReader reader = new StreamReader(response.GetResponseStream());
    string jsonResponse = reader.ReadToEnd();
		ResponseVO info = JsonUtility.FromJson<ResponseVO>(jsonResponse);
		JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);
		info.data = jsonNode["game"].ToString();
          
    return info;


	}

	

	public static ResponseVO ExitGameRoom(string game_id, string gameResult)
	{
		string requestString = String.Format("http://182.18.139.143:8282/public/webresources/app/api/v1/game/exit?user_id={0}&game_id={1}&game_data={2}", GameManager.userInfo.id, game_id, gameResult);
		// requestString = "http://182.18.139.143:8282/public/webresources/app/api/v1/game/start?user_id=102452&game_type=1&game_data=[]";
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestString);
		request.Headers.Add("Authorization", "Bearer "+GameManager.userToken);
    request.Timeout = 9000;
		request.Method = "POST";
    request.ReadWriteTimeout = 9000;
		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
    StreamReader reader = new StreamReader(response.GetResponseStream());
    string jsonResponse = reader.ReadToEnd();
		ResponseVO info = JsonUtility.FromJson<ResponseVO>(jsonResponse);
		JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);
		info.data = jsonNode["game"].ToString();
          
    return info;
	}

	
}
