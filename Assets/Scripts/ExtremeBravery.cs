using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ExtremeBravery : MonoBehaviour
{
    [Header("UI References")]
    public Animator notif;
    public Image displayImage;
    public Text displayText;
    public Text[] abilitySequenceTexts;
    public Image[] summonerImages;
    public Image[] itemImages;

    private Dictionary<string, ChampionData> _championData;
    private Dictionary<string, SummonerData> _summonerData;
    private Dictionary<string, ItemData> _itemData;

    private HashSet<string> _summonerSpellBlacklist = new HashSet<string> { "SomeInvalidID1", "SomeInvalidID2" };
    private HashSet<string> _itemBlacklist = new HashSet<string> { "SomeInvalidItemID1", "SomeInvalidItemID2" };
    private Notification _notificationSystem;
    private DataDragonService _dataDragonService;
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

    [System.Serializable]
    public class SummonerDataWrapper
    {
        public Dictionary<string, SummonerData> data;
    }

    [System.Serializable]
    public class SummonerData
    {
        public string id;
        public string name;
        public SummonerImage image;
    }

    [System.Serializable]
    public class SummonerImage
    {
        public string full;
    }

    [System.Serializable]
    public class ItemDataWrapper
    {
        public Dictionary<string, ItemData> data;
    }

    [System.Serializable]
    public class ItemData
    {
        public string id;
        public string name;
        public ItemImage image;
    }

    [System.Serializable]
    public class ItemImage
    {
        public string full;
    }

    private void Start()
    {
        _notificationSystem = Notification.Instance;
        _dataDragonService = DataDragonService.Instance;
        _config = Resources.Load<GameConfig>("GameConfig");
    }

    /// <summary>
    /// Picks a random champion, items, and summoner spells.
    /// </summary>
    public void PickChampAndItems()
    {
        if (_dataDragonService == null)
        {
            ShowError("DataDragon service not available.");
            return;
        }

        if (_championData == null || _summonerData == null || _itemData == null)
        {
            StartCoroutine(LoadDataFromJson());
        }
        else
        {
            ShowRandomData();
        }
    }

    private IEnumerator LoadDataFromJson()
    {
        if (_dataDragonService == null)
        {
            ShowError("DataDragon service not available.");
            yield break;
        }

        bool championsLoaded = false;
        bool summonersLoaded = false;
        bool itemsLoaded = false;

        // Load champions
        yield return StartCoroutine(_dataDragonService.FetchJsonData<ChampionDataWrapper>(
            _dataDragonService.GetChampionDataUrl(),
            onSuccess: (data) =>
            {
                if (data?.data != null && data.data.Count > 0)
                {
                    _championData = data.data;
                    championsLoaded = true;
                }
                else
                {
                    ShowError("No champion data received.");
                }
            },
            onError: (error) =>
            {
                ShowError($"Cannot fetch Champions: {error}");
            }
        ));

        if (!championsLoaded) yield break;

        // Load summoner spells
        yield return StartCoroutine(_dataDragonService.FetchJsonData<SummonerDataWrapper>(
            _dataDragonService.GetSummonerSpellDataUrl(),
            onSuccess: (data) =>
            {
                if (data?.data != null && data.data.Count > 0)
                {
                    _summonerData = data.data;
                    summonersLoaded = true;
                }
                else
                {
                    ShowError("No summoner spell data received.");
                }
            },
            onError: (error) =>
            {
                ShowError($"Cannot fetch Summoner Spells: {error}");
            }
        ));

        if (!summonersLoaded) yield break;

        // Load items
        yield return StartCoroutine(_dataDragonService.FetchJsonData<ItemDataWrapper>(
            _dataDragonService.GetItemDataUrl(),
            onSuccess: (data) =>
            {
                if (data?.data != null && data.data.Count > 0)
                {
                    _itemData = data.data;
                    itemsLoaded = true;
                }
                else
                {
                    ShowError("No item data received.");
                }
            },
            onError: (error) =>
            {
                ShowError($"Cannot fetch Items: {error}");
            }
        ));

        if (championsLoaded && summonersLoaded && itemsLoaded)
        {
            ShowRandomData();
        }
    }

    public void ShowRandomData()
    {
        if (_championData == null || _summonerData == null || _itemData == null)
        {
            ShowError("Data not loaded. Please try again.");
            return;
        }

        // Display random champion
        ChampionData randomChampion = GetRandomChampion();
        if (randomChampion == null)
        {
            ShowError("Failed to get random champion.");
            return;
        }

        if (displayText != null)
        {
            displayText.text = randomChampion.name;
        }

        if (displayImage != null && randomChampion.image != null)
        {
            string championImageUrl = _config != null
                ? _config.GetChampionImageUrl(randomChampion.image.full)
                : $"https://ddragon.leagueoflegends.com/cdn/13.17.1/img/champion/{randomChampion.image.full}";
            StartCoroutine(LoadImageFromUrl(championImageUrl, displayImage));
        }

        // Display random summoner spells
        List<SummonerData> randomSummoners = GetRandomSummonerSpells(2);
        if (randomSummoners != null && summonerImages != null)
        {
            for (int i = 0; i < Mathf.Min(2, summonerImages.Length) && i < randomSummoners.Count; i++)
            {
                if (summonerImages[i] != null && randomSummoners[i]?.image != null)
                {
                    string summonerImageUrl = _config != null
                        ? _config.GetSummonerSpellImageUrl(randomSummoners[i].image.full)
                        : $"https://ddragon.leagueoflegends.com/cdn/13.17.1/img/spell/{randomSummoners[i].image.full}";
                    StartCoroutine(LoadImageFromUrl(summonerImageUrl, summonerImages[i]));
                }
            }
        }

        // Display random items
        List<ItemData> randomItems = GetRandomItems(6);
        if (randomItems != null && itemImages != null)
        {
            for (int i = 0; i < Mathf.Min(6, itemImages.Length) && i < randomItems.Count; i++)
            {
                if (itemImages[i] != null && randomItems[i]?.image != null)
                {
                    string itemImageUrl = _config != null
                        ? _config.GetItemImageUrl(randomItems[i].image.full)
                        : $"https://ddragon.leagueoflegends.com/cdn/13.17.1/img/item/{randomItems[i].image.full}";
                    StartCoroutine(LoadImageFromUrl(itemImageUrl, itemImages[i]));
                }
            }
        }

        // Generate a random ability leveling sequence
        string abilitySequence = GenerateAbilitySequence();
        Debug.Log("Generated Ability Sequence: " + abilitySequence);

        // Set each ability sequence Text component
        if (abilitySequenceTexts != null)
        {
            for (int i = 0; i < abilitySequence.Length && i < abilitySequenceTexts.Length; i++)
            {
                if (abilitySequenceTexts[i] != null)
                {
                    abilitySequenceTexts[i].text = abilitySequence[i].ToString();
                }
            }
        }
    }
    private string GenerateAbilitySequence()
    {
        char[] abilities = new char[18];

        // R can be leveled up at champion levels 6, 11, and 16
        abilities[5] = 'R';
        abilities[10] = 'R';
        abilities[15] = 'R';

        // For the remaining slots, randomly choose between Q, W, and E
        char[] options = { 'Q', 'W', 'E' };
        for (int i = 0; i < 18; i++)
        {
            if (abilities[i] == 0) // 0 is the default value for char, so this checks if it's unset
            {
                abilities[i] = options[Random.Range(0, options.Length)];
            }
        }

        // Create a string with spaces between the characters and a newline after 9 characters
        StringBuilder formattedSequence = new StringBuilder();
        for (int i = 0; i < 18; i++)
        {
            formattedSequence.Append(abilities[i]);
        }

        return formattedSequence.ToString();
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

    private List<SummonerData> GetRandomSummonerSpells(int count)
    {
        if (_summonerData == null || _summonerData.Count == 0)
        {
            return null;
        }

        List<string> summonerKeys = new List<string>(_summonerData.Keys);
        // Filter out blacklisted spells
        summonerKeys.RemoveAll(key => _summonerSpellBlacklist.Contains(key));

        List<SummonerData> randomSummoners = new List<SummonerData>();

        for (int i = 0; i < count && summonerKeys.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, summonerKeys.Count);
            if (_summonerData.ContainsKey(summonerKeys[randomIndex]))
            {
                randomSummoners.Add(_summonerData[summonerKeys[randomIndex]]);
                summonerKeys.RemoveAt(randomIndex);  // Remove to avoid duplicates
            }
        }

        return randomSummoners;
    }

    private List<ItemData> GetRandomItems(int count)
    {
        if (_itemData == null || _itemData.Count == 0)
        {
            return null;
        }

        List<string> itemKeys = new List<string>(_itemData.Keys);
        // Filter out blacklisted items
        itemKeys.RemoveAll(key => _itemBlacklist.Contains(key));

        List<ItemData> randomItems = new List<ItemData>();

        for (int i = 0; i < count && itemKeys.Count > 0; i++)
        {
            int randomIndex = Random.Range(0, itemKeys.Count);
            if (_itemData.ContainsKey(itemKeys[randomIndex]))
            {
                randomItems.Add(_itemData[itemKeys[randomIndex]]);
                itemKeys.RemoveAt(randomIndex);  // Remove to avoid duplicates
            }
        }

        return randomItems;
    }


    private IEnumerator LoadImageFromUrl(string imageUrl, Image imageComponent)
    {
        if (_dataDragonService == null || imageComponent == null)
        {
            yield break;
        }

        yield return StartCoroutine(_dataDragonService.FetchImage(
            imageUrl,
            onSuccess: (texture) =>
            {
                if (texture != null && imageComponent != null)
                {
                    imageComponent.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                }
            },
            onError: (error) =>
            {
                ShowError($"Unable to load image: {error}");
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
            Debug.LogError($"ExtremeBravery: {message}");
        }
    }
}