
using TMPro;
using UnityEngine;

public class ControlDelJuego : MonoBehaviour
{
    [Header("Temporizador")]
    [SerializeField] private TMP_Text timerTexto;
    [SerializeField, Tooltip("Tiempo en segundos")] private float time = 300f;

    [Header("Resultado")]
    [SerializeField] private TMP_Text resultadoTexto;

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
        resultadoTexto.text = "¡VICTORIA!";
        Debug.Log("¡Has ganado!");
    }

    private void Derrota()
    {
        juegoTerminado = true;
        resultadoTexto.text = "¡DERROTA!";
        Debug.Log("¡Has perdido!");
    }
}
