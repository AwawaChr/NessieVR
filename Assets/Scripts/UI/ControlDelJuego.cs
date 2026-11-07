
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlDelJuego : MonoBehaviour
{
    [Header("Temporizador")]
    [SerializeField] private TMP_Text timerTexto;
    [SerializeField, Tooltip("Tiempo en segundos")] private float time = 300f;

    [Header("Resultado")]
    [SerializeField] private GameObject imagenVictoria;
    [SerializeField] private GameObject imagenDerrota;

    private int minutos, segundos;
    private bool juegoTerminado = false;

    void Update()
    {
        if (juegoTerminado) return;

        time -= Time.deltaTime;

        if (time <= 0)
        {
            time = 0;
            Derrota();
        }

        minutos = (int)(time / 60f);
        segundos = Mathf.CeilToInt(time % 60f);

        timerTexto.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void MonstruoMuerto()
    {
        if (juegoTerminado) return;

        Victoria();
    }

    private void Victoria()
    {
        juegoTerminado = true;

        imagenVictoria.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetenerMusica();
            AudioManager.Instance.SonidoGanaste();
        }

        Debug.Log("¡Has ganado!");
    }

    private void Derrota()
    {
        juegoTerminado = true;

        imagenDerrota.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.DetenerMusica();
            AudioManager.Instance.SonidoPerdiste();
        }

        Debug.Log("¡Has perdido!");
    }

    public void reinicioPartida()
    {
        Time.timeScale = 1f;
        Scene escenaActual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(escenaActual.name, LoadSceneMode.Single);
    }
}
