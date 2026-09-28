using UnityEngine;

/// <summary>
/// Centralized configuration ScriptableObject for game settings, API versions, and URLs.
/// </summary>
[CreateAssetMenu(fileName = "GameConfig", menuName = "RiftRandomizer/Game Config")]
public class GameConfig : ScriptableObject
{
    [Header("API Configuration")]
    [Tooltip("DataDragon API version (e.g., '13.17.1'). Leave empty to fetch latest.")]
    public string dataDragonVersion = "13.17.1";

    [Tooltip("Base URL for DataDragon API")]
    public string dataDragonBaseUrl = "https://ddragon.leagueoflegends.com/cdn";

    [Tooltip("Base URL for custom lists API")]
    public string customListsBaseUrl = "";

    [Header("Performance Settings")]
    [Tooltip("Target frame rate for the application")]
    [Range(30, 120)]
    public int targetFrameRate = 40;

    [Header("Scene Names")]
    [Tooltip("Name of the main names scene")]
    public string namesSceneName = "names";

    [Tooltip("Name of the tourney scene")]
    public string tourneySceneName = "tourney";

    [Tooltip("Name of the ad scene")]
    public string adSceneName = "ad";

    [Header("Randomization Constants")]
    [Tooltip("Maximum sibling index for list managers")]
    public int maxListSiblingIndex = 9;

    [Tooltip("Maximum sibling index for lock managers")]
    public int maxLockSiblingIndex = 7;

    [Tooltip("Maximum sibling index for lane managers")]
    public int maxLaneSiblingIndex = 4;

    [Tooltip("Maximum number of runes")]
    public int maxRunes = 16;

    [Header("Notification Settings")]
    [Tooltip("URL to check for internet connectivity")]
    public string connectivityCheckUrl = "https://officiallysp.net";

    [Tooltip("Delay between retry attempts in seconds")]
    public float retryDelay = 2.0f;

    /// <summary>
    /// Gets the full DataDragon URL for a specific resource type.
    /// </summary>
    public string GetDataDragonUrl(string resourceType, string fileName)
    {
        return $"{dataDragonBaseUrl}/{dataDragonVersion}/{resourceType}/{fileName}";
    }

    /// <summary>
    /// Gets the DataDragon image URL for champions.
    /// </summary>
    public string GetChampionImageUrl(string imageFileName)
    {
        return $"{dataDragonBaseUrl}/{dataDragonVersion}/img/champion/{imageFileName}";
    }

    /// <summary>
    /// Gets the DataDragon image URL for summoner spells.
    /// </summary>
    public string GetSummonerSpellImageUrl(string imageFileName)
    {
        return $"{dataDragonBaseUrl}/{dataDragonVersion}/img/spell/{imageFileName}";
    }

    /// <summary>
    /// Gets the DataDragon image URL for items.
    /// </summary>
    public string GetItemImageUrl(string imageFileName)
    {
        return $"{dataDragonBaseUrl}/{dataDragonVersion}/img/item/{imageFileName}";
    }
}

