using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages game mode selection via dropdown and shows/hides corresponding UI elements.
/// </summary>
public class GamemodeTypes : MonoBehaviour
{
    [Header("UI References")]
    public Dropdown dropdown;
    public GameObject[] gameObjects;

    private void Start()
    {
        if (dropdown != null)
        {
            dropdown.onValueChanged.AddListener(ShowSelectedGameObject);
        }
        else
        {
            Debug.LogWarning("GamemodeTypes: Dropdown is not assigned.");
        }
    }

    private void ShowSelectedGameObject(int index)
    {
        if (gameObjects == null)
        {
            Debug.LogWarning("GamemodeTypes: gameObjects array is not assigned.");
            return;
        }

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] != null)
            {
                gameObjects[i].SetActive(i == index);
            }
        }
    }
}