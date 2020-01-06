using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSelectionController : MonoBehaviour {

	public void PlayPracticeMode()
	{
		GameManager.GetInstance().SetCurrentGameMode(DartGameUtils.GameMode.PracticeMode);
		GameManager.GetInstance().lobbyEntryTime = true;
		GameSceneManager.LoadScene("GamePlayScene");
	}

	public void PlaySingleBetMode()
	{
		GameSceneManager.LoadScene("BetSelector");
	}

	public void PlaySideBetMode()
	{
		//GameSceneManager.LoadScene("SideBetScene");
	}

	public void OnBackBtnClicked()
	{
		GameSceneManager.LoadScene("HomeScreen");
	}

	public void OnNotificationBtnClicked()
	{

	}
}
