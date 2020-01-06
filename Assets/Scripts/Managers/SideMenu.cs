using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideMenu : MonoBehaviour {
	public GameObject sideMenu;
	public Text userName;
	// Use this for initialization
	void Start () {
		userName.text = GameManager.userInfo.name;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OpenPanel()
	{
		if(sideMenu != null)
		{
			Animator sideMenuAnim = sideMenu.GetComponent<Animator>();
			if(sideMenuAnim != null)
			{
				sideMenuAnim.SetBool("Open",true);
			}
		}
	}
	public void ClosePanel()
	{
		if(sideMenu != null)
		{
			Animator sideMenuAnim = sideMenu.GetComponent<Animator>();
			Debug.Log("Hai ");
			if(sideMenuAnim != null)
			{
				sideMenuAnim.SetBool("Open",false);
			}
		}
	}

	public void OnLogoutClick()
	{
		GameSceneManager.LoadScene("LoginSignup");
	}

	public void OnContactUsClick()
	{
		GameSceneManager.LoadScene("ContactUs");
	}

	public void OnFeedBackClick()
	{
		GameSceneManager.LoadScene("FeedbackScene");
	}

	public void OnGameGuideClick()
	{
		GameSceneManager.LoadScene("GameGuide");
	}

	public void OnPlayDartClick()
	{
		GameSceneManager.LoadScene("MenuScreen");
	}

	public void OnWalletClick()
	{
		GameSceneManager.LoadScene("WalletManagerScene");
	}

}
