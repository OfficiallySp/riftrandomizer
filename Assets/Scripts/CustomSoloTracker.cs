using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks solo game mode click count for display purposes (non-persistent).
/// </summary>
public class CustomSoloTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _soloClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the solo click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _soloClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _soloClicked.ToString() + "X ";
        }
    }
}