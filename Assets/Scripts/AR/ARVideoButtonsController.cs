using UnityEngine;
using UnityEngine.Video;
using Vuforia;

public class ARVideoButtonsController : MonoBehaviour
{
    [Header("Referencias de Componentes")]
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private VirtualButtonBehaviour vbPlayPause;
    [SerializeField] private VirtualButtonBehaviour vbRestart;

    private void Awake()
    {
        if (videoPlayer == null)
        {
            videoPlayer = GetComponentInChildren<VideoPlayer>();
        }

        if (videoPlayer == null)
        {
            Debug.LogError("<color=red>[ARButtons] ERROR:</color> No se encontró ningún componente VideoPlayer en el objeto o sus hijos.");
        }
    }

    private void Start()
    {
        RegisterVirtualButtons();
    }

    private void RegisterVirtualButtons()
    {
        // 1. Registro del botón Play/Pause
        if (vbPlayPause != null)
        {
            vbPlayPause.RegisterOnButtonPressed(OnPlayPausePressed);
            vbPlayPause.RegisterOnButtonReleased(OnPlayPauseReleased);
            Debug.Log($"<color=cyan>[ARButtons] REGISTRADO:</color> Botón Play/Pause ('{vbPlayPause.VirtualButtonName}') listo.");
        }
        else
        {
            Debug.LogWarning("<color=orange>[ARButtons] AVISO:</color> vbPlayPause no está asignado en el Inspector.");
        }

        // 2. Registro del botón Restart
        if (vbRestart != null)
        {
            vbRestart.RegisterOnButtonPressed(OnRestartPressed);
            vbRestart.RegisterOnButtonReleased(OnRestartReleased);
            Debug.Log($"<color=cyan>[ARButtons] REGISTRADO:</color> Botón Restart ('{vbRestart.VirtualButtonName}') listo.");
        }
        else
        {
            Debug.LogWarning("<color=orange>[ARButtons] AVISO:</color> vbRestart no está asignado en el Inspector.");
        }
    }

    private void OnDisable()
    {
        // Desregistrar callbacks para evitar referencias huérfanas en memoria
        if (vbPlayPause != null)
        {
            vbPlayPause.UnregisterOnButtonPressed(OnPlayPausePressed);
            vbPlayPause.UnregisterOnButtonReleased(OnPlayPauseReleased);
        }

        if (vbRestart != null)
        {
            vbRestart.UnregisterOnButtonPressed(OnRestartPressed);
            vbRestart.UnregisterOnButtonReleased(OnRestartReleased);
        }
    }

    // --- Callbacks Botón Play / Pause ---
    private void OnPlayPausePressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("<color=green>[ARButtons] PRESIONADO:</color> Botón Play/Pause detectó oclusión.");

        if (videoPlayer == null) return;

        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            Debug.Log("<color=yellow>[ARButtons] ACCIÓN:</color> Vídeo Pausado.");
        }
        else
        {
            videoPlayer.Play();
            Debug.Log("<color=lime>[ARButtons] ACCIÓN:</color> Vídeo Reanudado.");
        }
    }

    private void OnPlayPauseReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("<color=white>[ARButtons] LIBERADO:</color> Botón Play/Pause ya no está cubierto.");
    }

    // --- Callbacks Botón Restart ---
    private void OnRestartPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("<color=green>[ARButtons] PRESIONADO:</color> Botón Restart detectó oclusión.");

        if (videoPlayer == null) return;

        videoPlayer.time = 0;
        videoPlayer.Play();
        Debug.Log("<color=lime>[ARButtons] ACCIÓN:</color> Vídeo Reiniciado al segundo 00:00.");
    }

    private void OnRestartReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("<color=white>[ARButtons] LIBERADO:</color> Botón Restart ya no está cubierto.");
    }
}