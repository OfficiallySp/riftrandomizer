using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Tracks and displays total playtime across sessions.
/// </summary>
public class PersistTimer : MonoBehaviour
{
    public Text timertext;

    private SettingsManager _settingsManager;
    private int _playtime;
    private Coroutine _timerCoroutine;

    private const int SECONDS_PER_MINUTE = 60;
    private const int SECONDS_PER_HOUR = 3600;
    private const int SECONDS_PER_DAY = 86400;
    private const int DAYS_PER_YEAR = 365;

    private void Start()
    {
        _settingsManager = SettingsManager.Instance;
        if (_settingsManager != null)
        {
            _playtime = _settingsManager.GetPlaytime();
        }
        _timerCoroutine = StartCoroutine(PlayTimer());
    }

    private IEnumerator PlayTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            _playtime++;
            if (_settingsManager != null)
            {
                _settingsManager.SetPlaytime(_playtime);
            }
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (timertext == null) return;

        int seconds = _playtime % SECONDS_PER_MINUTE;
        int minutes = (_playtime / SECONDS_PER_MINUTE) % SECONDS_PER_MINUTE;
        int hours = (_playtime / SECONDS_PER_HOUR) % 24;
        int days = (_playtime / SECONDS_PER_DAY) % DAYS_PER_YEAR;

        timertext.text = $"{days}D {hours}H {minutes}M {seconds}S ";
    }

    private void OnDisable()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
        }
    }

    private void OnApplicationQuit()
    {
        if (_settingsManager != null)
        {
            _settingsManager.SetPlaytime(_playtime);
        }
    }
}