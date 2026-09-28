using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles random rune selection with optional repeat prevention.
/// </summary>
public class Rollrune : MonoBehaviour
{
    [Header("Configuration")]
    public bool preventRepeatRune = true;

    [Header("Rune Objects")]
    public GameObject[] runes;

    private GameConfig _config;
    private int _currentRune = -1;
    private int _previousRune = -1;
    private const int DEFAULT_MAX_RUNES = 16;

    private void Start()
    {
        _config = Resources.Load<GameConfig>("GameConfig");
        InitializeRunes();
    }

    private void InitializeRunes()
    {
        if (runes == null) return;

        foreach (GameObject rune in runes)
        {
            if (rune != null)
            {
                rune.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Selects a random rune and displays it.
    /// </summary>
    public void RuneSelect()
    {
        if (runes == null || runes.Length == 0)
        {
            Debug.LogWarning("Rollrune: No runes assigned.");
            return;
        }

        int maxRunes = _config != null ? _config.maxRunes : DEFAULT_MAX_RUNES;
        maxRunes = Mathf.Min(maxRunes, runes.Length);

        int newRune;
        if (preventRepeatRune && maxRunes > 1)
        {
            do
            {
                newRune = Random.Range(0, maxRunes);
            } while (newRune == _previousRune);
        }
        else
        {
            newRune = Random.Range(0, maxRunes);
        }

        _currentRune = newRune;
        _previousRune = _currentRune;

        // Deactivate all runes
        InitializeRunes();

        // Activate selected rune
        if (_currentRune >= 0 && _currentRune < runes.Length && runes[_currentRune] != null)
        {
            runes[_currentRune].SetActive(true);
        }
    }

    /// <summary>
    /// Toggles the prevent repeat rune setting.
    /// </summary>
    public void TogglePreventRepeatRune()
    {
        preventRepeatRune = !preventRepeatRune;
    }
}
