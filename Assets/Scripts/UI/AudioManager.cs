
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sonidos de interfaz")]
    public AudioClip sonidoBoton;
    public AudioClip sonidoCambioPantalla;
    public AudioClip sonidoInicioPartida;
    public AudioClip sonidoPerder;
    public AudioClip sonidoGanar;

    [Header("Música de fondo (opcional)")]
    public AudioClip musicaFondo;

    private AudioSource efectos;
    private AudioSource musica;

    [Header("Volumen efectos")]
    public float volumenEfectos = 1f;
    private Slider sliderEfectos;

    [Header("Volumen Musica")]
    public float volumenMusica = 1f;
    private Slider sliderMusica;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        efectos = gameObject.AddComponent<AudioSource>();
        musica = gameObject.AddComponent<AudioSource>();

        efectos.playOnAwake = false;

        musica.playOnAwake = false;
        musica.loop = true;
        musica.volume = volumenMusica;

        if (musicaFondo != null)
        {
            musica.clip = musicaFondo;
            musica.Play();
        }
    }

    // Conectar los Sliders de la escena actual
    public void ConfigurarSliders(Slider sliderEfectosUI, Slider sliderMusicaUI)
    {
        sliderEfectos = sliderEfectosUI;
        sliderMusica = sliderMusicaUI;

        if (sliderEfectos != null)
        {
            sliderEfectos.SetValueWithoutNotify(volumenEfectos);
            sliderEfectos.onValueChanged.AddListener(CambiarVolumenEfectos);
        }

        if (sliderMusica != null)
        {
            sliderMusica.SetValueWithoutNotify(volumenMusica);
            sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }
    }

    public void CambiarVolumenEfectos(float volumen)
    {
        volumenEfectos = volumen;
    }

    public void CambiarVolumenMusica(float volumen)
    {
        volumenMusica = volumen;
        musica.volume = volumenMusica;
    }

    public void SonidoBoton()
    {
        if (sonidoBoton != null)
            efectos.PlayOneShot(sonidoBoton, volumenEfectos);
    }

    public void SonidoCambioPantalla()
    {
        if (sonidoCambioPantalla != null)
            efectos.PlayOneShot(sonidoCambioPantalla, volumenEfectos);
    }

    public void SonidoInicioPartida()
    {
        if (sonidoInicioPartida != null)
            efectos.PlayOneShot(sonidoInicioPartida, volumenEfectos);
    }

    public void SonidoPerdiste()
    {
        if (sonidoPerder != null)
            efectos.PlayOneShot(sonidoPerder, volumenEfectos);
    }

    public void SonidoGanaste()
    {
        if (sonidoGanar != null)
            efectos.PlayOneShot(sonidoGanar, volumenEfectos);
    }
    public void ReproducirMusica()
    {
        if (musicaFondo != null && !musica.isPlaying)
        {
            musica.Play();
        }
    }

    public void DetenerMusica()
    {
        musica.Stop();
    }
}
