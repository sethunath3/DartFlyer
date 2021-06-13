using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections;
using UnityEngine.Networking;
using System.Text;

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
			StartCoroutine(DoSignUp(userName.text, email.text, password.text));
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


			
        }
	}

	IEnumerator DoSignUp(string userName,string email, string password)
	{
		WWWForm form = new WWWForm();
        form.AddField("name", userName);
        form.AddField("username", userName);
    	form.AddField("email", email);
		form.AddField("password", password);

        //UnityWebRequest www = UnityWebRequest.Post("http://182.18.139.143/WITSCLOUD/DEVELOPMENT/dartweb/index.php/api/register", form);
        UnityWebRequest www = UnityWebRequest.Post("https://dartplay.ml/index.php/Api/register", form);
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

				//ResponseVO info = JsonUtility.FromJson<ResponseVO>(www.downloadHandler.text);
			JSONNode jsonNode = SimpleJSON.JSON.Parse(www.downloadHandler.text);

			if(jsonNode["status"] == true)
			{
				errorMsg.text = jsonNode["message"].Value;
				FindObjectOfType<GenericPopup>().ShowPopup();
                FindObjectOfType<GenericPopup>().SetTextTo("Your Account has been created... Please login");
			}
			else{
				errorMsg.text = jsonNode["message"].Value;
			}
		}
	}

	public void SignUpSuccessfull()
	{
		GameSceneManager.LoadScene("SignInScene");
	}

	public void AlreadyHaveAnAccountClicked()
	{
		GameSceneManager.LoadScene("SignInScene");
	}

	public void OnBackButtonClicked()
	{
		GameSceneManager.LoadScene("LoginSignup");
	}
}
