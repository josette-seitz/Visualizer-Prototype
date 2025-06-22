using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource crowdCheerAudio;
    [SerializeField]
    private AudioSource firstDownAudio;
    [SerializeField]
    private AudioSource pantherRoarAudio;

    private float crowdCheerStartVol;
    private float duration = 3f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Crowd Cheer Start Volume
        crowdCheerStartVol = crowdCheerAudio.volume;
    }

    public void BeginningAudio(bool play)
    {
        if (play)
            crowdCheerAudio.Play();
        else
            StartCoroutine(FadeOutCrowdCheer());

    }

    public void EndingAudio()
    {
        firstDownAudio.PlayDelayed(0.35f);
        pantherRoarAudio.Play();
    }

    private IEnumerator FadeOutCrowdCheer()
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            crowdCheerAudio.volume = Mathf.Lerp(crowdCheerStartVol, 0f, time / duration);
            yield return null;
        }

        crowdCheerAudio.Stop();
        // Reset for next play
        crowdCheerAudio.volume = crowdCheerStartVol; // Reset for next time
    }
}
