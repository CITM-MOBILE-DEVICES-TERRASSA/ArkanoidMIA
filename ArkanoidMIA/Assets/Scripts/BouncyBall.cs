using UnityEngine;
using TMPro;
using System.Collections;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    public float initialVelocityY = 10f;
    private Rigidbody2D rb;
    private PlayerMovement paddle;
    private bool gameStarted = false;
    private Vector3 offsetFromPaddle;
    private Coroutine launchCoroutine;

    private int score = 0;
    private int lives = 3;

    public TextMeshProUGUI scoreTxt;
    public GameObject[] livesImage;
    public GameObject gameOverPanel;
    public GameObject YouWinPanel;
    private int brickCount;
    private float autoLaunchDelay = 2f;
    private float velocityIncreaseFactor = 1.05f;
    public GameObject extraLifePowerUpPrefab;
    public float powerUpDropChance = 1.0f;

    // Nuevas variables para el audio
    public AudioClip impactSound; // Asigna el clip de sonido de impacto en el inspector
    private AudioSource audioSource; // Componente AudioSource

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        paddle = FindObjectOfType<PlayerMovement>();
        brickCount = FindObjectOfType<LevelGenerator>().transform.childCount;

        if (GameManager.Instance != null)
        {
            score = GameManager.Instance.CurrentScore;
            lives = GameManager.Instance.CurrentLives;
            scoreTxt.text = score.ToString("00000");
            GameManager.Instance.OnLivesChanged += HandleLivesChanged;
        }

        UpdateLivesDisplay();
        offsetFromPaddle = transform.position - paddle.transform.position;
        rb.velocity = Vector2.zero;
        ResetBall();

        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnLivesChanged -= HandleLivesChanged;
        }
    }

    private void HandleLivesChanged(int newLives)
    {
        lives = newLives;
        UpdateLivesDisplay();
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
        else if (transform.position.y < minY)
        {
            if (lives <= 0)
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

        if (rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    public void StartMoving()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            StopAutoLaunchCoroutine();
            rb.velocity = Vector2.up * initialVelocityY;
        }
    }

    private void ResetBall()
    {
        gameStarted = false;
        rb.velocity = Vector2.zero;
        paddle.ResetPosition(); 
        transform.position = paddle.transform.position + offsetFromPaddle;

        StartAutoLaunchCoroutine();
    }

    private void StartAutoLaunchCoroutine()
    {
        StopAutoLaunchCoroutine();
        launchCoroutine = StartCoroutine(AutoLaunchBallAfterDelay(autoLaunchDelay));
    }

    private void StopAutoLaunchCoroutine()
    {
        if (launchCoroutine != null)
        {
            StopCoroutine(launchCoroutine);
            launchCoroutine = null;
        }
    }

    private IEnumerator AutoLaunchBallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!gameStarted)
        {
            StartMovingWithRandomAngle();
        }
    }

    private void StartMovingWithRandomAngle()
    {
        gameStarted = true;
        float angle = Random.Range(-45f, 45f);
        rb.velocity = Quaternion.Euler(0, 0, angle) * Vector2.up * initialVelocityY;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameStarted) return;

        // Reproducir el sonido de impacto
        if (impactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactSound); // Reproducir el sonido de impacto
        }

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

                    if (Random.value < powerUpDropChance && GameManager.Instance.CurrentLives < GameManager.Instance.MaxLives)
                    {
                        Instantiate(extraLifePowerUpPrefab, brick.transform.position, Quaternion.identity);
                    }

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

    private void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.DeleteSavedGame();
        }

        Destroy(gameObject);
    }
}
