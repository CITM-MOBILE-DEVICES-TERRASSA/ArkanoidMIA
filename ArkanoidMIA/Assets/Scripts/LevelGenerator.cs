using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public Vector2Int size; // Tamaño del nivel
    public Vector2 offset; // Espaciado entre ladrillos
    public GameObject brickPrefab; // Prefab de ladrillo
    
    [System.Serializable]
    public class RowConfig
    {
        public int hitPoints; // Dureza de los ladrillos en esta fila
        public Color color; // Color opcional para cada fila
    }
    
    public RowConfig[] rowConfigs; // Configuración de cada fila

    private void Awake()
    {
        // Si no hay configuraciones de fila, crear una configuración por defecto
        if (rowConfigs == null || rowConfigs.Length == 0)
        {
            InitializeDefaultRowConfigs();
        }

        // Asegurarse de que tenemos suficientes configuraciones para todas las filas
        if (rowConfigs.Length < size.y)
        {
            Debug.LogWarning("No hay suficientes configuraciones de fila. Se repetirá el último patrón.");
        }

        for (int j = 0; j < size.y; j++)
        {
            // Obtener la configuración para esta fila
            RowConfig currentConfig = GetRowConfig(j);

            for (int i = 0; i < size.x; i++)
            {
                GameObject newBrick = Instantiate(brickPrefab, transform);
                newBrick.transform.position = transform.position + new Vector3((float)((size.x - 1) * .5f - i) * offset.x, j * offset.y, 0);
                
                // Configurar el ladrillo
                Brick brickComponent = newBrick.GetComponent<Brick>();
                if (brickComponent != null)
                {
                    brickComponent.hitPoints = currentConfig.hitPoints;
                    
                    // Si el ladrillo tiene un SpriteRenderer, actualizar su color inicial
                    SpriteRenderer spriteRenderer = newBrick.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null)
                    {
                        spriteRenderer.color = currentConfig.color;
                    }
                }
            }
        }
    }

    private void InitializeDefaultRowConfigs()
    {
        // Configuración por defecto - 8 tipos de filas
        rowConfigs = new RowConfig[]
        {
            new RowConfig { hitPoints = 1, color = new Color(0.9f, 0.7f, 0.7f) }, // Rosa claro
            new RowConfig { hitPoints = 2, color = new Color(0.7f, 0.9f, 0.7f) }, // Verde claro
            new RowConfig { hitPoints = 3, color = new Color(0.7f, 0.7f, 0.9f) }, // Azul claro
            new RowConfig { hitPoints = 4, color = new Color(0.9f, 0.9f, 0.7f) }, // Amarillo claro
            new RowConfig { hitPoints = 5, color = new Color(0.9f, 0.7f, 0.9f) }, // Rosa fuerte
            new RowConfig { hitPoints = 6, color = new Color(0.7f, 0.9f, 0.9f) }, // Cian claro
            new RowConfig { hitPoints = 7, color = new Color(0.8f, 0.8f, 0.8f) }, // Gris claro
            new RowConfig { hitPoints = 8, color = new Color(0.9f, 0.5f, 0.5f) }  // Rojo claro
        };
    }

    private RowConfig GetRowConfig(int rowIndex)
    {
        // Si tenemos una configuración específica para esta fila, usarla
        if (rowIndex < rowConfigs.Length)
        {
            return rowConfigs[rowIndex];
        }
        
        // Si no hay suficientes configuraciones, usar la última disponible
        return rowConfigs[rowConfigs.Length - 1];
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}