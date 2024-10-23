using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Asegúrate de incluir esta línea para usar UI

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    public float maxX = 7.5f;
    public Slider movementSlider; // Referencia al Slider

    void Start()
    {
        if (movementSlider != null)
        {
            movementSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    void OnSliderValueChanged(float value)
    {
        float movementHorizontal = value * 2 - 1; // Convierte el rango de 0-1 a -1 a 1
        if ((movementHorizontal > 0 && transform.position.x < maxX) || 
            (movementHorizontal < 0 && transform.position.x > -maxX))
        {
            transform.position += Vector3.right * movementHorizontal * speed * Time.deltaTime;
        }
    }

    void Update()
    {
        // No es necesario realizar nada en Update para el movimiento del slider
    }
}
