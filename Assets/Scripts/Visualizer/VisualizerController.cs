using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Enums;

public class VisualizerController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField]
    private GameObject quarterback;
    [SerializeField]
    private GameObject wideReceiver;
    [SerializeField]
    private GameObject runningBack;
    [Header("Football")]
    [SerializeField]
    private GameObject football;
    [Header("Switch POV")]
    [SerializeField]
    private List<SwitchPOV> switchPOV;
    [SerializeField]
    private GameObject locomotionSystem;
    [Header("UI Menu")]
    [SerializeField]
    private GameObject menu;
    [SerializeField]
    private List<TextMeshPro> loadedPlayName;
    [SerializeField]
    private TextMeshPro durationPlayTime;

    private const string PlayDataFolder = "PlayData";
    private const string PassingPlayFile = "passing_play";
    private const string RunningPlayFile = "running_play";

    private Vector3 initialRefPosition;
    private Quaternion initialRefRotation;
    private PlayData playData;
    private int povIndex;
    private float playbackSpeed = 1.5f;
    private Transform player;

    public Transform CurrentPlayer => player;

    private void Start()
    {
        // Set Current player
        player = switchPOV[0].gameObject.transform;

        // Reset REF position and rotation
        initialRefPosition = switchPOV[0].transform.position;
        initialRefRotation = switchPOV[0].transform.rotation;
    }

    public void LoadPlayData(int playFile)
    {
        var play = (Play)playFile;
        string playDataFile = string.Empty;
        
        switch(play)
        {
            case Play.Passing:
                playDataFile = $"{PlayDataFolder}/{PassingPlayFile}";
                break;
            case Play.Running:
                playDataFile = $"{PlayDataFolder}/{RunningPlayFile}";
                break;
            default:
                Debug.LogError("Error: Select Play Error. Could not load Play Data.");
                break;
        }

        if (playDataFile != string.Empty)
        {
            TextAsset playJson = Resources.Load<TextAsset>(playDataFile);
            playData = JsonUtility.FromJson<PlayData>(playJson.text);

            Debug.Log($"Loaded play: {playData.metadata.playName}");
        }
    }

    public void SetXRPovIndex(int povCamera)
    {
        povIndex = povCamera;
    }

    public void SetPlaySpeed(float speed)
    {
        // Set speed
        playbackSpeed = speed;

        // Set POV
        SetXRPov();
    }

    public void ReplayPlay()
    {
        // Turn XR Controllers OFF except REF
        if (povIndex != 0)
        {
            foreach (var controller in switchPOV[povIndex].Controllers)
            {
                controller.SetActive(false);
            }
        }

        // Replay Visualizer Play
        StartCoroutine(PlaybackMovement(playData));
    }

    private void SetXRPov()
    {
        for (int i = 0; i < switchPOV.Count; i++)
        {
            if (i == povIndex)
            {
                // Set Current player
                player = switchPOV[i].gameObject.transform;

                // Turn Main Camera ON
                switchPOV[i].MainCamera.enabled = true;
                // Turn XR Tracking ON
                switchPOV[i].TrackedPoseDriver.enabled = true;

                // Turn REF XR Controllers ON
                if (i == 0)
                {
                    foreach (var controller in switchPOV[i].Controllers)
                    {
                        controller.SetActive(true);
                    }

                    locomotionSystem.SetActive(true);
                }
                    
                // Disable Menu
                menu.SetActive(false);
            }
            else
            {
                // Turn Main Camera OFF
                switchPOV[i].MainCamera.enabled = false;
                // Turn XR Tracking OFF
                switchPOV[i].TrackedPoseDriver.enabled = false;

                // Turn XR Controllers OFF
                foreach (var controller in switchPOV[i].Controllers)
                {
                    controller.SetActive(false);
                }
            }
        }

        // Start Visualizer Play
        StartCoroutine(PlaybackMovement(playData));
    }

    private IEnumerator PlaybackMovement(PlayData play)
    {
        float elapsed = 0f;
        float duration = play.metadata.duration;

        // Reset REF position and rotation
        if (povIndex != 0)
        {
            locomotionSystem.SetActive(false);

            if (initialRefPosition != switchPOV[0].transform.position || initialRefRotation != switchPOV[0].transform.rotation)
            {
                switchPOV[0].transform.SetPositionAndRotation(initialRefPosition, initialRefRotation);
            }
        }

        AudioManager.Instance.BeginningAudio(true);

        // Show Loaded Play name
        foreach(var lp in loadedPlayName)
        {
            lp.text = play.metadata.playName;
        }

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime * playbackSpeed;

            UpdateTransformAtTime(quarterback, play.qb, elapsed);
            UpdateTransformAtTime(wideReceiver, play.wr, elapsed);
            UpdateTransformAtTime(runningBack, play.rb, elapsed);
            UpdateFootballPosition(football, play.football, elapsed);

            yield return null;
        }

        // Move Menu
        StartCoroutine(PlayEnded());
    }

    private void UpdateTransformAtTime(GameObject player, List<PlayerFrame> frames, float time)
    {
        if (frames.Count == 0)
        {
            Debug.LogError($"Data is missing for Player: {player.name}");
            return;
        }

        for (int i = 0; i < frames.Count - 1; i++)
        {
            PlayerFrame a = frames[i];
            PlayerFrame b = frames[i + 1];

            if (time >= a.time && time <= b.time)
            {
                float t = Mathf.InverseLerp(a.time, b.time, time);
                Vector3 pos = Vector3.Lerp(ToVector3(a.position), ToVector3(b.position), t);
                Quaternion rot = Quaternion.Slerp(ToQuaternion(a.rotation), ToQuaternion(b.rotation), t);
                player.transform.SetPositionAndRotation(pos, rot);
                break;
            }
        }
    }

    private void UpdateFootballPosition(GameObject football, List<FootballFrame> frames, float time)
    {
        if (frames.Count == 0)
        {
            Debug.LogError($"Data is missing for Football: {football.name}");
            return;
        }

        football.transform.rotation = ToQuaternion(frames[0].rotation);

        for (int i = 0; i < frames.Count - 1; i++)
        {
            FootballFrame a = frames[i];
            FootballFrame b = frames[i + 1];

            if (time >= a.time && time <= b.time)
            {
                float t = Mathf.InverseLerp(a.time, b.time, time);
                Vector3 pos = Vector3.Lerp(ToVector3(a.position), ToVector3(b.position), t);
                football.transform.position = pos;
                break;
            }
        }
    }

    private Vector3 ToVector3(float[] arr)
    {
        var vector3 = new Vector3(arr[0], arr[1], arr[2]);

        return vector3;
    }

    private Quaternion ToQuaternion(float[] rot)
    {
        var quaternion = Quaternion.Euler(rot[0], rot[1], rot[2]);

        return quaternion;
    }

    private IEnumerator PlayEnded()
    {
        // Play ending audios
        AudioManager.Instance.EndingAudio();
        
        yield return new WaitForSeconds(1f);

        // Fade Out Crowd Cheering
        AudioManager.Instance.BeginningAudio(false);

        // Pos: apply offset in player's local space
        var menuOffset = menu.GetComponent<UIController>().MenuToPlayerOffset;
        menu.transform.position = player.TransformPoint(menuOffset);
        // Rot: have menu face the same direction as player
        menu.transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

        durationPlayTime.text = $"{playData.metadata.duration}seconds";
        menu.SetActive(true);

        // Turn XR Controllers ON
        var currPlayerControllers = player.GetComponent<SwitchPOV>().Controllers;

        foreach (var controller in currPlayerControllers)
        {
            controller.SetActive(true);
        }
    }
}
