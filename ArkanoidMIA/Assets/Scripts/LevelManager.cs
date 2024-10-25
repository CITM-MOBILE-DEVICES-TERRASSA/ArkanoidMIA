using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int totalBricks;

    void Start()
    {
        totalBricks = GameObject.FindGameObjectsWithTag("Brick").Length;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateBrickCount(totalBricks);
        }
    }

    public void BrickDestroyed()
    {
        totalBricks--;
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UpdateBrickCount(totalBricks);
        }
        
        if (totalBricks <= 0)
        {
            Debug.Log("Level Complete!");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AdvanceToNextLevel();
            }
        }
    }
}