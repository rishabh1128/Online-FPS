using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject loadingScreen;
    public GameObject menuButtons;
    public TMP_Text loadingText;
    public GameObject createRoomScreen;
    public TMP_InputField roomNameInput;
    public GameObject roomScreen;
    public TMP_Text roomNameText;
    public GameObject errorScreen;
    public TMP_Text errorText;

    private const int MAX_PLAYERS = 4;

    // Start is called before the first frame update
    void Start()
    {
        CloseMenus(); //To close menus when game starts if they are left open accidentally

        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to Network...";

        PhotonNetwork.ConnectUsingSettings(); //connect using the settings we set up
    }

    void CloseMenus()
    {
        loadingScreen.SetActive(false);
        menuButtons.SetActive(false);
        createRoomScreen.SetActive(false);
        roomScreen.SetActive(false);
        errorScreen.SetActive(false);
    }

    public override void OnConnectedToMaster() //overriding already defined method to connect to master server
    {
        //base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
        loadingText.text = "Joining lobby...";
    }

    public override void OnJoinedLobby() //after joining a lobby ---- Master Server -> Lobby -> Room
    {
        //base.OnJoinedLobby();
        CloseMenus();
        menuButtons.SetActive(true);

    }

    public void OpenRoomCreate()
    {
        CloseMenus();
        createRoomScreen.SetActive(true);
    }

    public void CreateRoom()
    {  
        if (!string.IsNullOrWhiteSpace(roomNameInput.text))
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = MAX_PLAYERS; //set max players who can play together to MAX_PLAYERS    
        
            PhotonNetwork.CreateRoom(roomNameInput.text);

            CloseMenus();
            loadingText.text = "Creating Room...";
            loadingScreen.SetActive(true);
        }
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        CloseMenus();
        roomScreen.SetActive(true);
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnCreateRoomFailed(short returnCode, string message) //Can happen when duplicate rooms are created for e.g.
    {
        //base.OnCreateRoomFailed(returnCode, message);
        errorText.text = "Failed to Create Room : " + message;
        CloseMenus();
        errorScreen.SetActive(true);
    }

    public void CloseErrorScreen()
    {
        CloseMenus();
        menuButtons.SetActive(true);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseMenus();
        loadingText.text = "Leaving Room...";
        loadingScreen.SetActive(true);
    }

    public override void OnLeftRoom()
    {
        //base.OnLeftRoom();
        CloseMenus();
        menuButtons.SetActive(true);
    }
}
    