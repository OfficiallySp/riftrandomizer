using UnityEngine;

public class ListLockManager : MonoBehaviour
{
    private int m_IndexNumber;

    private void Start()
    {
        m_IndexNumber = 7;
    }

    private void Update()
    {
        transform.SetSiblingIndex(m_IndexNumber);

        if (m_IndexNumber <= transform.GetSiblingIndex())
        {
            m_IndexNumber = Random.Range(0, 7);
        }
    }
}