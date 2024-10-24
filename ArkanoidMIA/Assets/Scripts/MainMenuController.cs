using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        // Cambia "SampleScene" por el nombre de la escena de tu Arkanoid
        SceneManager.LoadScene("SampleScene");
    }
}
