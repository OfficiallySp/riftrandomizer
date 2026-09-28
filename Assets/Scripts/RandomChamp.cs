using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles random champion selection from Riot Games DataDragon API.
/// </summary>
public class RandomChamp : MonoBehaviour
{
    [Header("UI References")]
    public Text displayText;
    public Image displayImage;
    public Animator notif;

    private Dictionary<string, ChampionData> _championData;
    private DataDragonService _dataDragonService;
    private Notification _notificationSystem;
    private GameConfig _config;

    // Champion data classes
    [System.Serializable]
    public class ChampionDataWrapper
    {
        public string type;
        public string format;
        public string version;
        public Dictionary<string, ChampionData> data;
    }

    [System.Serializable]
    public class ChampionData
    {
        public string version;
        public string id;
        public string key;
        public string name;
        public string title;
        public string blurb;
        public ChampionImage image;
    }

    [System.Serializable]
    public class ChampionImage
    {
        public string full;
        public string sprite;
        public string group;
        public int x;
        public int y;
        public int w;
        public int h;
    }

    private void Start()
    {
        _dataDragonService = DataDragonService.Instance;
        _notificationSystem = Notification.Instance;
        _config = Resources.Load<GameConfig>("GameConfig");
    }

    /// <summary>
    /// Picks and displays a random champion.
    /// </summary>
    public void PickChamp()
    {
        if (_dataDragonService == null)
        {
            Debug.LogError("RandomChamp: DataDragonService is not available.");
            return;
        }

        if (_championData == null || _championData.Count == 0)
        {
            StartCoroutine(LoadChampionsFromJson());
        }
        else
        {
            ShowRandomChampion();
        }
    }

    private IEnumerator LoadChampionsFromJson()
    {
        if (_dataDragonService == null)
        {
            ShowError("DataDragon service not available.");
            yield break;
        }

        string url = _dataDragonService.GetChampionDataUrl();
        yield return StartCoroutine(_dataDragonService.FetchJsonData<ChampionDataWrapper>(
            url,
            onSuccess: (data) =>
            {
                if (data?.data != null && data.data.Count > 0)
                {
                    _championData = data.data;
                    ShowRandomChampion();
                }
                else
                {
                    ShowError("No champion data received from API.");
                }
            },
            onError: (error) =>
            {
                ShowError($"Can't Contact Riot API: {error}");
            }
        ));
    }

    /// <summary>
    /// Shows a random champion and loads its image.
    /// </summary>
    public void ShowRandomChampion()
    {
        if (_championData == null || _championData.Count == 0)
        {
            ShowError("Champion data not loaded.");
            return;
        }

        ChampionData randomChampionData = GetRandomChampion();
        if (randomChampionData == null)
        {
            ShowError("Failed to get random champion.");
            return;
        }

        if (displayText != null)
        {
            displayText.text = randomChampionData.name;
        }

        if (displayImage != null && randomChampionData.image != null)
        {
            string imageUrl = _config != null 
                ? _config.GetChampionImageUrl(randomChampionData.image.full)
                : $"https://ddragon.leagueoflegends.com/cdn/13.17.1/img/champion/{randomChampionData.image.full}";
            
            StartCoroutine(LoadImageFromUrl(imageUrl));
        }
    }

    private ChampionData GetRandomChampion()
    {
        if (_championData == null || _championData.Count == 0)
        {
            return null;
        }

        List<string> championKeys = new List<string>(_championData.Keys);
        int randomIndex = Random.Range(0, championKeys.Count);
        return _championData[championKeys[randomIndex]];
    }

    private IEnumerator LoadImageFromUrl(string imageUrl)
    {
        if (_dataDragonService == null || displayImage == null)
        {
            yield break;
        }

        yield return StartCoroutine(_dataDragonService.FetchImage(
            imageUrl,
            onSuccess: (texture) =>
            {
                if (texture != null && displayImage != null)
                {
                    displayImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            },
            onError: (error) =>
            {
                ShowError($"Unable to load champion image: {error}");
            }
        ));
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
            Debug.LogError($"RandomChamp: {message}");
        }
    }
}
