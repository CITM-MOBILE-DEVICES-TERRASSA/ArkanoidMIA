using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float maxX = 7.5f; // Limite de movimiento del paddle
    public Slider movementSlider; // Referencia al Slider

    void Start()
    {
        // Oculta el handle predeterminado del slider
        if (movementSlider != null)
        {
            movementSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    // Este método se llama cuando cambia el valor del slider
    void OnSliderValueChanged(float value)
    {
        // Convierte el valor del slider (0-1) en posición del paddle (-maxX a maxX)
        float posX = Mathf.Lerp(-maxX, maxX, value);
        Vector3 newPosition = new Vector3(posX, transform.position.y, transform.position.z);
        transform.position = newPosition;
    }
}
