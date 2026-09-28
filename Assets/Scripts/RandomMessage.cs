using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a random message from a predefined array.
/// </summary>
public class RandomMessage : MonoBehaviour
{
    [Header("Messages")]
    public string[] messages;

    [Header("UI References")]
    public Text messageDisplay;

    /// <summary>
    /// Displays a random message from the messages array.
    /// </summary>
    public void RandomMessageDisplay()
    {
        if (messages == null || messages.Length == 0)
        {
            Debug.LogWarning("RandomMessage: No messages available.");
            if (messageDisplay != null)
            {
                messageDisplay.text = "";
            }
            return;
        }

        if (messageDisplay == null)
        {
            Debug.LogWarning("RandomMessage: messageDisplay is not assigned.");
            return;
        }

        // Select a random message
        int randomIndex = Random.Range(0, messages.Length);

        // Display the random message
        messageDisplay.text = messages[randomIndex];
        Debug.Log($"Random message displayed: {messages[randomIndex]}");
    }
}
