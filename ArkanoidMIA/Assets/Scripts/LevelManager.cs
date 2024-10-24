using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int totalBricks;

    void Start()
    {
        // Cuenta el número total de bricks en la escena
        totalBricks = GameObject.FindGameObjectsWithTag("Brick").Length;
    }

    public void BrickDestroyed()
    {
        totalBricks--;
        if (totalBricks <= 0)
        {
            // Cuando se destruyan todos los bricks, cargar el siguiente nivel
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "SampleScene")  // Si es el primer nivel
        {
            SceneManager.LoadScene("Level2");
        }
        else if (currentScene == "Level2")  // Si es el segundo nivel
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
