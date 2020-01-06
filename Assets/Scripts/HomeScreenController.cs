using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreenController : MonoBehaviour {

	public Text userName;
	public Text email;
	public Text uName;
	public Text walletBalance;
	public Text earningsValue;
	public Text winningStrikes;
	public Text totalGames;
	void Start () {
		userName.text = GameManager.userInfo.name;
		uName.text = ": "+GameManager.userInfo.name;
		email.text = ": "+GameManager.userInfo.email;
		walletBalance.text = "Amount in your wallet: "+ GameManager.userInfo.wallet_balance;
		earningsValue.text = ": $"+GameManager.userInfo.total_earnings.ToString();
		winningStrikes.text = ": "+GameManager.userInfo.winning_strikes.ToString();
		totalGames.text = ": "+(GameManager.userInfo.total_sidebet + GameManager.userInfo.total_single).ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnNavigateBtnClick()
	{

	}

	public void OnNotificationBtnClick()
	{

	}

	public void OnEnterRoomBtnClick()
	{
		GameSceneManager.LoadScene("MenuScreen");
	}

	public void ONChangePasswordBtnClick()
	{

	}

	public void OnRechargeBtnClick()
	{
		Application.OpenURL("http://182.18.139.143/crypto/load/load.html");
		//GameSceneManager.LoadScene("WebviewScene");
	}
}
