using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject locomotionSystem;
    [SerializeField]
    private Transform leftController;
    [SerializeField]
    private Transform rightController;

    //[SerializeField]
    //List<GameObject> nonLocalPlayer;
    //[Space]
    //[SerializeField]
    //private Camera mainCamera;
    //[SerializeField]
    //private ActionBasedController leftController;
    //[SerializeField]
    //private ActionBasedController rightController;

    private new PhotonView photonView;
    private Transform cameraTransform;

    private void Start()
    {
        Camera camera = GetComponentInChildren<Camera>();
        AudioListener listener = GetComponentInChildren<AudioListener>();
        TrackedPoseDriver trackedPose = GetComponentInChildren<TrackedPoseDriver>();

        photonView = GetComponent<PhotonView>();
        cameraTransform = camera.transform;

        if (!photonView.IsMine)
        {
            camera.enabled = false;
            listener.enabled = false;
            trackedPose.enabled = false;
        }
    }

    void Update()
    {
        // If local player, then move
        //if (photonView.IsMine)
        //{
        //    Look();
        //    Move();
        //}

        // If local player, then move
        if (photonView.IsMine)
        {
            locomotionSystem.SetActive(true);
        }
        else
        {
            locomotionSystem.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        // This was the trick!!! Now figure out with Players XR
        if (photonView.IsMine)
        {
            photonView.RPC("UpdatePlayerTransform", RpcTarget.Others, transform.position, transform.position, 
                cameraTransform.localPosition, cameraTransform.localRotation, leftController.localPosition,
                leftController.localRotation, rightController.localPosition, rightController.localRotation);
        }
    }

    [PunRPC]
    void UpdatePlayerTransform(Vector3 playerRootPos, Quaternion playerRootRot, Vector3 cameraTransformPos, Quaternion cameraTransformRot,
        Vector3 leftControllerPos, Quaternion leftControllerRot, Vector3 rightControllerPos, Quaternion rightControllerRot)
    {
        // Player Root Transform
        transform.position = playerRootPos;
        transform.rotation = playerRootRot;   

        // Main Camera Transform
        cameraTransform.localPosition = cameraTransformPos;
        cameraTransform.localRotation = cameraTransformRot;

        // TODO: Need to test
        // Controller Transform
        leftController.localPosition = leftControllerPos;
        leftController.localRotation = leftControllerRot;
        rightController.localPosition = rightControllerPos;
        rightController.localRotation = rightControllerRot;
    }
}
