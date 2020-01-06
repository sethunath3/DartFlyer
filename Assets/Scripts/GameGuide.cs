using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnSkipClick()
	{
		GameSceneManager.LoadScene("HomeScreen");
	}
}
