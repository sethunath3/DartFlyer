using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ContactUs : MonoBehaviour {

	// Use this for initialization

	public InputField contactNumber;
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
		if(contactNumber.text != "")
		{
			//Instantiate (loaderPrefab, loaderPrefab.transform.position, loaderPrefab.transform.rotation) as GameObject;
			WWWForm form = new WWWForm();
        	form.AddField("user_id", GameManager.userInfo.id);
        	form.AddField("mobile", contactNumber.text);

        	UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143:8282/public/webresources/app/api/v1/account/contact", form);
        	www.SetRequestHeader("Authorization", "Bearer "+GameManager.userToken);
			www.SendWebRequest();
			GameSceneManager.LoadScene("HomeScreen");
		}
	}
}
