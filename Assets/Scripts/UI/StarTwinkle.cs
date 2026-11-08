
using UnityEngine;
using UnityEngine.UI;

public class StarTwinkle : MonoBehaviour
{
    private Image star;
    private Vector3 originalScale;
    private Color originalColor;

    private float randomOffset;
    private float randomSpeed;

    [Header("Twinkle Settings")]
    public float minAlpha = 0.05f;
    public float maxAlpha = 1f;

    public float minScale = 0.65f;
    public float maxScale = 1.4f;

    void Start()
    {
        star = GetComponent<Image>();

        originalScale = transform.localScale;
        originalColor = star.color;

        randomOffset = Random.Range(0f, 10f);
        randomSpeed = Random.Range(2f, 4f);
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.unscaledTime * randomSpeed
                    + randomOffset) + 1f) / 2f;

        // Stronger brightness change
        Color color = originalColor;
        color.a = Mathf.Lerp(minAlpha, maxAlpha, t);
        star.color = color;

        // Stars grow and shrink
        float scale = Mathf.Lerp(minScale, maxScale, t);
        transform.localScale = originalScale * scale;
    }
}
