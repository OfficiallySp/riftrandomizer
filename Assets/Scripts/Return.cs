using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene navigation with escape key hold functionality.
/// </summary>
public class Return : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The minimum time the Escape key must be held down to trigger return")]
    public float holdTime = 1.0f;

    private GameConfig _config;
    private string _currentSceneName;
    private bool _escapePressed = false;
    private float _holdTimer = 0.0f;

    private void Start()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;
        _config = Resources.Load<GameConfig>("GameConfig");
    }

    private void LateUpdate()
    {
        string namesScene = _config != null ? _config.namesSceneName : "names";
        string tourneyScene = _config != null ? _config.tourneySceneName : "tourney";
        string adScene = _config != null ? _config.adSceneName : "ad";

        // Check if we're in a scene that supports escape to return
        if (_currentSceneName == tourneyScene || _currentSceneName == adScene)
        {
            HandleEscapeKey(namesScene);
        }
    }

    private void HandleEscapeKey(string targetScene)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _escapePressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            _escapePressed = false;
            _holdTimer = 0.0f;
        }

        if (_escapePressed)
        {
            _holdTimer += Time.deltaTime;

            if (_holdTimer >= holdTime)
            {
                SceneManager.LoadScene(targetScene);
                _escapePressed = false;
                _holdTimer = 0.0f;
            }
        }
        else
        {
            _holdTimer = 0.0f;
        }
    }

    /// <summary>
    /// Loads the tourney scene.
    /// </summary>
    public void LoadAlpha()
    {
        string tourneyScene = _config != null ? _config.tourneySceneName : "tourney";
        SceneManager.LoadScene(tourneyScene);
    }
}
