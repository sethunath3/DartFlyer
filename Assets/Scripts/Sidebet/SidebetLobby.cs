using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class SidebetLobby : MonoBehaviourPunCallbacks
{
    enum SceneMode
    {
        Loading = 0,
        Lobby,
        Room,
    }

    class PlayerInfo
    {
        public string playerName;
        public int betColor;
        public int betAmount;
        public SideBetPlayerCell playerCell;
    }

    [SerializeField] List<Canvas> roomUiList;
    [SerializeField] SideBetPlayerCell playerCellPrefab;

    [Header("LobbyComponents")]
    [SerializeField] TMP_InputField roomNameField;

    [Header("RoomComponents")]
    [SerializeField] TMP_Text roomCodeText;
    [SerializeField] GameObject popupPrefab;
    [SerializeField] Text userName;
    [SerializeField] Image betcolorIndicator;
    [SerializeField] Text betAmountDisplay;
    [SerializeField] GameObject playerCellHolder;

    [SerializeField] GameObject bettingPopup;
    [SerializeField] Image _popupBetColorIndicator;
    [SerializeField] Text _popupBetAmountindicator;

    private int betAmount;
    private int betColour;
    private DartGameUtils gameUtil;

    int colorIndex;
    int betAmountIndex;

    private Dictionary<string, PlayerInfo> playerData = new Dictionary<string, PlayerInfo>();

    string roomId = "";
    void Start()
    {
        ClearPlayerCells();
        bettingPopup.SetActive(false);
        SetUITo(SceneMode.Loading);
        //This makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            //Set the App version before connecting
            PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = Application.version;
            // Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void ClearPlayerCells()
    {
        foreach(Transform child in playerCellHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void SetUITo(SceneMode sceneMode)
    {
        foreach(Canvas sceneUi in roomUiList)
        {
            sceneUi.gameObject.SetActive(false);
        }
        roomUiList[(int)sceneMode].gameObject.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + cause.ToString() + " ServerAddress: " + PhotonNetwork.ServerAddress);
    }

    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.NickName = GameManager.userInfo.name;
        PhotonNetwork.NickName = "Sethu";
        Debug.Log("OnConnectedToMaster");
        SetUITo(SceneMode.Lobby);
        //After we connected to Master server, join the Lobby
        //PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    /*public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("We have received the Room list");
        //After this callback, update the room list
        createdRooms = roomList;
    }*/

    public void OnCreateRoomClick()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = (byte)5; //Set any number
        roomOptions.PublishUserId = true;
        SetUITo(SceneMode.Loading);
        roomId = GenerateRandomRoomName();
        PhotonNetwork.CreateRoom(roomId, roomOptions, TypedLobby.Default);
    }

    public void OnJoinRoomClick()
    {
        //Join the Room
        SetUITo(SceneMode.Loading);
        PhotonNetwork.JoinRoom(roomNameField.text);
    }

    public void OnChangeBetClicked()
    {
        colorIndex = 1;
        betAmountIndex = 0;
        RefreshBetPopup();
        bettingPopup.SetActive(true);
    }

    private void RefreshBetPopup()
    {
        _popupBetAmountindicator.text = GameManager.betamountList[betAmountIndex].ToString();
        _popupBetColorIndicator.color = (Color)gameUtil.colourMap[colorIndex];
    }

    public void ONBetUpBtnClicked()
    {
        if (betAmountIndex >= GameManager.betamountList.Length - 1)
        {
            betAmountIndex = 0;
        }
        else
        {
            betAmountIndex++;
        }
        RefreshBetPopup();
    }
    public void ONBetDownBtnClicked()
    {
        if (betAmountIndex == 0)
        {
            betAmountIndex = GameManager.betamountList.Length - 1;
        }
        else
        {
            betAmountIndex--;
        }
        RefreshBetPopup();
    }
    public void ONColorUpBtnClicked()
    {
        if (colorIndex >= gameUtil.colourMap.Count)
        {
            colorIndex = 1;
        }
        else
        {
            colorIndex++;
        }
        RefreshBetPopup();
    }
    public void ONColorDownBtnClicked()
    {
        if (colorIndex == 0)
        {
            colorIndex = gameUtil.colourMap.Count;
        }
        else
        {
            colorIndex--;
        }
        RefreshBetPopup();
    }

    public void OnSaveBetBtnClicked()
    {
        betcolorIndicator.color = (Color)gameUtil.colourMap[colorIndex];
        betAmountDisplay.text = GameManager.betamountList[betAmountIndex].ToString();
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SetOrAddPlayerBetTo", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.NickName, colorIndex, GameManager.betamountList[betAmountIndex]);
        bettingPopup.SetActive(false);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnCreateRoomFailed got called. This can happen if the room exists (even if not visible). Try another room name.");
        //joiningRoom = false;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRoomFailed got called. This can happen if the room is not existing or full or closed.");
        //joiningRoom = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed got called. This can happen if the room is not existing or full or closed.");
        //joiningRoom = false;
    }

    public override void OnCreatedRoom()
    {
        //SetUITo(SceneMode.Room);
        Debug.Log("OnCreatedRoom");
        
        //Load the Scene called GameLevel (Make sure it's added to build settings)
        //PhotonNetwork.LoadLevel("GameLevel");
    }

    public override void OnJoinedRoom()
    {
        roomCodeText.text = roomId;
        //userName.text = GameManager.userInfo.name;
        userName.text = "Sethunath";
        gameUtil = new DartGameUtils();
        betAmount = GameManager.betamountList[GameManager.betamountList.Length - 1];
        betColour = UnityEngine.Random.Range(1, 7);
        GameManager.playerBets.Clear();
        DartGameUtils.BetStructure betStruct = new DartGameUtils.BetStructure
        {
            BetAmount = betAmount,
            BetColour = betColour
        };
        GameManager.playerBets.Add(betStruct);
        GameManager.GetInstance().SetCurrentGameMode(DartGameUtils.GameMode.SideBetMode);

        betcolorIndicator.GetComponent<Image>().color = gameUtil.colourMap[betColour];
        betAmountDisplay.text = betAmount.ToString();
        SetUITo(SceneMode.Room);
        Debug.Log("OnJoinedRoom");


        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SetOrAddPlayerBetTo", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.UserId, PhotonNetwork.NickName, betColour, betAmount);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
    }

    private string GenerateRandomRoomName()
    {
        var desiredCodeLength = 5;
        var code = "";
        char[] characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        while (code.Length < desiredCodeLength)
        {
            code += characters[Random.Range(0, characters.Length)];
        }
        return code;
    }

    [PunRPC]
    void SetOrAddPlayerBetTo(string _playerId, string _playerName, int _betColor, int _betValue)
    {
        Debug.Log(string.Format("Bet set by: {0}: {1}", _playerName, _betColor));
        if(playerData.ContainsKey(_playerId))
        {
            playerData[_playerId].playerName = _playerName;
            playerData[_playerId].betAmount = _betValue;
            playerData[_playerId].betColor = _betColor;
            playerData[_playerId].playerCell.SetPlayerData(_playerName, gameUtil.colourMap[_betColor], _betValue.ToString());
        }
        else
        {
            PlayerInfo player = new PlayerInfo();
            player.playerName = _playerName;
            player.betColor = _betColor;
            player.betAmount = _betValue;
            player.playerCell = Instantiate<SideBetPlayerCell>(playerCellPrefab);
            player.playerCell.SetPlayerData(_playerName, gameUtil.colourMap[_betColor], _betValue.ToString());
            player.playerCell.transform.parent = playerCellHolder.transform;
            playerData.Add(_playerId, player);
        }
    }
}
