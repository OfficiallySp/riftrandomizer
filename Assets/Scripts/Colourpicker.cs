using UnityEngine;

/// <summary>
/// Handles random colour selection with optional repeat prevention.
/// </summary>
public class Colourpicker : MonoBehaviour
{
    /// <summary>
    /// Available colour choices.
    /// </summary>
    public enum ColourChoice
    {
        Red,
        Blue,
        Yellow,
        Green,
    }

    [Header("Configuration")]
    public bool preventRepeatColour = true;

    [Header("Colour Objects")]
    public GameObject[] colourObjects;

    /// <summary>
    /// Gets the currently selected colour.
    /// </summary>
    public ColourChoice SelectedColour { get; private set; }

    private ColourChoice _previousColour;
    private int _colourCount;

    private void Start()
    {
        _colourCount = System.Enum.GetValues(typeof(ColourChoice)).Length;
    }

    /// <summary>
    /// Selects a random colour and updates the display.
    /// </summary>
    public void ColourSelect()
    {
        ColourChoice newColour;
        
        if (preventRepeatColour && _colourCount > 1)
        {
            do
            {
                newColour = (ColourChoice)Random.Range(0, _colourCount);
            } while (newColour == SelectedColour);
        }
        else
        {
            newColour = (ColourChoice)Random.Range(0, _colourCount);
        }

        _previousColour = SelectedColour;
        SelectedColour = newColour;
        UpdateColourObjects();
    }

    private void UpdateColourObjects()
    {
        if (colourObjects == null) return;

        for (int i = 0; i < colourObjects.Length; i++)
        {
            if (colourObjects[i] != null)
            {
                colourObjects[i].SetActive(i == (int)SelectedColour);
            }
        }
    }

    /// <summary>
    /// Toggles the prevent repeat colour setting.
    /// </summary>
    public void TogglePreventRepeatColour()
    {
        preventRepeatColour = !preventRepeatColour;
    }
}