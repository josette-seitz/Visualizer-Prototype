using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string RoomName = "Panthers_vs_TheAints";
    private string playerNickname;

    void Start()
    {
        // For testing - Non XR
        //ConnectToServer("ThePanthers");
    }

    public void ConnectToServer(string playerName)
    {
        PhotonNetwork.ConnectUsingSettings();
        playerNickname = playerName;
        Debug.Log("<color=#4BAAC8>Photon:</color> Try Connecting to Server...");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("<color=#4BAAC8>Photon:</color> Connected to Server.");
        base.OnConnectedToMaster();
        // Lobby: players can see available rooms or create new ones.
        // In this case no lobby needed, create/join room.
        // PhotonNetwork.JoinLobby();

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log($"<color=#4BAAC8>Photon:</color> {playerNickname} Player has Joined Room.");
        base.OnJoinedRoom();

        // Spawn Player
        PhotonNetwork.LocalPlayer.NickName = playerNickname;
        SpawnPlayer.Instance.Spawn(playerNickname);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("<color=#4BAAC8>Photon:</color> Player Failed to Join Room.");
        base.OnJoinRoomFailed(returnCode, message);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"<color=#4BAAC8>Photon:</color> New Player has Joined the Room.");
        base.OnPlayerEnteredRoom(newPlayer);
    }
}
