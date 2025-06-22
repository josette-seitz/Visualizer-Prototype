using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SetTeleportProvider : MonoBehaviour
{
    void Start()
    {
        // Grab Teleport Provider
        var teleportProvider = GetComponent<TeleportationProvider>();
        
        // Set Teleport Provider
        var teleportArea = GameObject.FindObjectOfType<TeleportationArea>();
        teleportArea.teleportationProvider = teleportProvider;
    }
}
