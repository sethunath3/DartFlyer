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
		walletBalance.text = "Amount in your wallet: "+ GameManager.userInfo.walletAmount;
		earningsValue.text = ": $"+(GameManager.gameHistory[0].totalEarning + GameManager.gameHistory[1].totalEarning + GameManager.gameHistory[2].totalEarning).ToString();
		winningStrikes.text = ": "+(GameManager.gameHistory[0].winningStrike + GameManager.gameHistory[1].winningStrike + GameManager.gameHistory[2].winningStrike).ToString();
		totalGames.text = ": "+(GameManager.gameHistory[0].totalMatches + GameManager.gameHistory[1].totalMatches + GameManager.gameHistory[2].totalMatches).ToString();
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
