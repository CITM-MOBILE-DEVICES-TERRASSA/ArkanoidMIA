using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public float initialVelocityY = 10f; // Velocidad inicial vertical
    private Rigidbody2D rb;
    private PlayerMovement paddle;
    private bool gameStarted = false;
    private Vector3 offsetFromPaddle;

    private int score = 0;
    private int lives = 3;

    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject YouWinPanel;
    private int brickCount;

     private void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        paddle = FindObjectOfType<PlayerMovement>();
        brickCount = FindObjectOfType<LevelGenerator>().transform.childCount;
        
        // Cargar datos guardados si existe el GameManager
        if (GameManager.Instance != null)
        {
            score = GameManager.Instance.CurrentScore;
            lives = GameManager.Instance.CurrentLives;
            scoreTxt.text = score.ToString("00000");
        }
        
        // Actualizar visualización de vidas
        UpdateLivesDisplay();
        
        offsetFromPaddle = transform.position - paddle.transform.position;
        rb.velocity = Vector2.zero;
        ResetBall();
    }

        private void UpdateLivesDisplay()
    {
        for (int i = 0; i < livesImage.Length; i++)
        {
            livesImage[i].SetActive(i < lives);
        }
    }

        private void Update()
    {
        if (!gameStarted)
        {
            transform.position = paddle.transform.position + offsetFromPaddle;
        }
        else if(transform.position.y < minY)
        {
            if(lives <= 0)
            {
                GameOver();
            }
            else
            {
                lives--;
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.UpdateLives(lives);
                }
                livesImage[lives].SetActive(false);
                ResetBall();
            }
        }

        if(rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    public void StartMoving()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            rb.velocity = Vector2.up * initialVelocityY;
        }
    }

    private void ResetBall()
    {
        gameStarted = false;
        rb.velocity = Vector2.zero;
        paddle.ResetPosition(); // Llamamos al nuevo método en PlayerMovement
        transform.position = paddle.transform.position + offsetFromPaddle;
    }

    private float velocityIncreaseFactor = 1.05f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameStarted) return;

        // Aumentar la velocidad de la bola al chocar
        rb.velocity *= velocityIncreaseFactor;

        if (collision.gameObject.CompareTag("Brick"))
        {
            Brick brick = collision.gameObject.GetComponent<Brick>();
            if (brick != null)
            {
                brick.TakeDamage();
                if (brick.IsDestroyed())
                {
                    Destroy(collision.gameObject);
                    score += 10;
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.UpdateScore(score);
                    }
                    scoreTxt.text = score.ToString("00000");
                    brickCount--;
                    if (brickCount <= 0)
                    {
                        if (GameManager.Instance != null)
                        {
                            GameManager.Instance.AdvanceToNextLevel();
                        }
                        else
                        {
                            YouWinPanel.SetActive(true);
                            Time.timeScale = 0;
                        }
                    }
                }
            }
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        
        // Opcional: borrar el juego guardado cuando pierdes
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DeleteSavedGame();
        }
        
        Destroy(gameObject);
    }
}