using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class ARVideoTouchControls : MonoBehaviour
{
    [Header("Componente de Vídeo")]
    [SerializeField] private VideoPlayer videoPlayer;

    [Header("Contenedor de Controles UI")]
    [Tooltip("El GameObject padre en el Canvas que contiene los botones")]
    [SerializeField] private GameObject controlsContainer;

    [Header("Botones y Textos UI")]
    [SerializeField] private Button btnPlayPause;
    [SerializeField] private Button btnRestart;
    [SerializeField] private TextMeshProUGUI txtPlayPause;

    private void Awake()
    {
        if (btnPlayPause != null)
            btnPlayPause.onClick.AddListener(TogglePlayPause);

        if (btnRestart != null)
            btnRestart.onClick.AddListener(RestartVideo);

        UpdatePlayPauseLabel();
    }

    // Callback para Vuforia: On Target Found
    public void OnTargetFound()
    {
        if (controlsContainer != null)
            controlsContainer.SetActive(true);

        if (videoPlayer != null)
            videoPlayer.Play();

        UpdatePlayPauseLabel();
    }

    // Callback para Vuforia: On Target Lost
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
        {
            videoPlayer.Pause();
        }
        else
        {
            videoPlayer.Play();
        }

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
}