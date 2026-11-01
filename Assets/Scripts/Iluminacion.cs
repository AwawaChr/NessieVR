
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightPostProcessing : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;

    [Header("Post Processing")]
    [SerializeField] private Volume volume;

    [Header("Configuración nocturna")]
    [SerializeField] private float nightExposure = -2f;
    [SerializeField] private float nightContrast = 10f;
    [SerializeField] private float nightSaturation = -10f;
    [SerializeField] private float nightBloom = 3f;

    [Header("Filtro de color nocturno")]
    [SerializeField] private Color nightColorFilter =
        new Color(0.55f, 0.65f, 1f);

    [Header("Viñeta nocturna")]
    [SerializeField] private Color nightVignetteColor =
        new Color(0.08f, 0.12f, 0.3f);

    [SerializeField] private float nightVignette = 0.35f;
    [SerializeField] private float vignetteSmoothness = 0.5f;

    private ColorAdjustments colorAdjustments;
    private Bloom bloom;
    private Vignette vignette;

    // Valores originales del día obtenidos del Volume Profile.
    private float originalExposure;
    private float originalContrast;
    private float originalSaturation;
    private float originalBloom;

    private Color originalColorFilter;

    private float originalVignette;
    private float originalVignetteSmoothness;
    private Color originalVignetteColor;

    private float currentSliderValue;

    // Comprueba las referencias, obtiene los Overrides,
    // guarda los valores originales y conecta el Slider.
    // Entrada: ninguna.
    // Salida: void.
    private void Start()
    {
        if (slider == null || volume == null || volume.profile == null)
        {
            Debug.LogError(
                "DayNightPostProcessing: Falta asignar el Slider o el Volume.",
                this
            );

            enabled = false;
            return;
        }

        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out bloom);
        volume.profile.TryGet(out vignette);

        CaptureDaytimeSettings();

        currentSliderValue = slider.value;

        slider.onValueChanged.AddListener(UpdateEffects);

        ApplyEffects(currentSliderValue);
    }

    // Obtiene y guarda todos los valores originales del día
    // directamente desde los Overrides del Volume Profile.
    // Entrada: ninguna.
    // Salida: void.
    private void CaptureDaytimeSettings()
    {
        if (colorAdjustments != null)
        {
            originalExposure = colorAdjustments.postExposure.value;
            originalContrast = colorAdjustments.contrast.value;
            originalSaturation = colorAdjustments.saturation.value;
            originalColorFilter = colorAdjustments.colorFilter.value;
        }

        if (bloom != null)
        {
            originalBloom = bloom.intensity.value;
        }

        if (vignette != null)
        {
            originalVignette = vignette.intensity.value;
            originalVignetteSmoothness = vignette.smoothness.value;
            originalVignetteColor = vignette.color.value;
        }
    }

    // Recibe el valor del Slider y actualiza la transición.
    // Entrada: float value, valor entre 0 (día) y 1 (noche).
    // Salida: void.
    public void UpdateEffects(float value)
    {
        currentSliderValue = Mathf.Clamp01(value);

        ApplyEffects(currentSliderValue);
    }

    // Aplica todos los efectos según el valor actual de la transición.
    // Entrada: float value, intensidad de la transición.
    // Salida: void.
    private void ApplyEffects(float value)
    {
        UpdateColorAdjustments(value);
        UpdateBloom(value);
        UpdateVignette(value);
    }

    // Interpola exposición, contraste, saturación y filtro de color
    // entre los valores originales del día y los valores nocturnos.
    // Entrada: float value, intensidad de la transición.
    // Salida: void.
    private void UpdateColorAdjustments(float value)
    {
        if (colorAdjustments == null)
            return;

        colorAdjustments.postExposure.value =
            Mathf.Lerp(originalExposure, nightExposure, value);

        colorAdjustments.contrast.value =
            Mathf.Lerp(originalContrast, nightContrast, value);

        colorAdjustments.saturation.value =
            Mathf.Lerp(originalSaturation, nightSaturation, value);

        colorAdjustments.colorFilter.value =
            Color.Lerp(originalColorFilter, nightColorFilter, value);
    }

    // Interpola la intensidad del Bloom entre el valor original
    // del día y el valor configurado para la noche.
    // Entrada: float value, intensidad de la transición.
    // Salida: void.
    private void UpdateBloom(float value)
    {
        if (bloom == null)
            return;

        bloom.intensity.value =
            Mathf.Lerp(originalBloom, nightBloom, value);
    }

    // Interpola el color, la intensidad y la suavidad de la viñeta
    // entre la configuración original del día y la nocturna.
    // Entrada: float value, intensidad de la transición.
    // Salida: void.
    private void UpdateVignette(float value)
    {
        if (vignette == null)
            return;

        vignette.intensity.value =
            Mathf.Lerp(originalVignette, nightVignette, value);

        vignette.smoothness.value =
            Mathf.Lerp(originalVignetteSmoothness, vignetteSmoothness, value);

        vignette.color.value =
            Color.Lerp(originalVignetteColor, nightVignetteColor, value);
    }

    // Actualiza los efectos continuamente para aplicar los cambios
    // realizados en los parámetros nocturnos desde el Inspector.
    // Entrada: ninguna.
    // Salida: void.
    private void Update()
    {
        ApplyEffects(currentSliderValue);
    }

    // Desconecta el evento del Slider al destruir el componente.
    // Entrada: ninguna.
    // Salida: void.
    private void OnDestroy()
    {
        if (slider != null)
            slider.onValueChanged.RemoveListener(UpdateEffects);
    }
}
