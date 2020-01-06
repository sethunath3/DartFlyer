using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager  : MonoBehaviour{



    private DartGameUtils.GameMode currentGameMode;
    public static string currentGameId;
    public static string currentGameBetId;
	public int betColour;
	public int betAmount;
	public bool lobbyEntryTime = false;
    public static List<DartGameUtils.BetStructure> playerBets = new List<DartGameUtils.BetStructure>{};
	public static UserInfo userInfo;
    public static string userToken;


	private static GameManager Instance;

    private void Awake() {
         if (Instance == null) {
             Instance = this;
             DontDestroyOnLoad(Instance);
         }
     }

    public static GameManager GetInstance()
    {
        if (Instance == null) {
             Instance = new GameManager();
        }

        return Instance;
    } 
     

    public DartGameUtils.GameMode GetCurrentGameMode()
    {
        return currentGameMode;
    }

    public void SetCurrentGameMode(DartGameUtils.GameMode value)
    {
        currentGameMode = value;
    }
}
