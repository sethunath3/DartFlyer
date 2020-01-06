using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;


[System.Serializable]
	public class ResponseVO
	{
		public string status;
		public string token;
		public string data;
		public string message;
		public string auth_header_format;
	};

public class ServerCallManager : MonoBehaviour {


	// Use this for initialization

	WWWForm form;
	string serverCallURL;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public ResponseVO EnterGameRoom(DartGameUtils.GameMode gameType, string betData)
	{
		form = new WWWForm();
        form.AddField("user_id", GameManager.userInfo.id);
        form.AddField("game_type", ((int)gameType).ToString());
    	form.AddField("game_data", betData);

		serverCallURL = "http://182.18.139.143:8282/public/webresources/app/api/v1/game/start";

		StartCoroutine(StartServerCall());

		Debug.Log("Control reached here");
		return null;
	}


	IEnumerator StartServerCall() 
	{
        UnityWebRequest www = UnityWebRequest.Post(serverCallURL, form);
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
        }
    }
}
