using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks button click count with persistence across sessions.
/// </summary>
public class ButtonTrackerPersist : MonoBehaviour
{
    public Text stattext;

    private SettingsManager _settingsManager;
    private int _buttonClicked;

    private void Start()
    {
        _settingsManager = SettingsManager.Instance;
        if (_settingsManager != null)
        {
            _buttonClicked = _settingsManager.GetButtonClickedCount();
        }
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the button click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _buttonClicked++;
        if (_settingsManager != null)
        {
            _settingsManager.SetButtonClickedCount(_buttonClicked);
        }
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _buttonClicked.ToString() + "X ";
        }
    }

    private void OnApplicationQuit()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetButtonClickedCount(_buttonClicked);
        }
    }
}
