using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles startup scene navigation.
/// </summary>
public class Startup : MonoBehaviour
{
    private GameConfig _config;

    private void Start()
    {
        _config = Resources.Load<GameConfig>("GameConfig");
    }

    /// <summary>
    /// Cancels update and loads the names scene.
    /// </summary>
    public void CancelUpdate()
    {
        string namesScene = _config != null ? _config.namesSceneName : "names";
        SceneManager.LoadScene(namesScene);
    }
}
