using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitePopupController : MonoBehaviour {

	public InputField userID;
	public GameObject PopupCanvas;
	void Start () {
		PopupCanvas.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnInviteBtnClicked()
	{
		PopupCanvas.SetActive(false);
	}

	public void OnCloseBtnClicked()
	{
		PopupCanvas.SetActive(false);
	}
}
