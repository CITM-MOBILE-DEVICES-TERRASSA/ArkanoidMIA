using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float maxX = 17.5f;
    public Slider movementSlider;
    private BouncyBall ball;
    private bool hasMoved = false;
    private float initialSliderValue = 0.5f; // Valor central del slider

    private bool isAutoMode = false; // Modo automático

    void Start()
    {
        ball = FindObjectOfType<BouncyBall>();
        if (movementSlider != null)
        {
            movementSlider.onValueChanged.AddListener(OnSliderValueChanged);
            // Establecer el slider en el centro al inicio
            movementSlider.value = initialSliderValue;
        }
    }

    void Update()
    {
        if (isAutoMode && ball != null)
        {
            // Modo automático: mover el paddle a la posición X de la pelota
            float targetX = Mathf.Clamp(ball.transform.position.x, -maxX, maxX);
            Vector3 newPosition = new Vector3(targetX, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
        }

        // Cambiar entre modo manual y automático al presionar la tecla "A"
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleAutoMode(!isAutoMode);
        }

        // Manejar controles táctiles
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        // Verifica si hay toques en la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                // Obtiene la posición del toque en la pantalla
                Vector2 touchPosition = touch.position;

                // Convierte la posición del toque a la posición del mundo
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, Camera.main.nearClipPlane));

                // Mueve el paddle a la posición del toque, limitando a los bordes
                float posX = Mathf.Clamp(worldPosition.x, -maxX, maxX);
                Vector3 newPosition = new Vector3(posX, transform.position.y, transform.position.z);
                transform.position = newPosition;

                // Inicia el movimiento de la pelota si es la primera vez que se mueve
                if (!hasMoved)
                {
                    hasMoved = true;
                    ball.StartMoving();
                }
            }
        }
    }

    void OnSliderValueChanged(float value)
    {
        if (isAutoMode) return; // No permitir el control manual si está en modo automático

        if (!hasMoved && value != initialSliderValue)
        {
            hasMoved = true;
            ball.StartMoving(); // Iniciar el movimiento de la pelota
        }

        // Mover el paddle en modo manual
        float posX = Mathf.Lerp(-maxX, maxX, value);
        Vector3 newPosition = new Vector3(posX, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }

    public void ToggleAutoMode(bool auto)
    {
        isAutoMode = auto;
        if (isAutoMode)
        {
            hasMoved = true; // Para iniciar el movimiento de la bola si no se ha movido antes
            ball.StartMoving();
        }
    }

    public void ResetPosition()
    {
        // Resetear el paddle al centro
        hasMoved = false;
        if (movementSlider != null)
        {
            movementSlider.value = initialSliderValue;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            ExtraLife();
            Destroy(other.gameObject); // Destruye el power-up después de recogerlo
        }
    }

    private void ExtraLife()
    {
        GameManager.Instance.AddLife();
    }
}
