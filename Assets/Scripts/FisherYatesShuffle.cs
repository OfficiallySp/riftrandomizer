using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Implements Fisher-Yates shuffle algorithm to randomly reorder child transforms.
/// </summary>
public class FisherYatesShuffle : MonoBehaviour
{
    /// <summary>
    /// Shuffles the sibling indices of all child transforms using Fisher-Yates algorithm.
    /// </summary>
    public void ShuffleSiblings()
    {
        List<Transform> siblings = GetComponentsInChildren<Transform>().ToList();
        
        if (siblings.Count <= 1)
        {
            return; // Nothing to shuffle
        }

        // Remove the parent object from the list (first element is always the parent)
        siblings.RemoveAt(0);

        if (siblings.Count <= 1)
        {
            return; // Only one child, nothing to shuffle
        }

        // Fisher-Yates shuffle
        int n = siblings.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            
            // Swap elements
            Transform temp = siblings[n];
            siblings[n] = siblings[k];
            siblings[k] = temp;
        }

        // Apply the new order by setting sibling indices
        for (int i = 0; i < siblings.Count; i++)
        {
            if (siblings[i] != null)
            {
                siblings[i].SetSiblingIndex(i);
            }
        }
    }
}