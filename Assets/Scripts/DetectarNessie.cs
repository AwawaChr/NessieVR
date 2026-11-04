
using UnityEngine;

public class DetectarNessie : MonoBehaviour
{
    [Header("Referencias")]
    public Light linterna;
    public GameObject nessie;

    [Header("Configuracion")]
    [Tooltip("Radio de la esfera utilizada para ampliar la deteccion.")]
    public float radioDeteccion = 0.3f;

    [Tooltip("Intervalo entre mensajes de depuracion.")]
    public float intervaloDebug = 1f;

    [Header("Resultado")]
    public bool nessieIluminado;

    private float siguienteDebug;
    private bool estadoAnterior;
    private bool primerEstado = true;

    private void Update()
    {
        // Comprueba si Nessie se encuentra dentro del haz.
        nessieIluminado = ComprobarIluminacion(nessie);

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
        // Comprueba que las referencias esten asignadas.
        if (linterna == null)
        {
            MostrarDepuracion("ERROR: No se ha asignado la linterna.");
            return false;
        }

        if (objetivo == null)
        {
            MostrarDepuracion("ERROR: No se ha asignado Nessie.");
            return false;
        }

        // Comprueba que el foco sea de tipo Spot.
        if (linterna.type != LightType.Spot)
        {
            MostrarDepuracion("ERROR: La linterna no es de tipo Spot.");
            return false;
        }

        // Obtiene los colliders del objeto y de sus hijos.
        Collider[] colliders = objetivo.GetComponentsInChildren<Collider>();

        if (colliders.Length == 0)
        {
            MostrarDepuracion("ERROR: Nessie no tiene ningun Collider.");
            return false;
        }

        // Obtiene la posicion y direccion de la linterna.
        Vector3 origen = linterna.transform.position;
        Vector3 direccion = linterna.transform.forward;

        // Calcula el angulo maximo permitido.
        float anguloMaximo = linterna.spotAngle / 2f;

        // Lanza una esfera invisible hacia delante.
        RaycastHit[] impactos = Physics.SphereCastAll(
            origen,
            radioDeteccion,
            direccion,
            linterna.range
        );

        // Comprueba cada objeto alcanzado por el SphereCast.
        foreach (RaycastHit impacto in impactos)
        {
            // Comprueba si el impacto pertenece a Nessie.
            bool perteneceANessie =
                impacto.collider.transform == objetivo.transform ||
                impacto.collider.transform.IsChildOf(objetivo.transform);

            if (!perteneceANessie)
                continue;

            // Calcula la direccion desde la linterna hasta Nessie.
            Vector3 direccionNessie = impacto.point - origen;

            // Calcula la distancia hasta el punto de impacto.
            float distancia = direccionNessie.magnitude;

            // Calcula el angulo entre el foco y Nessie.
            float angulo = Vector3.Angle(
                direccion,
                direccionNessie
            );

            // Comprueba si Nessie esta dentro del alcance.
            bool dentroAlcance = distancia <= linterna.range;

            // Comprueba si Nessie esta dentro del cono de luz.
            bool dentroAngulo = angulo <= anguloMaximo;

            // Informa de los resultados de la comprobacion.
            if (dentroAlcance && dentroAngulo)
            {
                MostrarDepuracion(
                    "DETECTADO: Nessie esta dentro del haz. " +
                    "Distancia: " + distancia.ToString("F2") +
                    " | Angulo: " + angulo.ToString("F2")
                );

                return true;
            }

            MostrarDepuracion(
                "IMPACTO EN NESSIE, PERO FUERA DEL CONO. " +
                "Distancia: " + distancia.ToString("F2") +
                " | Angulo: " + angulo.ToString("F2") +
                " | Angulo maximo: " + anguloMaximo.ToString("F2")
            );
        }

        MostrarDepuracion(
            "SIN DETECCION: SphereCast no ha encontrado Nessie."
        );

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
        Gizmos.DrawRay(origen, derecha * linterna.range);
        Gizmos.DrawRay(origen, izquierda * linterna.range);
        Gizmos.DrawRay(origen, arriba * linterna.range);
        Gizmos.DrawRay(origen, abajo * linterna.range);
    }
}