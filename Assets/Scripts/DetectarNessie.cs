using UnityEngine;
using UnityEngine.UI;

public class DetectarNessie : MonoBehaviour
{
    [Header("Referencias")]
    public Light linterna;
    public GameObject nessie;

    [Header("Exposición")]
    public Slider barraExposicion;
    public float tiempoExposicionNecesario = 10f;

    [Header("Control del juego")]
    public ControlDelJuego controlJuego;

    [Header("Configuracion")]
    public float radioDeteccion = 0.1f;
    public float intervaloDebug = 1f;

    [Header("Resultado")]
    public bool nessieIluminado;

    // === INICIO: REFERENCIA DE AUDIO AÑADIDA ===
    [Header("Audio Detección")]
    public AudioSource audioSource;
    // === FIN: REFERENCIA DE AUDIO AÑADIDA ===

    private float exposicionActual = 0f;
    private float siguienteDebug;

    private bool estadoAnterior;
    private bool primerEstado = true;
    private bool nessieEliminado = false;

    private void Start()
    {
        if (barraExposicion != null)
        {
            barraExposicion.minValue = 0f;
            barraExposicion.maxValue = 1f;
            barraExposicion.value = 0f;
        }

        // === INICIO: COMPROBAR OBTENCIÓN DEL AUDIO SOURCE ===
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        // === FIN: COMPROBAR OBTENCIÓN DEL AUDIO SOURCE ===
    }

    private void Update()
    {
        if (nessieEliminado)
            return;

        // Comprueba si Nessie está iluminado.
        nessieIluminado = ComprobarIluminacion(nessie);

        // Actualiza la exposición acumulativa.
        if (nessieIluminado)
        {
            exposicionActual += Time.deltaTime;
        }

        // Evita superar el tiempo necesario.
        exposicionActual = Mathf.Clamp(
            exposicionActual,
            0f,
            tiempoExposicionNecesario
        );

        // Actualiza la barra de exposición.
        if (barraExposicion != null &&
            tiempoExposicionNecesario > 0f)
        {
            barraExposicion.value =
                exposicionActual / tiempoExposicionNecesario;
        }

        // Comprueba si Nessie está completamente expuesto.
        if (exposicionActual >= tiempoExposicionNecesario &&
            tiempoExposicionNecesario > 0f)
        {
            nessieEliminado = true;

            // === INICIO: DETENER MÚSICA AL COMPLETAR LA EXPOSICIÓN ===
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
            // === FIN: DETENER MÚSICA AL COMPLETAR LA EXPOSICIÓN ===

            Debug.Log("¡Nessie completamente expuesto!");

            if (controlJuego != null)
            {
                controlJuego.MonstruoMuerto();
            }
            else
            {
                Debug.LogError(
                    "No se ha asignado ControlDelJuego."
                );
            }

            return;
        }

        // Informa cuando cambia el estado de iluminación.
        if (primerEstado || nessieIluminado != estadoAnterior)
        {
            Debug.Log(nessieIluminado
                ? "[LINTERNA] Nessie está iluminado."
                : "[LINTERNA] Nessie no está iluminado.");

            // === INICIO: GESTIÓN DE REPRODUCCIÓN DE MÚSICA AL CAMBIAR DE ESTADO ===
            if (audioSource != null)
            {
                if (nessieIluminado)
                {
                    if (!audioSource.isPlaying)
                        audioSource.Play();
                }
                else
                {
                    if (audioSource.isPlaying)
                        audioSource.Stop();
                }
            }
            // === FIN: GESTIÓN DE REPRODUCCIÓN DE MÚSICA AL CAMBIAR DE ESTADO ===

            estadoAnterior = nessieIluminado;
            primerEstado = false;
        }
    }

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

        // Comprueba el alcance.
        if (distancia > linterna.range)
            return false;

        // Comprueba el ángulo de la linterna.
        float angulo = Vector3.Angle(
            linterna.transform.forward,
            direccion
        );

        if (angulo > linterna.spotAngle / 2f)
            return false;

        // Comprueba si existe algún obstáculo.
        if (Physics.Raycast(
            origen,
            direccion.normalized,
            out RaycastHit impacto,
            distancia
        ))
        {
            bool esNessie =
                impacto.collider.transform == objetivo.transform ||
                impacto.collider.transform.IsChildOf(
                    objetivo.transform
                );

            if (esNessie)
            {
                MostrarDepuracion(
                    "DETECTADO: Nessie está iluminado."
                );

                return true;
            }

            MostrarDepuracion(
                "BLOQUEADO: " + impacto.collider.name
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

        float semianngulo = linterna.spotAngle / 2f;

        Vector3 derecha = Quaternion.AngleAxis(
            semianngulo,
            linterna.transform.up
        ) * direccion;

        Vector3 izquierda = Quaternion.AngleAxis(
            -semianngulo,
            linterna.transform.up
        ) * direccion;

        Vector3 arriba = Quaternion.AngleAxis(
            -semianngulo,
            linterna.transform.right
        ) * direccion;

        Vector3 abajo = Quaternion.AngleAxis(
            semianngulo,
            linterna.transform.right
        ) * direccion;

        Gizmos.DrawRay(
            origen,
            derecha * linterna.range
        );

        Gizmos.DrawRay(
            origen,
            izquierda * linterna.range
        );

        Gizmos.DrawRay(
            origen,
            arriba * linterna.range
        );

        Gizmos.DrawRay(
            origen,
            abajo * linterna.range
        );
    }
}