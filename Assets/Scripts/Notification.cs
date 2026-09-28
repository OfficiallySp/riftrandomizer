using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public GameObject popupPanel;  // Link this to your Popup panel in the editor
    public AudioClip successClip; // Attach a "Success" audio clip here in the editor
    public AudioClip failureClip; // Attach a "Failure" audio clip here in the editor
    public float soundDelay = 0.5f; // Delay in seconds before the sound is played
    public float soundVolume = 1.0f; // Sound volume (0.0 to 1.0)
    public float soundPitch = 1.0f; // Sound pitch
    public float retryDelay = 2.0f; // Delay between retry attempts in seconds
    private AudioSource audioSource;
    private int retryCount = 0;

    public static Notification Instance; // Singleton Instance

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Attach an AudioSource component to the GameObject
        StartCoroutine(CheckInternetConnection());
    }

    IEnumerator CheckInternetConnection()
    {
        // Load config for connectivity URL
        GameConfig config = Resources.Load<GameConfig>("GameConfig");
        string connectivityUrl = config != null ? config.connectivityCheckUrl : "https://officiallysp.net";
        float retryDelayValue = config != null ? config.retryDelay : retryDelay;

        // First, check basic internet reachability
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            ShowPopup("No Internet Connection. Working Offline", failureClip);
            yield break;
        }

        // Now check if we can reach a specific URL
        UnityWebRequest www = UnityWebRequest.Get(connectivityUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            ShowPopup($"{connectivityUrl} is down, Retrying...", failureClip);
            retryCount++;
            yield return new WaitForSeconds(retryDelayValue * retryCount);
            StartCoroutine(CheckInternetConnection());
        }
        else
        {
            ShowPopup($"Connected to {connectivityUrl}", successClip);
            retryCount = 0;
        }
    }

    public void ShowPopup(string message, AudioClip clip)
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("Notification: popupPanel is not assigned.");
            return;
        }

        popupPanel.SetActive(true);
        Text messageText = popupPanel.GetComponentInChildren<Text>();
        if (messageText != null)
        {
            messageText.text = message;
        }
        else
        {
            Debug.LogWarning("Notification: No Text component found in popupPanel.");
        }
        StartCoroutine(PlaySoundWithDelay(clip, soundDelay));
    }

    public void Notify(string message)
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("Notification: popupPanel is not assigned.");
            return;
        }

        popupPanel.SetActive(true);
        Text messageText = popupPanel.GetComponentInChildren<Text>();
        if (messageText != null)
        {
            messageText.text = message;
        }
        PlaySound(successClip);
    }

    public void Error(string message)
    {
        if (popupPanel == null)
        {
            Debug.LogWarning("Notification: popupPanel is not assigned.");
            return;
        }

        popupPanel.SetActive(true);
        Text messageText = popupPanel.GetComponentInChildren<Text>();
        if (messageText != null)
        {
            messageText.text = message;
        }
        PlaySound(failureClip);
    }
    IEnumerator PlaySoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(clip);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.volume = soundVolume;
            audioSource.pitch = soundPitch;
            audioSource.Play();
        }
    }
}
