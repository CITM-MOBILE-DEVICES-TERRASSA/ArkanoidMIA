using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int MaxLives = 3;
    
    // Datos del juego
    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }
    public int CurrentLives { get; private set; }
    public string CurrentLevel { get; private set; }
    public int RemainingBricks { get; private set; }

    // Evento para notificar cambios en las vidas
    public delegate void LivesChangedHandler(int newLives);
    public event LivesChangedHandler OnLivesChanged;

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

    public void AddLife()
    {
        if (CurrentLives < MaxLives)
        {
            CurrentLives++;
            OnLivesChanged?.Invoke(CurrentLives);
            SaveGame();
            Debug.Log($"Vida añadida. Vidas actuales: {CurrentLives}");
        }
    }

    public void LoseLife()
    {
        if (CurrentLives > 0)
        {
            CurrentLives--;
            OnLivesChanged?.Invoke(CurrentLives);
            SaveGame();
        }
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
        
        // Notificar del cambio de vidas cuando se carga el juego
        OnLivesChanged?.Invoke(CurrentLives);
        
        Debug.Log($"Game Loaded - Score: {CurrentScore}, Lives: {CurrentLives}, Level: {CurrentLevel}, Bricks: {RemainingBricks}");
    }

    public void UpdateScore(int newScore)
    {
        CurrentScore = newScore;
        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
        SaveGame();
    }

    public void UpdateLives(int lives)
    {
        CurrentLives = lives;
        OnLivesChanged?.Invoke(CurrentLives);
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
        OnLivesChanged?.Invoke(CurrentLives);
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
        }
    }

    private string GetNextLevelName(string currentLevel)
    {
        switch (currentLevel)
        {
            case "SampleScene":
                return "Level2";
            case "Level2":
                return "SampleScene";
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