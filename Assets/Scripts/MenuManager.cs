using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class MenuManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private GameObject userNameScreen, ConnectScreen;
    [SerializeField]
    private GameObject CreateUserNameButton;
    [SerializeField]
    private InputField UserNameInput, CreateRoomInput, JoinRoomInput;

    

    void Awake()
    {
        //Photon use our photon network settings
        PhotonNetwork.ConnectUsingSettings();

    }

   

    //this function will be called by photon then we succesfully connected to server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master!!!!");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    //After succesfully connected to master photon will call another function

    public override void OnJoinedLobby()
    {
        Debug.Log("Connected to Lobby!!!!");
        userNameScreen.SetActive(true);
    }

    //When we succesfully connected to a room Photon is going to call another function calledOnJoinRoom

        
    public override void OnJoinedRoom()
    {
        //Load game scene

        PhotonNetwork.LoadLevel(1);

    }
    



    #region  UIMethods

    public void OnClick_CreateNameButton()
    {

        PhotonNetwork.NickName = UserNameInput.text;
        userNameScreen.SetActive(false);
        ConnectScreen.SetActive(true);


    }

    public void OnNameField_Changed()
    {
        if (UserNameInput.text.Length >= 2)
        {
            CreateUserNameButton.SetActive(true);
        }
        else
        {
            CreateUserNameButton.SetActive(false);
        }
    }

    public void Onclick_JoinRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(JoinRoomInput.text, ro, TypedLobby.Default);
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 4; //we are setting maximum player number to 4
        PhotonNetwork.CreateRoom(CreateRoomInput.text, ro, null);
    }

    #endregion

}
