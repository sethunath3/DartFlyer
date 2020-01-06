using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginScreensHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TouchScreenKeyboard.hideInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//Login or Signup screen buttton handlers
	public void LoginBtnClicked()
	{
		GameSceneManager.LoadScene("SignInScene");
	}
	public void SignUpBtnClicked()
	{
		GameSceneManager.LoadScene("SignUpScene");
	}
}
