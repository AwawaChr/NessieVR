using System.Collections;
using UnityEngine;

/// <summary>
/// Gestiona la aparición, inmersión y reubicación de Nessie,
/// así como los efectos de audio asociados a sus transiciones acuáticas.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class NessieController : MonoBehaviour
{
    [Header("Referencias de Gestión")]
    [SerializeField] private NessieSpawnManager spawnManager;
    [SerializeField] private GameObject nessieVisualRoot;

    [Header("Parámetros de Animación de Agua")]
    [Tooltip("Profundidad a la que se hunde bajo el agua")]
    [SerializeField] private float submergedDepth = 0.4f;
    [Tooltip("Duración de la animación de subida/bajada en segundos")]
    [SerializeField] private float transitionDuration = 0.6f;

    [Header("Audio")]
    [Tooltip("Sonido al salir del agua")]
    [SerializeField] private AudioClip emergeAudioClip;
    [Tooltip("Sonido al sumergirse en el agua")]
    [SerializeField] private AudioClip submergeAudioClip;

    private AudioSource audioSource;
    private Coroutine activeTransitionCoroutine;
    private bool isCurrentlyVisible = false;
    private Vector3 baseScale;

    public bool IsVisible => isCurrentlyVisible;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0.5f; // Audio semi-espacial en AR

        if (nessieVisualRoot == null)
        {
            nessieVisualRoot = gameObject;
        }

        baseScale = nessieVisualRoot.transform.localScale;

        // Empieza completamente oculto
        nessieVisualRoot.SetActive(false);
        isCurrentlyVisible = false;
    }

    /// <summary>
    /// Desaparece de su posición actual y reaparece en un nuevo Spawn Point aleatorio.
    /// </summary>
    public void RelocateToRandomSpawn()
    {
        if (spawnManager == null)
        {
            Debug.LogError("[NessieController] No hay NessieSpawnManager asignado.");
            return;
        }

        Transform targetSpawn = spawnManager.GetRandomSpawnPoint();
        if (targetSpawn != null)
        {
            StartCoroutine(RoutineRelocateSequence(targetSpawn));
        }
    }

    /// <summary>
    /// Forzar reaparición en un punto concreto.
    /// </summary>
    public void SpawnAt(Transform spawnTransform)
    {
        if (spawnTransform == null) return;
        StartCoroutine(RoutineRelocateSequence(spawnTransform));
    }

    private IEnumerator RoutineRelocateSequence(Transform targetSpawn)
    {
        // Si ya está visible, primero sumergirse
        if (isCurrentlyVisible)
        {
            yield return StartCoroutine(RoutineSubmerge());
        }

        // Posicionar en el nuevo punto garantizando jerarquía y escala
        transform.SetPositionAndRotation(targetSpawn.position, targetSpawn.rotation);
        nessieVisualRoot.transform.localScale = baseScale;

        // Emerger del agua
        yield return StartCoroutine(RoutineEmerge());
    }

    public void Emerge()
    {
        if (activeTransitionCoroutine != null) StopCoroutine(activeTransitionCoroutine);
        activeTransitionCoroutine = StartCoroutine(RoutineEmerge());
    }

    public void Submerge()
    {
        if (activeTransitionCoroutine != null) StopCoroutine(activeTransitionCoroutine);
        activeTransitionCoroutine = StartCoroutine(RoutineSubmerge());
    }

    private IEnumerator RoutineEmerge()
    {
        isCurrentlyVisible = true;
        nessieVisualRoot.SetActive(true);

        // Sonido de emerger del agua
        PlaySound(emergeAudioClip);

        Vector3 targetLocalPos = Vector3.zero;
        Vector3 startLocalPos = new Vector3(0f, -submergedDepth, 0f);
        nessieVisualRoot.transform.localPosition = startLocalPos;

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            // Curva suave de salida
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            nessieVisualRoot.transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, smoothT);
            yield return null;
        }

        nessieVisualRoot.transform.localPosition = targetLocalPos;
        activeTransitionCoroutine = null;
    }

    private IEnumerator RoutineSubmerge()
    {
        // Sonido de sumergirse en el agua
        PlaySound(submergeAudioClip);

        Vector3 startLocalPos = nessieVisualRoot.transform.localPosition;
        Vector3 targetLocalPos = new Vector3(0f, -submergedDepth, 0f);

        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / transitionDuration);
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            nessieVisualRoot.transform.localPosition = Vector3.Lerp(startLocalPos, targetLocalPos, smoothT);
            yield return null;
        }

        nessieVisualRoot.transform.localPosition = targetLocalPos;
        nessieVisualRoot.SetActive(false);
        isCurrentlyVisible = false;
        activeTransitionCoroutine = null;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}