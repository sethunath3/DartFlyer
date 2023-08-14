using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class UserInfo
{
	public int id;
	public string guid;
	public string name;
	public string email;
	public string username;
	public string token;
	public long walletAmount;
	public string last_login_at;
	public string status;
	public string created_at;
	public string updated_at;
	//public string deactivated_at;
	//public string deleted_at;
	//public long total_single;
	//public long total_sidebet;
	//public long total_practice;
	//public long total_earnings;
	//public long winning_strikes;

}

public class GameHistory
{
	public long totalEarning;
	public long winningStrike;
	public long loosingStrike;
	public long totalStrike;
	public long totalBetAmount;
	public long totalMatches;
	public int gameMode;
}


public class DartGameUtils {

	public string baseURL = "http://182.18.139.143:8282/public/webresources/app/api/v1/account/login";

	public enum GameMode { PracticeMode = 1, SinglebetMode=2, SideBetMode = 3};

	public struct BetStructure
	{
		public long BetAmount;
		public int BetColour;
	}

	public Dictionary<int, Color> colourMap = new Dictionary<int, Color>() 
	{
        {1, Color.green},
        {2, Color.yellow},
		{3, Color.blue},
		{4, Color.black},
		{5, Color.white},
        {6, Color.red}
    };

	//public int[] betamountList = new int[] {1,2,3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15};

	 
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string GetBetStructureJSON()
	{
		string jsonString = "[";
		foreach(BetStructure str in GameManager.playerBets)
     	{
			 jsonString = jsonString + "{\"bet_amount\":\"" + str.BetAmount.ToString() + "\",\"bet_color\":\"" + str.BetColour.ToString() + "\"}";
        }
		jsonString = jsonString + "]";
		return jsonString;
	} 
}
