using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Counts up timer displaying days, hours, minutes, and seconds.
/// </summary>
public class CounterTimer : MonoBehaviour
{
    [Header("UI References")]
    public Text timerText;

    [Header("Timer Values")]
    public float dayCount;
    public float hourCount;
    public float minuteCount;
    public float secondsCount;
    public float millisecondsCount;

    [Header("Timer State")]
    public bool isStopped = false;

    private const float MILLISECONDS_TO_SECONDS = 1000f;
    private const float MAX_MILLISECONDS = 999f;
    private const float SECONDS_PER_MINUTE = 60f;
    private const float MINUTES_PER_HOUR = 60f;
    private const float HOURS_PER_DAY = 24f;
    private const string TIMER_FORMAT = "{0:00}:{1:00}:{2:00}:{3:00}";

    private void Update()
    {
        if (!isStopped)
        {
            UpdateTimerUI();
        }
    }

    /// <summary>
    /// Updates the timer display and increments values.
    /// </summary>
    public void UpdateTimerUI()
    {
        if (timerText == null) return;

        millisecondsCount += Time.deltaTime * MILLISECONDS_TO_SECONDS;
        timerText.text = string.Format(TIMER_FORMAT, dayCount, hourCount, minuteCount, secondsCount);

        if (millisecondsCount >= MAX_MILLISECONDS)
        {
            millisecondsCount = 0;
            secondsCount++;
        }
        else if (secondsCount >= SECONDS_PER_MINUTE)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (minuteCount >= MINUTES_PER_HOUR)
        {
            hourCount++;
            minuteCount = 0;
        }
        else if (hourCount >= HOURS_PER_DAY)
        {
            dayCount++;
            hourCount = 0;
        }
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void EndTimer()
    {
        isStopped = true;
    }

    /// <summary>
    /// Resets the timer to zero and starts it.
    /// </summary>
    public void ResetTimer()
    {
        isStopped = false;
        millisecondsCount = 0;
        secondsCount = 0;
        minuteCount = 0;
        hourCount = 0;
        dayCount = 0;
    }
}
