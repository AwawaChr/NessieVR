using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using TMPro;

public class ARVideoTouchControls : MonoBehaviour
{
    [Header("Componente de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Contenedor de Controles UI")]
    [SerializeField] private GameObject controlsContainer;

    [Header("Botones y Textos UI")]
    [SerializeField] private Button btnPlayPause;
    [SerializeField] private Button btnRestart;
    [SerializeField] private Button btnBackToMenu;
    [SerializeField] private TextMeshProUGUI txtPlayPause;

    [Header("Navegación")]
    [Tooltip("Nombre exacto de la escena del menú principal en Build Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private void Awake()
    {
        if (btnPlayPause != null)
            btnPlayPause.onClick.AddListener(TogglePlayPause);

        if (btnRestart != null)
            btnRestart.onClick.AddListener(RestartVideo);

        if (btnBackToMenu != null)
            btnBackToMenu.onClick.AddListener(ReturnToMainMenu);

        UpdatePlayPauseLabel();
    }

    private void Update()
    {
        // Soporte para botón 'Atrás' nativo de Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMainMenu();
        }
    }

    public void OnTargetFound()
    {
        if (controlsContainer != null)
            controlsContainer.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Play();

        UpdatePlayPauseLabel();
    }

    public void OnTargetLost()
    {
        if (controlsContainer != null)
            controlsContainer.SetActive(false);

        if (videoPlayer != null && videoPlayer.isPlaying)
            videoPlayer.Pause();
    }

    public void TogglePlayPause()
    {
        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying)
            videoPlayer.Pause();
        else
            videoPlayer.Play();

        UpdatePlayPauseLabel();
    }

    public void RestartVideo()
    {
        if (videoPlayer == null) return;

        videoPlayer.time = 0;
        videoPlayer.Play();
        UpdatePlayPauseLabel();
    }

    private void UpdatePlayPauseLabel()
    {
        if (txtPlayPause != null && videoPlayer != null)
        {
            txtPlayPause.text = videoPlayer.isPlaying ? "PAUSA" : "PLAY";
        }
    }

    public void ReturnToMainMenu()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}