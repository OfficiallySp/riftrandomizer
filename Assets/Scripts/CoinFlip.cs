using UnityEngine;

/// <summary>
/// Handles coin flip functionality with heads/tails display.
/// </summary>
public class CoinFlip : MonoBehaviour
{
    public GameObject Heads;
    public GameObject Tails;

    private const int HEADS_VALUE = 0;
    private const int TAILS_VALUE = 1;

    /// <summary>
    /// Flips the coin and displays the result.
    /// </summary>
    public void FlipCoin()
    {
        if (Heads == null || Tails == null)
        {
            Debug.LogWarning("CoinFlip: Heads or Tails GameObject is not assigned.");
            return;
        }

        int coinState = Random.Range(0, 2);
        bool isHeads = coinState == HEADS_VALUE;

        Heads.SetActive(isHeads);
        Tails.SetActive(!isHeads);
    }
}
