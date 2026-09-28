using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles loading and displaying random lists from a JSON API.
/// </summary>
public class RandomList : MonoBehaviour
{
    [Header("Configuration")]
    public string jsonFileUrl;

    [Header("UI References")]
    public Text listNameText;
    public Text listIdText;
    public Text peopleText;
    public Button moreInfoButton;
    public Animator notif;

    private List<ListData> _listData;
    private Notification _notificationSystem;
    private DataDragonService _dataDragonService;

    [System.Serializable]
    public class ListDataWrapper
    {
        public List<ListData> lists;
    }

    [System.Serializable]
    public class ListData
    {
        public int id;
        public string name;
        public List<string> people;
        public string moreInfoUrl;
    }

    private void Start()
    {
        _notificationSystem = Notification.Instance;
        _dataDragonService = DataDragonService.Instance;

        if (!string.IsNullOrEmpty(jsonFileUrl))
        {
            StartCoroutine(LoadListsFromJson());
        }
        else
        {
            Debug.LogWarning("RandomList: jsonFileUrl is not set.");
        }
    }

    /// <summary>
    /// Refreshes the list data from the API.
    /// </summary>
    public void Refresh()
    {
        if (!string.IsNullOrEmpty(jsonFileUrl))
        {
            StartCoroutine(LoadListsFromJson());
        }
    }

    private IEnumerator LoadListsFromJson()
    {
        if (string.IsNullOrEmpty(jsonFileUrl))
        {
            ShowError("List URL is not configured.");
            yield break;
        }

        if (_dataDragonService != null)
        {
            yield return StartCoroutine(_dataDragonService.FetchJsonData<ListDataWrapper>(
                jsonFileUrl,
                onSuccess: (data) =>
                {
                    if (data?.lists != null && data.lists.Count > 0)
                    {
                        _listData = data.lists;
                    }
                    else
                    {
                        ShowError("No list data received.");
                    }
                },
                onError: (error) =>
                {
                    ShowError($"List cannot be fetched: {error}");
                }
            ));
        }
        else
        {
            // Fallback to direct UnityWebRequest if service not available
            using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(jsonFileUrl))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
                {
                    try
                    {
                        ListDataWrapper listDataWrapper = Newtonsoft.Json.JsonConvert.DeserializeObject<ListDataWrapper>(www.downloadHandler.text);
                        if (listDataWrapper?.lists != null && listDataWrapper.lists.Count > 0)
                        {
                            _listData = listDataWrapper.lists;
                        }
                        else
                        {
                            ShowError("No list data received.");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        ShowError($"Failed to parse list data: {ex.Message}");
                    }
                }
                else
                {
                    ShowError($"List cannot be fetched: {www.error}");
                }
            }
        }
    }

    /// <summary>
    /// Picks and displays a random list.
    /// </summary>
    public void PickList()
    {
        if (_listData == null || _listData.Count == 0)
        {
            ShowError("List data not loaded. Please refresh.");
            return;
        }

        ShowRandomList();
    }

    public void ShowRandomList()
    {
        if (_listData == null || _listData.Count == 0)
        {
            ShowError("No list data available.");
            return;
        }

        ListData randomListData = GetRandomList();
        if (randomListData == null)
        {
            ShowError("Failed to get random list.");
            return;
        }

        if (listNameText != null)
        {
            listNameText.text = randomListData.name;
        }

        if (listIdText != null)
        {
            listIdText.text = $"{randomListData.id}/{_listData.Count}";
        }

        if (peopleText != null && randomListData.people != null)
        {
            peopleText.text = string.Join("\n", randomListData.people);
        }

        if (moreInfoButton != null && !string.IsNullOrEmpty(randomListData.moreInfoUrl))
        {
            moreInfoButton.onClick.RemoveAllListeners();
            moreInfoButton.onClick.AddListener(() => Application.OpenURL(randomListData.moreInfoUrl));
        }
    }

    private ListData GetRandomList()
    {
        if (_listData == null || _listData.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, _listData.Count);
        return _listData[randomIndex];
    }

    private void ShowError(string message)
    {
        if (notif != null)
        {
            notif.SetTrigger("Play");
        }

        if (_notificationSystem != null)
        {
            _notificationSystem.ShowPopup(message, _notificationSystem.failureClip);
        }
        else
        {
            Debug.LogError($"RandomList: {message}");
        }
    }
}
