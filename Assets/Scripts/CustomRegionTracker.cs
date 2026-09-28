using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks region game mode click count for display purposes (non-persistent).
/// </summary>
public class CustomRegionTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _regionClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the region click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _regionClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _regionClicked.ToString() + "X ";
        }
    }
}