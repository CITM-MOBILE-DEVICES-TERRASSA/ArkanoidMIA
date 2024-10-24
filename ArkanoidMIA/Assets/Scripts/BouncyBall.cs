using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BouncyBall : MonoBehaviour
{
    public float minY = -5.5f;
    public float maxVelocity = 15f;
    private Rigidbody2D rb;

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
        brickCount = FindObjectOfType<LevelGenerator>().transform.childCount;
        rb.velocity = Vector2.down * 10f;
    }

    private void Update()
    {
        if(transform.position.y < minY)
        {
            if(lives <= 0)
            {
                GameOver();
            }
            else
            {
                transform.position = Vector3.zero;
                rb.velocity = Vector2.down * 10f;
                lives--;
                livesImage[lives].SetActive(false);
            }
        }

        if(rb.velocity.magnitude > maxVelocity)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
        }
    }

    private float velocityIncreaseFactor = 1.05f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Incrementar la magnitud de la velocidad en cada rebote
        float currentSpeed = rb.velocity.magnitude;
        Vector2 currentDirection = rb.velocity.normalized;
        float newSpeed = currentSpeed * velocityIncreaseFactor;

        if (newSpeed > maxVelocity)
        {
            newSpeed = maxVelocity;
        }

        rb.velocity = currentDirection * newSpeed;

        // Detectar si la colisión es con un ladrillo
        if (collision.gameObject.CompareTag("Brick"))
        {
            // Código para gestionar la destrucción del ladrillo
            Brick brick = collision.gameObject.GetComponent<Brick>();
            if (brick != null)
            {
                brick.TakeDamage(); // Aplicamos daño al ladrillo
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
