using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic Network Manager Class. Responsible for Creating / Joining Rooms and handling game connection / disconnection
/// </summary>
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Tooltip("Networked Player Object")]
    public GameObject playerPrefab;

    [Tooltip("Canvas To Disable when going online")]
    public GameObject playerCanvas;


    Recorder primaryRecorder;
    bool connecting = false;
    string gameVersion = "1";

    [SerializeField]
    private byte maxPlayersPerRoom = 20;


    private void Awake()
    {
        primaryRecorder = GetComponent<Recorder>();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") &! connecting)
        {
            connecting = true;
            Connect();
            WorldTextInfo.singleton.NewMessage("Connecting Online");
        }
    }

    /// <summary>
    /// Basic method to initialize the most simple connection and go online
    /// </summary>
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #region Pun Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected To Master -- PUN");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Diconnected: " + cause.ToString() + " ||| -- PUN");
        WorldTextInfo.singleton.NewMessage("- Disconnected -\nPress (A) / (Enter) To Join Game");
        playerCanvas.SetActive(true);
        connecting = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room -- PUN");
        if (playerPrefab != null)
        {
            NetworkPlayerController networkController = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0).GetComponent<NetworkPlayerController>();

            networkController.SetupNetworkPlayer((NetworkPlayerType)Asymmetry.AsymmetricCrossPlatformController.singleton.playerType);

            WorldTextInfo.singleton.NewMessage("Joined Online Room");
            primaryRecorder.StartRecording();
            playerCanvas.SetActive(false);
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed To Join Room. | " + message + " | Creating Room Now! -- PUN");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo roomInfo in roomList)
        {
            print(roomInfo.Name);
        }
    }
    #endregion
}
