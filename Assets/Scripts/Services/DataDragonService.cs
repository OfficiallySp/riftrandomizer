using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

/// <summary>
/// Service for interacting with Riot Games DataDragon API with caching and error handling.
/// </summary>
public class DataDragonService : MonoBehaviour
{
    private static DataDragonService _instance;
    private GameConfig _config;

    // Cache storage
    private Dictionary<string, CachedData> _cache = new Dictionary<string, CachedData>();
    private const float CACHE_TTL_SECONDS = 300f; // 5 minutes

    private class CachedData
    {
        public string Data { get; set; }
        public float Timestamp { get; set; }
    }

    public static DataDragonService Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DataDragonService");
                _instance = go.AddComponent<DataDragonService>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Load config from Resources
        _config = Resources.Load<GameConfig>("GameConfig");
        if (_config == null)
        {
            Debug.LogWarning("DataDragonService: GameConfig not found in Resources. Using defaults.");
            _config = ScriptableObject.CreateInstance<GameConfig>();
        }
    }

    /// <summary>
    /// Fetches JSON data from a URL with caching and retry logic.
    /// </summary>
    public IEnumerator FetchJsonData<T>(string url, System.Action<T> onSuccess, System.Action<string> onError, int maxRetries = 3)
    {
        // Check cache first
        if (_cache.ContainsKey(url))
        {
            CachedData cached = _cache[url];
            if (Time.time - cached.Timestamp < CACHE_TTL_SECONDS)
            {
                try
                {
                    T data = JsonConvert.DeserializeObject<T>(cached.Data);
                    onSuccess?.Invoke(data);
                    yield break;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"DataDragonService: Failed to deserialize cached data: {ex.Message}");
                    _cache.Remove(url);
                }
            }
            else
            {
                _cache.Remove(url);
            }
        }

        // Fetch from API with retry logic
        int retryCount = 0;
        while (retryCount < maxRetries)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    // Cache the response
                    _cache[url] = new CachedData
                    {
                        Data = www.downloadHandler.text,
                        Timestamp = Time.time
                    };

                    try
                    {
                        T data = JsonConvert.DeserializeObject<T>(www.downloadHandler.text);
                        onSuccess?.Invoke(data);
                    }
                    catch (Exception ex)
                    {
                        onError?.Invoke($"Failed to parse JSON: {ex.Message}");
                    }
                    yield break;
                }
                else
                {
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        yield return new WaitForSeconds(1f * retryCount); // Exponential backoff
                    }
                    else
                    {
                        onError?.Invoke($"{www.error} (Attempted {maxRetries} times)");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Fetches an image from a URL.
    /// </summary>
    public IEnumerator FetchImage(string url, System.Action<Texture2D> onSuccess, System.Action<string> onError)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                onSuccess?.Invoke(texture);
            }
            else
            {
                onError?.Invoke(www.error);
            }
        }
    }

    /// <summary>
    /// Gets the champion data JSON URL.
    /// </summary>
    public string GetChampionDataUrl(string language = "en_US")
    {
        return _config.GetDataDragonUrl("data", $"{language}/champion.json");
    }

    /// <summary>
    /// Gets the summoner spell data JSON URL.
    /// </summary>
    public string GetSummonerSpellDataUrl(string language = "en_US")
    {
        return _config.GetDataDragonUrl("data", $"{language}/summoner.json");
    }

    /// <summary>
    /// Gets the item data JSON URL.
    /// </summary>
    public string GetItemDataUrl(string language = "en_US")
    {
        return _config.GetDataDragonUrl("data", $"{language}/item.json");
    }

    /// <summary>
    /// Clears the cache.
    /// </summary>
    public void ClearCache()
    {
        _cache.Clear();
    }

    /// <summary>
    /// Clears expired cache entries.
    /// </summary>
    public void ClearExpiredCache()
    {
        List<string> keysToRemove = new List<string>();
        foreach (var kvp in _cache)
        {
            if (Time.time - kvp.Value.Timestamp >= CACHE_TTL_SECONDS)
            {
                keysToRemove.Add(kvp.Key);
            }
        }

        foreach (string key in keysToRemove)
        {
            _cache.Remove(key);
        }
    }
}

