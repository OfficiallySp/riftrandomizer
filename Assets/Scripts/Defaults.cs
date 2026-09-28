using UnityEngine;

/// <summary>
/// Handles default settings toggles for lock and lane visibility.
/// </summary>
public class Defaults : MonoBehaviour
{
    private SettingsManager _settingsManager;

    private void Start()
    {
        _settingsManager = SettingsManager.Instance;
    }

    /// <summary>
    /// Sets lock status to enabled.
    /// </summary>
    public void LockToggle()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetLockStatus(true);
        }
    }

    /// <summary>
    /// Sets lock status to disabled.
    /// </summary>
    public void LockUnToggle()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetLockStatus(false);
        }
    }

    /// <summary>
    /// Sets lane visibility to hidden.
    /// </summary>
    public void LaneToggle()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetLaneStatus(true);
        }
    }

    /// <summary>
    /// Sets lane visibility to visible.
    /// </summary>
    public void LaneUnToggle()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetLaneStatus(false);
        }
    }
}
