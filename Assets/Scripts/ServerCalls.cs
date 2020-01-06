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


	public static ResponseVO ValidateUserWithEmail(string email, string password)
  {
    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://182.18.139.143:8282/public/webresources/app/api/v1/account/login?username={0}&password={1}", email, password));
    request.Timeout = 9000;
		request.Method = "POST";
    request.ReadWriteTimeout = 9000;
					HttpWebResponse response = (HttpWebResponse)request.GetResponse();
          StreamReader reader = new StreamReader(response.GetResponseStream());
          string jsonResponse = reader.ReadToEnd();
					ResponseVO info = JsonUtility.FromJson<ResponseVO>(jsonResponse);
					JSONNode jsonNode = SimpleJSON.JSON.Parse(jsonResponse);
					info.data = jsonNode["data"].ToString();
					//Debug.Log ("Department 0 "+ jsonNode[0].ToString());
					//UserInfo userDetails = JsonUtility.FromJson<UserInfo>(jsonNode["data"].ToString());
          
          return info;
  }

	public static ResponseVO SignUpUser(string userName,string email, string password)
	{
					HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("http://182.18.139.143:8282/public/webresources/app/api/v1/account/create?name={0}&userid={1}&email={2}&password={3}", userName, email, email, password));
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
          
          return info;
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
