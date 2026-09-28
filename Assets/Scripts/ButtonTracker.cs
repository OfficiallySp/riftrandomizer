using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks button click count for display purposes (non-persistent).
/// </summary>
public class ButtonTracker : MonoBehaviour
{
    [Header("UI References")]
    public Text stattext;

    private int _buttonClicked = 0;

    private void Start()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Increments the button click count and updates the display.
    /// </summary>
    public void ButtonClick()
    {
        _buttonClicked++;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (stattext != null)
        {
            stattext.text = _buttonClicked.ToString() + "X ";
        }
    }
}
