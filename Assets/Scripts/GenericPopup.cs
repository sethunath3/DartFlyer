using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericPopup : MonoBehaviour {

	public Text textField;
	public GameObject thisPopup;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTextTo(string message)
	{
		textField.text = message;
	}

	public void ShowPopup()
	{
		thisPopup.SetActive(true);
	}

	public void HidePopup()
	{
		thisPopup.SetActive(false);
	}
}
