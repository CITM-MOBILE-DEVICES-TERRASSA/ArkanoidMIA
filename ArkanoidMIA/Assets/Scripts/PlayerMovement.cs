// PlayerMovement.cs
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float maxX = 7.5f;
    public Slider movementSlider;
    private BouncyBall ball;
    private bool hasMoved = false;
    private float initialSliderValue = 0.5f; // Valor central del slider

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

    void OnSliderValueChanged(float value)
    {
        // Si es el primer movimiento, iniciar el movimiento de la pelota
        if (!hasMoved && value != initialSliderValue)
        {
            hasMoved = true;
            ball.StartMoving();
        }

        // Mover el paddle
        float posX = Mathf.Lerp(-maxX, maxX, value);
        Vector3 newPosition = new Vector3(posX, transform.position.y, transform.position.z);
        transform.position = newPosition;
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
}