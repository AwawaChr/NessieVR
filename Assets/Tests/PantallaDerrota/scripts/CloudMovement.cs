
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed = 0.2f;
    public float horizontalDistance = 4f;
    public float verticalDistance = 1f;

    private RectTransform rect;
    private Vector2 startPosition;
    private float offset;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPosition = rect.anchoredPosition;
        offset = Random.Range(0f, 10f);
    }

    void Update()
    {
        float movement = Mathf.Sin(Time.time * speed + offset);

        float x = movement * horizontalDistance;
        float y = Mathf.Sin(Time.time * speed * 0.7f + offset)
                  * verticalDistance;

        rect.anchoredPosition = startPosition + new Vector2(x, y);
    }
}
