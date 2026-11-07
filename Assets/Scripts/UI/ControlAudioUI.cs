
using UnityEngine;
using UnityEngine.UI;

public class ControlAudioUI : MonoBehaviour
{
    [Header("Sliders")]
    [SerializeField] private Slider sliderEfectos;
    [SerializeField] private Slider sliderMusica;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.ConfigurarSliders(
                sliderEfectos,
                sliderMusica
            );
        }
        else
        {
            Debug.LogError("No se ha encontrado el AudioManager");
        }
    }

    // Sonidos de los botones
    public void SonidoBoton()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoBoton();
    }

    public void SonidoCambioPantalla()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoCambioPantalla();
    }

    public void SonidoInicioPartida()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SonidoInicioPartida();
    }
}
