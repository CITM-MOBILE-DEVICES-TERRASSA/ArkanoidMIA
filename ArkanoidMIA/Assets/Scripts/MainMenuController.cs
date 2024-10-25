using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    private Button continueButton;
    private Button playButton;

    private void Awake()
    {
        // Busca los botones dentro del Canvas
        Transform canvas = transform.Find("Canvas");
        if (canvas != null)
        {
            // Encuentra los botones por su nombre en la jerarquía
            continueButton = canvas.Find("ButtonContinue")?.GetComponent<Button>();
            playButton = canvas.Find("Button")?.GetComponent<Button>();

            // Verificación de debug
            Debug.Log("Continue Button found: " + (continueButton != null));
            Debug.Log("Play Button found: " + (playButton != null));
        }
        else
        {
            Debug.LogError("Canvas not found in hierarchy!");
        }
    }

    private void Start()
    {
        // Configura los listeners de los botones
        if (playButton != null)
        {
            playButton.onClick.AddListener(PlayGame);
        }
        
        if (continueButton != null)
        {
            continueButton.onClick.AddListener(ContinueGame);
            // Solo habilita el botón continue si hay una partida guardada
            continueButton.interactable = PlayerPrefs.HasKey("CurrentLevel");
        }
    }

    public void PlayGame()
    {
        Debug.Log("Play button clicked!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartNewGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found! Make sure you have a GameManager in your scene.");
        }
    }

    public void ContinueGame()
    {
        Debug.Log("Continue button clicked!");
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ContinueGame();
        }
        else
        {
            Debug.LogError("GameManager instance not found! Make sure you have a GameManager in your scene.");
        }
    }
}