using UnityEngine;

/// <summary>
/// Gestiona la transición visual del escenario entre el modo de previsualización (ghost)
/// y el modo final anclado con sus materiales definitivos.
/// </summary>
public class PlacementPreviewFeedback : MonoBehaviour
{
    [Header("Renderers a Modificar")]
    [Tooltip("Lista de mallas del escenario. Si se deja vacía, buscará automáticamente todas las hijas.")]
    [SerializeField] private Renderer[] targetRenderers;

    [Header("Material Ghost")]
    [Tooltip("Material transparente/translúcido para la fase de búsqueda.")]
    [SerializeField] private Material previewGhostMaterial;

    // Almacén en memoria de los materiales originales de cada Renderer
    private Material[][] originalMaterials;
    private bool isInitialized = false;

    private void Awake()
    {
        InitializeRenderers();
    }

    private void InitializeRenderers()
    {
        if (isInitialized) return;

        // Si no se asignaron manualmente en el inspector, los obtenemos automáticamente
        if (targetRenderers == null || targetRenderers.Length == 0)
        {
            targetRenderers = GetComponentsInChildren<Renderer>(true);
        }

        // Cachear los materiales originales compartidos para restaurarlos después
        originalMaterials = new Material[targetRenderers.Length][];
        for (int i = 0; i < targetRenderers.Length; i++)
        {
            originalMaterials[i] = targetRenderers[i].sharedMaterials;
        }

        isInitialized = true;
    }

    /// <summary>
    /// Activa o desactiva la apariencia de previsualización.
    /// </summary>
    /// <param name="isPreview">True para aplicar ghost, False para restaurar originales.</param>
    public void SetPreviewMode(bool isPreview)
    {
        if (!isInitialized)
        {
            InitializeRenderers();
        }

        if (previewGhostMaterial == null)
        {
            Debug.LogWarning("[PlacementPreviewFeedback] No se ha asignado el material previewGhostMaterial en el Inspector.");
            return;
        }

        for (int i = 0; i < targetRenderers.Length; i++)
        {
            if (targetRenderers[i] == null) continue;

            if (isPreview)
            {
                // Reemplazamos todos los sub-materiales de esta malla por el material ghost
                Material[] ghostMats = new Material[originalMaterials[i].Length];
                for (int m = 0; m < ghostMats.Length; m++)
                {
                    ghostMats[m] = previewGhostMaterial;
                }
                targetRenderers[i].materials = ghostMats;
            }
            else
            {
                // Restauramos la lista exacta de materiales originales
                targetRenderers[i].materials = originalMaterials[i];
            }
        }
    }
}