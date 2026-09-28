using UnityEngine;

/// <summary>
/// Centralized manager for PlayerPrefs settings with type-safe access.
/// </summary>
public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;

    public static SettingsManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("SettingsManager");
                _instance = go.AddComponent<SettingsManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // PlayerPrefs keys
    private const string KEY_LOCK_STATUS = "LockStatus";
    private const string KEY_LANE_STATUS = "LaneStatus";
    private const string KEY_BUTTON_CLICKED = "buttonclicked";
    private const string KEY_CUSTOM_CLICKED = "customclicked";
    private const string KEY_PLAYTIME = "playtime";

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Lock Status
    public bool GetLockStatus()
    {
        return PlayerPrefs.GetInt(KEY_LOCK_STATUS, 0) == 1;
    }

    public void SetLockStatus(bool locked)
    {
        PlayerPrefs.SetInt(KEY_LOCK_STATUS, locked ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Lane Status
    public bool GetLaneStatus()
    {
        return PlayerPrefs.GetInt(KEY_LANE_STATUS, 0) == 1;
    }

    public void SetLaneStatus(bool lanesHidden)
    {
        PlayerPrefs.SetInt(KEY_LANE_STATUS, lanesHidden ? 1 : 0);
        PlayerPrefs.Save();
    }

    // Button Clicked Count
    public int GetButtonClickedCount()
    {
        return PlayerPrefs.GetInt(KEY_BUTTON_CLICKED, 0);
    }

    public void SetButtonClickedCount(int count)
    {
        PlayerPrefs.SetInt(KEY_BUTTON_CLICKED, count);
        PlayerPrefs.Save();
    }

    public void IncrementButtonClickedCount()
    {
        int current = GetButtonClickedCount();
        SetButtonClickedCount(current + 1);
    }

    // Custom Clicked Count
    public int GetCustomClickedCount()
    {
        return PlayerPrefs.GetInt(KEY_CUSTOM_CLICKED, 0);
    }

    public void SetCustomClickedCount(int count)
    {
        PlayerPrefs.SetInt(KEY_CUSTOM_CLICKED, count);
        PlayerPrefs.Save();
    }

    public void IncrementCustomClickedCount()
    {
        int current = GetCustomClickedCount();
        SetCustomClickedCount(current + 1);
    }

    // Playtime
    public int GetPlaytime()
    {
        return PlayerPrefs.GetInt(KEY_PLAYTIME, 0);
    }

    public void SetPlaytime(int seconds)
    {
        PlayerPrefs.SetInt(KEY_PLAYTIME, seconds);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Saves all PlayerPrefs immediately.
    /// </summary>
    public void Save()
    {
        PlayerPrefs.Save();
    }
}

