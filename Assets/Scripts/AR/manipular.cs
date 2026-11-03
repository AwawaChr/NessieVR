using UnityEngine;

public class manipular : MonoBehaviour
{
    [Header("Rotation")]
    public float rotSpeed = 0.4f;

    [Header("Pinch Zoom")]
    public float minScale = 0.5f;
    public float maxScale = 2.5f;

    private Camera arCamera;
    private Renderer[] objectRenderers;

    private bool rotating = false;
    private bool pinching = false;

    private float initialDistance;
    private Vector3 initialScale;

    void Start()
    {
        // Find the Vuforia ARCamera directly
        GameObject cameraObject = GameObject.Find("ARCamera");

        if (cameraObject != null)
            arCamera = cameraObject.GetComponent<Camera>();

        // Fallback if ARCamera was not found
        if (arCamera == null)
            arCamera = Camera.main;

        // Get renderers without requiring a Collider
        objectRenderers = GetComponentsInChildren<Renderer>();

        initialScale = transform.localScale;
    }

    void Update()
    {
        if (arCamera == null || Input.touchCount == 0)
            return;

        // ONE FINGER: ROTATE
        if (Input.touchCount == 1)
        {
            pinching = false;

            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                rotating = IsTouchingObject(touch.position);

            if (touch.phase == TouchPhase.Moved && rotating)
            {
                transform.Rotate(
                    touch.deltaPosition.y * rotSpeed,
                    -touch.deltaPosition.x * rotSpeed,
                    0,
                    Space.World
                );
            }

            if (touch.phase == TouchPhase.Ended ||
                touch.phase == TouchPhase.Canceled)
            {
                rotating = false;
            }
        }

        // TWO FINGERS: PINCH TO ZOOM
        else if (Input.touchCount == 2)
        {
            rotating = false;

            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (!pinching)
            {
                if (IsTouchingObject(touchZero.position) ||
                    IsTouchingObject(touchOne.position))
                {
                    initialDistance = Vector2.Distance(
                        touchZero.position,
                        touchOne.position
                    );

                    initialScale = transform.localScale;
                    pinching = true;
                }

                return;
            }

            float currentDistance = Vector2.Distance(
                touchZero.position,
                touchOne.position
            );

            if (initialDistance <= 0f)
                return;

            float factor = currentDistance / initialDistance;

            float scaleFactor = Mathf.Clamp(
                factor,
                minScale,
                maxScale
            );

            transform.localScale = initialScale * scaleFactor;
        }
    }

    private bool IsTouchingObject(Vector2 screenPosition)
    {
        if (objectRenderers == null || objectRenderers.Length == 0)
            return false;

        foreach (Renderer rend in objectRenderers)
        {
            Bounds bounds = rend.bounds;

            Vector3 min = bounds.min;
            Vector3 max = bounds.max;

            float minX = float.MaxValue;
            float minY = float.MaxValue;
            float maxX = float.MinValue;
            float maxY = float.MinValue;

            for (int i = 0; i < 8; i++)
            {
                Vector3 corner = new Vector3(
                    (i & 1) == 0 ? min.x : max.x,
                    (i & 2) == 0 ? min.y : max.y,
                    (i & 4) == 0 ? min.z : max.z
                );

                Vector3 screenPoint = arCamera.WorldToScreenPoint(corner);

                if (screenPoint.z <= 0)
                    continue;

                minX = Mathf.Min(minX, screenPoint.x);
                minY = Mathf.Min(minY, screenPoint.y);
                maxX = Mathf.Max(maxX, screenPoint.x);
                maxY = Mathf.Max(maxY, screenPoint.y);
            }

            if (screenPosition.x >= minX &&
                screenPosition.x <= maxX &&
                screenPosition.y >= minY &&
                screenPosition.y <= maxY)
            {
                return true;
            }
        }

        return false;
    }
}