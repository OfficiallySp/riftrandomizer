using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks skin game mode click count for display purposes (non-persistent).
/// </summary>
public class CustomSkinTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _skinClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the skin click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _skinClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _skinClicked.ToString() + "X ";
        }
    }
}