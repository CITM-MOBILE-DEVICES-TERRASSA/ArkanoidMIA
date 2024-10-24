using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public int hitPoints = 1; // Puntos de vida del ladrillo
    public Color[] damageColors; // Colores que representan la salud del ladrillo

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Establecer el color inicial según el hitPoints
        UpdateColor();
    }

    // Método para aplicar daño al ladrillo
    public void TakeDamage()
    {
        hitPoints--;
        UpdateColor();
    }

    // Actualizar el color del ladrillo según los puntos de vida
    private void UpdateColor()
    {
        if (hitPoints >= 0 && hitPoints < damageColors.Length)
        {
            spriteRenderer.color = damageColors[hitPoints];
        }

        // Si el ladrillo no tiene más vida, se puede destruir
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Método para verificar si el ladrillo está destruido
    public bool IsDestroyed()
    {
        return hitPoints <= 0;
    }
}
