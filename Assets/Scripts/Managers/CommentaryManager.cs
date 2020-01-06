using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CommentaryManager : MonoBehaviour {

const int DART1 = 1;
const int DART2 = 2;
const int DART3 = 3;
public Text ToastText;

public GameObject Shoot1,Shoot2,Shoot3;
public int bulb = 0 ,dart = 0;
public string resourceName = "Backgrounds";

 public Sprite[] backgrounds = new Sprite[3];

	// Use this for initialization
	void Start () 
	{
		backgrounds[0] = Resources.Load<Sprite> ("HUD/Off");
		backgrounds[1] = Resources.Load<Sprite> ("HUD/Green");
		backgrounds[2] = Resources.Load<Sprite> ("HUD/Red");
		ResetIndicatorPanel();
	}

	public void ResetIndicatorPanel()
	{
		for(int i =1; i<=3; i++)
		{
			ToggleIndicator(i, 0);
		}
		//ToastText.text = "";
	}

    public void SetText(string commentary)
    {
        ToastText.text = commentary;
	}

	public void ToggleIndicator(int indicatorID, int state)
	{
		string panelID = "Off" + indicatorID;
		GameObject.Find (panelID).GetComponent<Image> ().sprite = backgrounds[state];
	}
}