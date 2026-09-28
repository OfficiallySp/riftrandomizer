using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the current value of a speed slider.
/// </summary>
public class SpeedSlider : MonoBehaviour
{
    [Header("UI References")]
    public Slider slider;
    public Text text;

    private const string SPEED_FORMAT = "SPEED = {0:F2}X";

    private void LateUpdate()
    {
        if (text != null && slider != null)
        {
            text.text = string.Format(SPEED_FORMAT, slider.value);
        }
    }
}