using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignUpHandler : MonoBehaviour {

	public InputField userName;
	public InputField email;
    public InputField password;
	public InputField confirmationPassword;

	public Text errorMsg;

	private bool localValidated;

	void Start () {
		errorMsg.text = "";
		localValidated = false;
		TouchScreenKeyboard.hideInput = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SignUpUser()
	{
		if(password.text != confirmationPassword.text)
		{
			password.text = "";
			confirmationPassword.text = "";
			errorMsg.text = "Passwords doesn't match";
			return;
		}
		else if(password.text.Length<8)
		{
			password.text = "";
			confirmationPassword.text = "";
			errorMsg.text = "Password must have minimum 8 Characters";
			return;
		}
		localValidated = true;
		if(localValidated)
		{
			// auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWith(task => {
  			// 	if (task.IsCanceled) {
    		// 		Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
    		// 		return;
  			// 	}
  			// 	if (task.IsFaulted) {
    		// 		Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    		// 		return;
  			// 	}

  			// 	// Firebase user has been created.
  			// 	Firebase.Auth.FirebaseUser newUser = task.Result;
  			// 	Debug.LogFormat("Firebase user created successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
			// });
			// GameSceneManager.LoadScene("SignInScene");


			ServerCalls.ResponseVO loginResponse = ServerCalls.SignUpUser(userName.text, email.text, password.text);
			if(loginResponse.status == "true")
			{
				// UserInfo userDetails = JsonUtility.FromJson<UserInfo>(loginResponse.data);
				// GameManager.userInfo = JsonUtility.FromJson<UserInfo>(loginResponse.data);
				errorMsg.text = loginResponse.message;
				FindObjectOfType<GenericPopup>().ShowPopup();
                FindObjectOfType<GenericPopup>().SetTextTo("Your Account has been created... Please login");
			
			}
			else{
				errorMsg.text = loginResponse.message;
			}
		}
	}

	public void SignUpSuccessfull()
	{
		GameSceneManager.LoadScene("SignInScene");
	}

	public void OnBackButtonClicked()
	{
		GameSceneManager.LoadScene("LoginSignup");
	}
}
