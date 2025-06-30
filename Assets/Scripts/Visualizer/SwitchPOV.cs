using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Visualizer
{
    public class SwitchPOV : MonoBehaviour
    {
        [Header("XR Camera")] [SerializeField] private Camera mainCamera;
        [SerializeField] private TrackedPoseDriver trackedPoseDriver;

        [Header("Controllers")] [SerializeField]
        private List<GameObject> controllers;

        // Getters
        public Camera MainCamera => mainCamera;
        public TrackedPoseDriver TrackedPoseDriver => trackedPoseDriver;
        public List<GameObject> Controllers => controllers;
    }
}
