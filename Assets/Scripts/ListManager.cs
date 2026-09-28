using UnityEngine;

/// <summary>
/// Manages the sibling index of a list element for randomization purposes with continuous shuffling animation.
/// </summary>
public class ListManager : MonoBehaviour
{
    private const int MAX_SIBLING_INDEX = 9;
    private int _indexNumber;

    private void Start()
    {
        _indexNumber = MAX_SIBLING_INDEX;
    }

    private void Update()
    {
        transform.SetSiblingIndex(_indexNumber);

        if (_indexNumber <= transform.GetSiblingIndex())
        {
            _indexNumber = Random.Range(0, MAX_SIBLING_INDEX);
        }
    }
}
