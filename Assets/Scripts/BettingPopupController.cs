using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingPopupController : MonoBehaviour {

	// Use this for initialization
	int colorIndex;
	int betAmountIndex;

	public GameObject _betColorIndicator;
	public Text _betAmountindicator;

	public GameObject PopupCanvas; 

	DartGameUtils gameUtil;

	void Start () {
		gameUtil = new DartGameUtils();
		colorIndex = 1;
		betAmountIndex = 0;

		RefreshComponentsAndData();

		PopupCanvas.SetActive(true);
		if(GameManager.GetInstance().GetCurrentGameMode() == DartGameUtils.GameMode.SinglebetMode)
		{
			GameManager.playerBets.Clear();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnCloseBtnClicked()
	{
		PopupCanvas.SetActive(false);
	}

	public void ONBetUpBtnClicked()
	{
		if(betAmountIndex >= gameUtil.betamountList.Length-1)
		{
			betAmountIndex = 0;
		}
		else
		{
			betAmountIndex++;
		}
		RefreshComponentsAndData();
	}
	public void ONBetDownBtnClicked()
	{
		if(betAmountIndex == 0)
		{
			betAmountIndex = gameUtil.betamountList.Length-1;
		}
		else
		{
			betAmountIndex--;
		}
		RefreshComponentsAndData();
	}
	public void ONColorUpBtnClicked()
	{
		if(colorIndex >= gameUtil.colourMap.Count)
		{
			colorIndex = 1;
		}
		else
		{
			colorIndex++;
		}
		RefreshComponentsAndData();
	}
	public void ONColorDownBtnClicked()
	{
		if(colorIndex == 0)
		{
			colorIndex = gameUtil.colourMap.Count;
		}
		else
		{
			colorIndex--;
		}
		RefreshComponentsAndData();
	}

	private void RefreshComponentsAndData()
	{
		_betAmountindicator.text = gameUtil.betamountList[betAmountIndex].ToString();
		_betColorIndicator.GetComponent<Image>().color = (Color)gameUtil.colourMap[colorIndex];
	}

	public void OnBetBtnClicked()
	{
		DartGameUtils.BetStructure betStruct = new DartGameUtils.BetStructure
		{
			BetAmount = gameUtil.betamountList[betAmountIndex],
			BetColour = colorIndex
		};
		GameManager.playerBets.Clear();
		GameManager.playerBets.Add(betStruct);
		PopupCanvas.SetActive(false);
	}
}
