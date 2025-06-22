using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Header("Teleport To")]
    [SerializeField]
    private Transform movePlayerTo;

    private const string TeleportPlayer = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TeleportPlayer))
        {
            other.transform.SetLocalPositionAndRotation(movePlayerTo.localPosition, movePlayerTo.localRotation);
        }
    }
}
