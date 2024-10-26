using UnityEngine;

public class ExtraLifePowerUp : MonoBehaviour
{
    public float fallSpeed = 2f;
    private Rigidbody2D rb;
    private bool hasBeenCollected = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        rb.isKinematic = false;
        rb.velocity = new Vector2(0, -fallSpeed);
    }

    private void Update()
    {
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasBeenCollected && collision.gameObject.CompareTag("Paddle"))
        {
            hasBeenCollected = true;
            GameManager.Instance.AddLife();
            Destroy(gameObject);
        }
    }
}