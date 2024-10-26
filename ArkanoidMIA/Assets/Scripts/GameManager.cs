using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // Datos del juego
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    public int CurrentLives { get; private set; }
    public string CurrentLevel { get; private set; }
    public int RemainingBricks { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        LoadGame();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("CurrentScore", CurrentScore);
        PlayerPrefs.SetInt("HighScore", HighScore);
        PlayerPrefs.SetInt("Lives", CurrentLives);
        PlayerPrefs.SetString("CurrentLevel", CurrentLevel);
        PlayerPrefs.SetInt("RemainingBricks", RemainingBricks);
        PlayerPrefs.Save();
        
        Debug.Log($"Game Saved - Score: {CurrentScore}, Lives: {CurrentLives}, Level: {CurrentLevel}, Bricks: {RemainingBricks}");
    }

    public void LoadGame()
    {
        CurrentScore = PlayerPrefs.GetInt("CurrentScore", 0);
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        CurrentLives = PlayerPrefs.GetInt("Lives", 3);
        CurrentLevel = PlayerPrefs.GetString("CurrentLevel", "SampleScene");
        RemainingBricks = PlayerPrefs.GetInt("RemainingBricks", 0);
        
        Debug.Log($"Game Loaded - Score: {CurrentScore}, Lives: {CurrentLives}, Level: {CurrentLevel}, Bricks: {RemainingBricks}");
    }

    public void UpdateScore(int newScore)
    {
        CurrentScore = newScore;
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore); // Guarda el nuevo High Score en PlayerPrefs
        }
        SaveGame();
    }

    public void UpdateLives(int lives)
    {
        CurrentLives = lives;
        SaveGame();
    }

    public void UpdateBrickCount(int remainingBricks)
    {
        RemainingBricks = remainingBricks;
        SaveGame();
    }

    public void StartNewGame()
    {
        CurrentScore = 0;
        CurrentLives = 3;
        CurrentLevel = "SampleScene";
        SaveGame();
        SceneManager.LoadScene(CurrentLevel);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            LoadGame();
            SceneManager.LoadScene(CurrentLevel);
        }
        else
        {
            StartNewGame();
        }
    }

    public void AdvanceToNextLevel()
    {
        string nextLevel = GetNextLevelName(CurrentLevel);
        if (!string.IsNullOrEmpty(nextLevel))
        {
            CurrentLevel = nextLevel;
            SaveGame();
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            Debug.Log("¡Juego Completado!");
            // Aquí podrías mostrar una pantalla de victoria final
        }
    }

    private string GetNextLevelName(string currentLevel)
    {
        switch (currentLevel)
        {
            case "SampleScene":
                return "Level2";
            case "Level2":
                return "SampleScene"; // Regresa al primer nivel en lugar de ir a un tercer nivel
            default:
                return "SampleScene";
        }
    }


    public void DeleteSavedGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}