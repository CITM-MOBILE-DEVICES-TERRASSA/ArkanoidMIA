using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;

    private void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        UpdateHighScoreDisplay();
    }

    private void UpdateHighScoreDisplay()
    {
        int highScore = GameManager.Instance.HighScore;
        highScoreText.text = $"High Score: {highScore}";
    }

    private void OnEnable()
    {
        GameManager.Instance.LoadGame();
        UpdateHighScoreDisplay();
    }
}
