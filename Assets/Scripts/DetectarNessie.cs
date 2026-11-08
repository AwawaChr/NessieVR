
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DetectarNessie : MonoBehaviour
{
    [Header("Referencias")]
    public Light linterna;
    public GameObject nessie;
    public NessieController nessieController;

    [Header("Exposición")]
    public Slider barraExposicion;
    public float tiempoExposicionNecesario = 10f;
    public float progresoPorDeteccion = 2f;

    [Header("Control del juego")]
    public ControlDelJuego controlJuego;

    [Header("Configuración")]
    public float radioDeteccion = 0.1f;
    public float intervaloDebug = 1f;
    public float tiempoBloqueoTrasDeteccion = 1.5f;

    [Header("Audio de detección")]
    public AudioSource audioDeteccion;
    public AudioClip sonidoIluminacion;

    [Header("Resultado")]
    public bool nessieIluminado;

    private float exposicionActual = 0f;
    private float siguienteDebug = 0f;

    private bool nessieEliminado = false;
    private bool estaReapareciendo = false;
    private bool esperandoQueDejeDeIluminar = false;

    private void Start()
    {
        if (barraExposicion != null)
        {
            barraExposicion.minValue = 0f;
            barraExposicion.maxValue = 1f;
            barraExposicion.value = 0f;
        }

        if (nessie == null)
            Debug.LogError("[DetectarNessie] Nessie no asignada.");

        if (linterna == null)
            Debug.LogError("[DetectarNessie] Linterna no asignada.");

        if (nessieController == null)
            Debug.LogError("[DetectarNessie] NessieController no asignado.");

        if (audioDeteccion == null)
            Debug.LogWarning("[DetectarNessie] Audio de detección no asignado.");
    }

    private void Update()
    {
        if (nessieEliminado || estaReapareciendo)
            return;

        nessieIluminado = ComprobarIluminacion(nessie);

        // Esperar a que el jugador deje de iluminar a Nessie
        // antes de permitir otra detección.
        if (esperandoQueDejeDeIluminar)
        {
            if (!nessieIluminado)
            {
                esperandoQueDejeDeIluminar = false;
                Debug.Log("[NESSIE] Lista para una nueva detección.");
            }

            return;
        }

        if (!nessieIluminado)
            return;

        // Nueva detección
        Debug.Log("[NESSIE] ¡Iluminada!");

        // Reproducir el sonido asignado directamente al AudioSource
        if (audioDeteccion != null)
        {
            audioDeteccion.Play();
        }

        // Añadir progreso
        exposicionActual += progresoPorDeteccion;

        exposicionActual = Mathf.Clamp(
            exposicionActual,
            0f,
            tiempoExposicionNecesario
        );

        ActualizarBarra();

        Debug.Log(
            "[NESSIE] Progreso: " +
            exposicionActual + "/" +
            tiempoExposicionNecesario
        );

        // Comprobar victoria
        if (exposicionActual >= tiempoExposicionNecesario)
        {
            nessieEliminado = true;
            nessieIluminado = false;

            Debug.Log("¡VICTORIA! Nessie ha sido encontrada.");

            if (controlJuego != null)
            {
                controlJuego.MonstruoMuerto();
            }
            else
            {
                Debug.LogError(
                    "[DetectarNessie] ControlDelJuego no asignado."
                );
            }

            return;
        }

        // Evitar que una misma iluminación cuente varias veces
        esperandoQueDejeDeIluminar = true;

        if (nessieController != null)
        {
            StartCoroutine(ProcesarDeteccion());
        }
        else
        {
            Debug.LogError(
                "[DetectarNessie] No se puede mover a Nessie: " +
                "falta NessieController."
            );
        }
    }

    // =========================================
    // REUBICACIÓN DE NESSIE
    // =========================================

    private IEnumerator ProcesarDeteccion()
    {
        estaReapareciendo = true;

        // El NessieController reproduce:
        // 1. Sonido de sumergirse.
        // 2. Movimiento hacia abajo y desaparición.
        // 3. Cambio de Spawn Point.
        // 4. Sonido de emerger.
        // 5. Aparición de Nessie.

        nessieController.RelocateToRandomSpawn();

        // Esperar a que terminen las transiciones de agua.
        yield return new WaitForSeconds(tiempoBloqueoTrasDeteccion);

        estaReapareciendo = false;

        nessieIluminado = ComprobarIluminacion(nessie);

        Debug.Log("[NESSIE] Reaparecida. ¡Encuéntrala de nuevo!");
    }

    // =========================================
    // ACTUALIZAR BARRA
    // =========================================

    private void ActualizarBarra()
    {
        if (barraExposicion != null &&
            tiempoExposicionNecesario > 0f)
        {
            barraExposicion.value =
                exposicionActual / tiempoExposicionNecesario;
        }
    }

    // =========================================
    // DETECCIÓN DE ILUMINACIÓN
    // =========================================

    private bool ComprobarIluminacion(GameObject objetivo)
    {
        if (linterna == null || objetivo == null)
            return false;

        Collider colisionador =
            objetivo.GetComponentInChildren<Collider>();

        if (colisionador == null)
            return false;

        Vector3 origen = linterna.transform.position;
        Vector3 destino = colisionador.bounds.center;

        Vector3 direccion = destino - origen;
        float distancia = direccion.magnitude;

        // Comprobar alcance
        if (distancia > linterna.range)
            return false;

        // Comprobar ángulo
        float angulo = Vector3.Angle(
            linterna.transform.forward,
            direccion
        );

        if (angulo > linterna.spotAngle / 2f)
            return false;

        // Comprobar obstáculos
        if (Physics.Raycast(
            origen,
            direccion.normalized,
            out RaycastHit impacto,
            distancia
        ))
        {
            bool esNessie =
                impacto.collider.transform == objetivo.transform ||
                impacto.collider.transform.IsChildOf(objetivo.transform);

            if (esNessie)
            {
                MostrarDepuracion("Nessie está iluminada.");
                return true;
            }

            MostrarDepuracion(
                "Luz bloqueada por: " + impacto.collider.name
            );
        }

        return false;
    }

    private void MostrarDepuracion(string mensaje)
    {
        if (Time.time >= siguienteDebug)
        {
            Debug.Log("[DETECTOR LINTERNA] " + mensaje);
            siguienteDebug = Time.time + intervaloDebug;
        }
    }

    // =========================================
    // GIZMOS
    // =========================================

    private void OnDrawGizmos()
    {
        if (linterna == null)
            return;

        Vector3 origen = linterna.transform.position;
        Vector3 direccion = linterna.transform.forward;

        Gizmos.color = nessieIluminado
            ? Color.green
            : Color.red;

        Gizmos.DrawRay(
            origen,
            direccion * linterna.range
        );
    }
}
