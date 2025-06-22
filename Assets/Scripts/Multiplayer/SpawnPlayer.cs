using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SpawnPlayer : MonoBehaviour
{
    // Singleton instance
    public static SpawnPlayer Instance
    { 
        get; 
        private set; 
    }

    public GameObject neutralPlayer;
    [Header("Player Prefabs")]
    public GameObject bluePlayer;
    public GameObject goldPlayer;

    private const string ThePanthers = "ThePanthers";
    private const string TheAints = "TheAints";

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Spawn(string playerNickname)
    {
        string playerPrefab = "";
        
        if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
        {
            Destroy(neutralPlayer);
            
            switch(playerNickname)
            {
                case ThePanthers:
                    playerPrefab = bluePlayer.name;
                    break;
                case TheAints:
                    playerPrefab = goldPlayer.name;
                    break;
                default:
                    Debug.LogError("<color=red>Error:</color>Please Choose a Team.");
                    break;
            }
            GameObject player = PhotonNetwork.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Debug.Log($"<color=#4BAAC8>Photon:</color> {playerNickname} Player has been Spawned.");
        }
        else
        {
            Debug.LogError("<color=#4BAAC8>Photon:</color> Cannot spawn player. Not connected to Photon or not in a room.");
        }
    }
}
