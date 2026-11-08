using UnityEngine;

/// <summary>
/// Controlador principal del minijuego en el archipiélago.
/// Permanece bloqueado hasta que el sistema de Ground Plane confirma el anclaje del escenario
/// y gestiona el inicio de la sesión y la primera aparición de Nessie.
/// </summary>
public class ArchipelagoGameManager : MonoBehaviour
{
    [Header("Estado del Juego")]
    [SerializeField] private bool isGameActive = false;

    [Header("Personaje Principal")]
    [Tooltip("Referencia al controlador de Nessie")]
    [SerializeField] private NessieController nessie;

    [Header("Elementos del Minijuego a Activar")]
    [Tooltip("Objetos, agentes o sistemas de partículas que deben activarse al arrancar")]
    [SerializeField] private GameObject[] gameplayEntities;

    [Tooltip("Animadores o scripts de IA del archipiélago")]
    [SerializeField] private Behaviour[] gameplayBehaviours;

    public bool IsGameActive => isGameActive;

    private void Awake()
    {
        // Asegurar que todo empiece completamente inactivo
        SetGameplaySystemsActive(false);
    }

    private void OnEnable()
    {
        // Suscripción al evento de anclaje confirmado de Ground Plane
        GroundPlanePlacementController.OnScenarioAnchoredAndReady += HandleScenarioPlaced;
    }

    private void OnDisable()
    {
        // Cancelar suscripción para evitar fugas de memoria
        GroundPlanePlacementController.OnScenarioAnchoredAndReady -= HandleScenarioPlaced;
    }

    /// <summary>
    /// Se ejecuta automáticamente cuando el usuario toca la pantalla o pulsa "Colocar Escenario".
    /// </summary>
    private void HandleScenarioPlaced()
    {
        Debug.Log("[ArchipelagoGameManager] Escenario anclado con éxito. Iniciando minijuego...");
        StartGameSession();
    }

    private void StartGameSession()
    {
        isGameActive = true;

        // Activar entidades secundarias y comportamientos
        SetGameplaySystemsActive(true);

        // Hace aparecer a Nessie en su primer Spawn Point tras colocar la isla
        if (nessie != null)
        {
            nessie.RelocateToRandomSpawn();
        }
        else
        {
            Debug.LogWarning("[ArchipelagoGameManager] NessieController no está asignado en el Inspector.");
        }
    }

    private void Update()
    {
        if (!isGameActive) return;

        // Lógica de bucle de juego si se requiere más adelante
    }

    private void SetGameplaySystemsActive(bool state)
    {
        if (gameplayEntities != null)
        {
            foreach (var entity in gameplayEntities)
            {
                if (entity != null) entity.SetActive(state);
            }
        }

        if (gameplayBehaviours != null)
        {
            foreach (var behaviour in gameplayBehaviours)
            {
                if (behaviour != null) behaviour.enabled = state;
            }
        }
    }
}