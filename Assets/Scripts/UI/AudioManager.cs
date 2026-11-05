using UnityEngine;

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
    [Range(0f, 1f)]
    public float volumenEfectos = 1f;
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

        if (musicaFondo != null)
        {
            musica.clip = musicaFondo;
            musica.Play();
        }
    }

    public void SonidoBoton()
    {
        efectos.PlayOneShot(sonidoBoton,volumenEfectos);
    }

    public void SonidoCambioPantalla()
    {
        efectos.PlayOneShot(sonidoCambioPantalla,volumenEfectos);
    }

    public void SonidoInicioPartida()
    {
        efectos.PlayOneShot(sonidoInicioPartida,volumenEfectos);
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