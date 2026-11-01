using UnityEngine;
using UnityEngine.Video;

public class ARVideoTargetController : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private RenderTexture targetRenderTexture;

    private void Awake()
    {
        if (videoPlayer == null)
            videoPlayer = GetComponentInChildren<VideoPlayer>();
    }

    // Llamar cuando Vuforia detecte el marcador
    public void OnTargetFound()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    // Llamar cuando la cámara pierda el marcador
    public void OnTargetLost()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Pause(); // Usa Stop() si prefieres que reinicie desde el principio
        }
    }

    private void OnDestroy()
    {
        // Limpiar la textura para no dejar el último fotograma congelado en memoria
        if (targetRenderTexture != null)
        {
            RenderTexture activeRT = RenderTexture.active;
            RenderTexture.active = targetRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = activeRT;
        }
    }
}