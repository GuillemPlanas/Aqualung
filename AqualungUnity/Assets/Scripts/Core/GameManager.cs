using System.Collections;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Gestor de la càrrega d'escenes al joc
 */
public class GameManager : MonoBehaviour
{
    public delegate void OnEventDelegate();

    public event OnEventDelegate OnLevelChange;


    [Header("Menus")] [SerializeField] private string mainMenu = "MainMenu";
    [SerializeField] private string gameOver = "GameOver";
    [SerializeField] private string victory = "Victory";

    [Header("Load Delays")] [SerializeField]
    private float sceneLoadDelay = 2f;

    [SerializeField] private float sceneDefeatDelay = 5f;
    [SerializeField] private float sceneVictoryDelay = 5f;


    [Header("Levels")] [SerializeField] private GameLevelsSO gameLevels;


    public static GameLevelsSO GameLevels
    {
        get { return _instance.gameLevels; }
    }


    private static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


    public static void LoadGame()
    {
        GameState.Instance.Reset();
        var level = Instance.gameLevels.Levels[0];

        LoadLevel(level.sceneName);
    }

    public static void LoadMainMenu()
    {
        LoadLevel(Instance.mainMenu);
    }

    public static void LoadGameOver()
    {
        AudioManager.Instance.StopTrack();
        Instance.StartCoroutine(Instance.DelayedLoadLevel(Instance.gameOver, Instance.sceneDefeatDelay));
    }

    public static void LoadVictory()
    {
        AudioManager.Instance.StopTrack();
        Instance.StartCoroutine(Instance.DelayedLoadLevel(Instance.victory, Instance.sceneVictoryDelay));
    }


    public static void LoadLevel(string sceneName)
    {
        Instance.StartCoroutine(Instance.DelayedLoadLevel(sceneName, Instance.sceneLoadDelay));
    }

    private IEnumerator DelayedLoadLevel(string sceneName, float delay)
    {
        if (OnLevelChange != null) OnLevelChange();
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
    
    public static void QuitGame()
    {
        Application.Quit();
    }
}