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
        
        // Calcular el offset inicial entre la pelota y el paddle
        offsetFromPaddle = transform.position - paddle.transform.position;
        
        // Inicialmente la pelota no se mueve
        rb.velocity = Vector2.zero;
        ResetBall();
    }

    private void Update()
    {
        if (!gameStarted)
        {
            // Mantener la pelota sobre el paddle
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
        if (!gameStarted) return; // Ignorar colisiones si el juego no ha comenzado

        // Incrementar la magnitud de la velocidad en cada rebote
        float currentSpeed = rb.velocity.magnitude;
        Vector2 currentDirection = rb.velocity.normalized;
        float newSpeed = currentSpeed * velocityIncreaseFactor;

        if (newSpeed > maxVelocity)
        {
            newSpeed = maxVelocity;
        }

        rb.velocity = currentDirection * newSpeed;

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
                    scoreTxt.text = score.ToString("00000");
                    brickCount--;
                    if (brickCount <= 0)
                    {
                        YouWinPanel.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        Destroy(gameObject);
    }
}