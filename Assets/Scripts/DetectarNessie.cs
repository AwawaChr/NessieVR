
using UnityEngine;
using UnityEngine.UI;
public class DetectarNessie : MonoBehaviour
{
    [Header("Referencias")]
    public Light linterna;
    public GameObject nessie;

    [Header("Configuracion")]
    [Tooltip("Radio de la esfera utilizada para ampliar la deteccion.")]
    public float radioDeteccion = 0.1f;

    [Tooltip("Intervalo entre mensajes de depuracion.")]
    public float intervaloDebug = 1f;

    [Header("Resultado")]
    public bool nessieIluminado;

    private float siguienteDebug;
    private bool estadoAnterior;
    private bool primerEstado = true;

    [Header("Exposición")]
    public Slider barraExposicion;
    public float tiempoExposicionNecesario = 10f;

    [Header("Control del juego")]
    public ControlDelJuego controlJuego;

    private float exposicionActual = 0f;
    private bool nessieEliminado = false;


    private void Update()
    {
        // Comprueba si Nessie se encuentra dentro del haz.
        nessieIluminado = ComprobarIluminacion(nessie);

        if (nessieIluminado && !nessieEliminado)
        {
            exposicionActual += Time.deltaTime;

            if (exposicionActual >= tiempoExposicionNecesario)
            {
                exposicionActual = tiempoExposicionNecesario;
                nessieEliminado = true;

                if (controlJuego != null)
                {
                    controlJuego.MonstruoMuerto();
                }
            }
        }

        if (barraExposicion != null)
        {
            barraExposicion.value =
                exposicionActual / tiempoExposicionNecesario;
        }

        // Informa cuando cambia el estado de deteccion.
        if (primerEstado || nessieIluminado != estadoAnterior)
        {
            Debug.Log(nessieIluminado
                ? "[LINTERNA] Nessie esta iluminado."
                : "[LINTERNA] Nessie no esta iluminado.");

            estadoAnterior = nessieIluminado;
            primerEstado = false;
        }
    }

    /// <summary>
    /// Comprueba si el objeto indicado se encuentra dentro del alcance y del angulo de apertura de la linterna.
    ///
    /// Recibe:
    /// objetivo: GameObject que se desea detectar.
    ///
    /// Devuelve:
    /// true si el objeto esta dentro del haz;
    /// false si esta fuera o no puede detectarse.
    /// </summary>
    
private bool ComprobarIluminacion(GameObject objetivo)
{
    if (linterna == null || objetivo == null)
        return false;

    // Obtiene el Collider de Nessie, incluso si está en un hijo.
    Collider colisionador = objetivo.GetComponentInChildren<Collider>();

    if (colisionador == null)
        return false;

    Vector3 origen = linterna.transform.position;

    // Utiliza el centro del Collider de Nessie.
    Vector3 destino = colisionador.bounds.center;

    Vector3 direccion = destino - origen;

    float distancia = direccion.magnitude;

    // Comprueba que Nessie esté dentro del alcance.
    if (distancia > linterna.range)
        return false;

    // Comprueba que Nessie esté dentro del ángulo de la linterna.
    float angulo = Vector3.Angle(
        linterna.transform.forward,
        direccion
    );

    if (angulo > linterna.spotAngle / 2f)
        return false;

    // Comprueba qué objeto recibe primero el rayo.
    if (Physics.Raycast(
        origen,
        direccion.normalized,
        out RaycastHit impacto,
        distancia
    ))
    {
        // Comprueba si el impacto pertenece a Nessie.
        bool esNessie =
            impacto.collider.transform == objetivo.transform ||
            impacto.collider.transform.IsChildOf(objetivo.transform);

        if (esNessie)
        {
            MostrarDepuracion("DETECTADO: Nessie está iluminado.");
            return true;
        }

        MostrarDepuracion(
            "BLOQUEADO: " + impacto.collider.name
        );
    }

    return false;
}


    /// <summary>
    /// Muestra mensajes de depuracion en la consola respetando el intervalo configurado.
    ///
    /// Recibe:
    /// mensaje: texto que se desea mostrar.
    ///
    /// Devuelve:
    /// No devuelve ningun valor.
    /// </summary>
    private void MostrarDepuracion(string mensaje)
    {
        if (Time.time >= siguienteDebug)
        {
            Debug.Log("[DETECTOR LINTERNA] " + mensaje);
            siguienteDebug = Time.time + intervaloDebug;
        }
    }

    /// <summary>
    /// Dibuja una representacion aproximada del cono de la linterna en la ventana Scene.
    /// </summary>
    private void OnDrawGizmos()
    {
        if (linterna == null)
            return;

        Vector3 origen = linterna.transform.position;
        Vector3 direccion = linterna.transform.forward;

        Gizmos.color = nessieIluminado ? Color.green : Color.red;

        // Dibuja la direccion central del foco.
        Gizmos.DrawRay(origen, direccion * linterna.range);

        // Calcula los limites del cono.
        float semianngulo = linterna.spotAngle / 2f;

        Vector3 derecha = Quaternion.AngleAxis(
            semianngulo, linterna.transform.up
        ) * direccion;

        Vector3 izquierda = Quaternion.AngleAxis(
            -semianngulo, linterna.transform.up
        ) * direccion;

        Vector3 arriba = Quaternion.AngleAxis(
            -semianngulo, linterna.transform.right
        ) * direccion;

        Vector3 abajo = Quaternion.AngleAxis(
            semianngulo, linterna.transform.right
        ) * direccion;

        // Dibuja los cuatro limites aproximados del cono.
        Gizmos.DrawRay(origen, derecha * linterna.range);x
        Gizmos.DrawRay(origen, izquierda * linterna.range);
        Gizmos.DrawRay(origen, arriba * linterna.range);
        Gizmos.DrawRay(origen, abajo * linterna.range);
    }
}