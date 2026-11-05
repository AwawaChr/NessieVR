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

    [Header("Música de fondo (opcional)")]
    public AudioClip musicaFondo;

    private AudioSource efectos;
    private AudioSource musica;
    
    [Header("Volumen efectos")]
    public float volumenEfectos = 1f;
    public Slider sliderEfectos;
    
    [Header("Volumen Musica")]
    public float volumenMusica = 1f;
    public Slider sliderMusica;
    
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
        
        if (sliderMusica != null)
        {
            sliderMusica.value = volumenMusica;

            sliderMusica.onValueChanged.AddListener(CambiarVolumenMusica);
        }

        if (musicaFondo != null)    
        {
            musica.clip = musicaFondo;
            musica.Play();
        }
    }
    
    public void CambiarVolumenMusica(float volumen)
    {
        volumenMusica = volumen;
        musica.volume = volumenMusica;
    }

    public void SonidoBoton()
    {
        efectos.PlayOneShot(sonidoBoton,sliderEfectos.value);
    }

    public void SonidoCambioPantalla()
    {
        efectos.PlayOneShot(sonidoCambioPantalla,sliderEfectos.value);
    }

    public void SonidoInicioPartida()
    {
        efectos.PlayOneShot(sonidoInicioPartida,sliderEfectos.value);
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