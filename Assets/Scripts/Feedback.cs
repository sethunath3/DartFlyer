using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Feedback : MonoBehaviour {

	// Use this for initialization

	public InputField feedback;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void  OnCloseBtnClick()
	{
		GameSceneManager.LoadScene("HomeScreen");
	} 

	public void OnSubmitBtnClick()
	{
		if(feedback.text != "")
		{
			//Instantiate (loaderPrefab, loaderPrefab.transform.position, loaderPrefab.transform.rotation) as GameObject;
			WWWForm form = new WWWForm();
        	form.AddField("user_id", GameManager.userInfo.id);
        	form.AddField("message", feedback.text);

        	UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143:8282/public/webresources/app/api/v1/account/feedback", form);
        	www.SetRequestHeader("Authorization", "Bearer "+GameManager.userToken);
			www.SendWebRequest();
			GameSceneManager.LoadScene("HomeScreen");
		}
	}
}
