using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

/// <summary>
/// Main application manager handling initialization, settings, and scene management.
/// </summary>
public class AppManager : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer Video;

    [Header("Lock UI Elements")]
    public GameObject LockedName1;
    public GameObject LockedName2;
    public GameObject LockOn;
    public GameObject LockOff;

    [Header("Lane UI Elements")]
    public GameObject LanesLeft;
    public GameObject LanesRight;
    public GameObject LaneToggleOn;
    public GameObject LaneToggleOff;

    private GameConfig _config;
    private SettingsManager _settingsManager;

    private void Start()
    {
        Application.backgroundLoadingPriority = UnityEngine.ThreadPriority.Low;

        // Load config
        _config = Resources.Load<GameConfig>("GameConfig");
        if (_config != null)
        {
            Application.targetFrameRate = _config.targetFrameRate;
        }
        else
        {
            Application.targetFrameRate = 40;
            Debug.LogWarning("AppManager: GameConfig not found, using default frame rate.");
        }

        // Initialize settings manager
        _settingsManager = SettingsManager.Instance;

        // Load and apply settings
        ApplyLockSettings();
        ApplyLaneSettings();
    }

    private void ApplyLockSettings()
    {
        bool lockStatus = _settingsManager.GetLockStatus();

        if (LockedName1 != null) LockedName1.SetActive(lockStatus);
        if (LockedName2 != null) LockedName2.SetActive(lockStatus);
        if (LockOn != null) LockOn.SetActive(lockStatus);
        if (LockOff != null) LockOff.SetActive(!lockStatus);
    }

    private void ApplyLaneSettings()
    {
        bool laneStatus = _settingsManager.GetLaneStatus();

        if (LanesLeft != null) LanesLeft.SetActive(!laneStatus);
        if (LanesRight != null) LanesRight.SetActive(!laneStatus);
        if (LaneToggleOn != null) LaneToggleOn.SetActive(!laneStatus);
        if (LaneToggleOff != null) LaneToggleOff.SetActive(laneStatus);
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void NameSelect()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }
}
