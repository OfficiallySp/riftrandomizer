using System;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Manages video playback based on day of week and time-based rotation.
/// </summary>
public class VideoPicker : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Start date for video rotation calculation")]
    public DateTime startDate = new DateTime(2022, 10, 9);

    [Tooltip("Days of week when videos should play")]
    public DayOfWeek[] playDays = { DayOfWeek.Saturday, DayOfWeek.Sunday, DayOfWeek.Thursday };

    private VideoPlayer _player;
    private VideoClip[] _videoBank;
    private int _currentVideo;

    private const int DAYS_PER_WEEK = 7;

    private void Awake()
    {
        _player = GetComponent<VideoPlayer>();
        if (_player == null)
        {
            Debug.LogError("VideoPicker: VideoPlayer component not found.");
            return;
        }

        _videoBank = Resources.LoadAll<VideoClip>("Video");
        if (_videoBank == null || _videoBank.Length == 0)
        {
            Debug.LogWarning("VideoPicker: No videos found in Resources/Video folder.");
        }
    }

    private void Start()
    {
        if (_player == null || _videoBank == null || _videoBank.Length == 0)
        {
            return;
        }

        DateTime currentDate = DateTime.Now.Date;
        DayOfWeek currentDay = currentDate.DayOfWeek;

        // Check if today is a play day
        bool shouldPlay = false;
        if (playDays != null && playDays.Length > 0)
        {
            foreach (DayOfWeek day in playDays)
            {
                if (currentDay == day)
                {
                    shouldPlay = true;
                    break;
                }
            }
        }

        if (shouldPlay)
        {
            // Calculate the number of weeks passed since the start date
            TimeSpan timeSpan = currentDate - startDate;
            int weeksPassed = Mathf.Max(0, timeSpan.Days / DAYS_PER_WEEK);

            // Calculate the current video index using the modulo operator
            _currentVideo = weeksPassed % _videoBank.Length;

            // Set the player's clip to this video and play it
            if (_currentVideo >= 0 && _currentVideo < _videoBank.Length && _videoBank[_currentVideo] != null)
            {
                _player.clip = _videoBank[_currentVideo];
                _player.Play();
            }
        }
    }
}
