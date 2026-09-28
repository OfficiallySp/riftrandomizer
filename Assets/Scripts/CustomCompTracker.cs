using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks composition game mode click count for display purposes (non-persistent).
/// </summary>
public class CustomCompTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _compClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the composition click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _compClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _compClicked.ToString() + "X ";
        }
    }
}