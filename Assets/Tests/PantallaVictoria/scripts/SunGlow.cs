
using UnityEngine;
using UnityEngine.UI;

public class SunGlow : MonoBehaviour
{
    public float speed = 1.5f;
    public float minScale = 0.85f;
    public float maxScale = 1.1f;

    public float minAlpha = 0.5f;
    public float maxAlpha = 1f;

    private RectTransform rect;
    private Image image;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    void Update()
    {
        float pulse = (Mathf.Sin(Time.time * speed) + 1f) / 2f;

        float scale = Mathf.Lerp(minScale, maxScale, pulse);
        rect.localScale = Vector3.one * scale;

        float alpha = Mathf.Lerp(minAlpha, maxAlpha, pulse);

        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
