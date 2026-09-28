using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks mirror game mode click count for display purposes (non-persistent).
/// </summary>
public class CustomMirrorTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _mirrorClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the mirror click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _mirrorClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _mirrorClicked.ToString() + "X ";
        }
    }
}