using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks custom game mode click count with persistence across sessions.
/// </summary>
public class CustomTrackerPersist : MonoBehaviour
{
    public Text stattext;

    private SettingsManager _settingsManager;
    private int _customClicked;

    private void Start()
    {
        _settingsManager = SettingsManager.Instance;
        if (_settingsManager != null)
        {
            _customClicked = _settingsManager.GetCustomClickedCount();
        }
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the custom click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _customClicked++;
        if (_settingsManager != null)
        {
            _settingsManager.SetCustomClickedCount(_customClicked);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _customClicked.ToString() + "X ";
        }
    }

    private void OnApplicationQuit()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetCustomClickedCount(_customClicked);
        }
    }
}