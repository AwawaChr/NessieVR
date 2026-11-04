using UnityEngine;

/// <summary>
/// Controlador principal del minijuego en el archipiélago.
/// Permanece bloqueado hasta que el sistema de Ground Plane confirma el anclaje del escenario.
/// </summary>
public class ArchipelagoGameManager : MonoBehaviour
{
    [Header("Estado del Juego")]
    [SerializeField] private bool isGameActive = false;

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
        // Suscripción al evento de anclaje confirmado
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

        // Activar entidades y comportamientos
        SetGameplaySystemsActive(true);

        // Aquí puedes disparar timers, spawns de personajes o audio ambiental
    }

    private void Update()
    {
        // Si el juego aún no ha empezado, no procesar nada
        if (!isGameActive) return;

        // --- LÓGICA DE ACTUALIZACIÓN DEL MINIJUEGO AQUÍ ---
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