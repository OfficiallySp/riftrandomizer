using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles pasting names from clipboard into input fields.
/// </summary>
public class PasteNames : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("Maximum length for each name")]
    public int maxNameLength = 50;

    [Header("Input Fields")]
    public List<InputField> inputFields = new List<InputField>();

    /// <summary>
    /// Pastes names from clipboard into the input fields.
    /// </summary>
    public void PasteFromClipboard()
    {
        if (inputFields == null || inputFields.Count == 0)
        {
            Debug.LogWarning("PasteNames: No input fields assigned.");
            return;
        }

        string clipboardData = GUIUtility.systemCopyBuffer;
        if (string.IsNullOrEmpty(clipboardData))
        {
            Debug.LogWarning("PasteNames: Clipboard is empty.");
            return;
        }

        string[] names = clipboardData.Split('\n');

        for (int i = 0; i < names.Length && i < inputFields.Count; i++)
        {
            if (inputFields[i] != null)
            {
                string trimmedName = names[i].Trim();
                // Validate and limit length
                if (trimmedName.Length > maxNameLength)
                {
                    trimmedName = trimmedName.Substring(0, maxNameLength);
                }
                inputFields[i].text = trimmedName;
            }
        }
    }
}
