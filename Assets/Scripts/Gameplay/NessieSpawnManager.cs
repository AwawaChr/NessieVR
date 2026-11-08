using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gestiona y valida los puntos de aparición (Spawn Points) de Nessie.
/// Garantiza la coherencia espacial relativa al anclaje AR del archipiélago.
/// </summary>
public class NessieSpawnManager : MonoBehaviour
{
    [Header("Colección de Puntos")]
    [Tooltip("Lista de los transforms correspondientes a cada Spawn Point")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Gizmos y Depuración")]
    [Tooltip("Radio estimado de Nessie para comprobar colisiones en la vista Scene")]
    [SerializeField] private float gizmoRadius = 0.15f;
    [SerializeField] private Color gizmoColor = new Color(0f, 0.9f, 0.7f, 0.45f);

    private int lastSpawnIndex = -1;

    public int TotalSpawnPoints => spawnPoints != null ? spawnPoints.Length : 0;

    /// <summary>
    /// Devuelve un Spawn Point aleatorio asegurando que sea distinto al anterior.
    /// </summary>
    public Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[NessieSpawnManager] No hay Spawn Points asignados en el Inspector.");
            return null;
        }

        if (spawnPoints.Length == 1) return spawnPoints[0];

        int newIndex;
        do
        {
            newIndex = Random.Range(0, spawnPoints.Length);
        } while (newIndex == lastSpawnIndex);

        lastSpawnIndex = newIndex;
        return spawnPoints[newIndex];
    }

    /// <summary>
    /// Devuelve la posición y rotación del spawn point indicado por índice.
    /// </summary>
    public Transform GetSpawnPoint(int index)
    {
        if (spawnPoints == null || index < 0 || index >= spawnPoints.Length)
        {
            Debug.LogError($"[NessieSpawnManager] Índice {index} fuera de rango.");
            return null;
        }

        return spawnPoints[index];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (spawnPoints == null) return;

        Gizmos.color = gizmoColor;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] != null)
            {
                // Dibujar esfera del tamaño de Nessie
                Gizmos.DrawSphere(spawnPoints[i].position, gizmoRadius);
                Gizmos.DrawWireSphere(spawnPoints[i].position, gizmoRadius);

                // Dibujar flecha de orientación hacia donde mira el punto
                Gizmos.color = Color.blue;
                Gizmos.DrawRay(spawnPoints[i].position, spawnPoints[i].forward * (gizmoRadius * 1.5f));
                Gizmos.color = gizmoColor;
            }
        }
    }
#endif
}