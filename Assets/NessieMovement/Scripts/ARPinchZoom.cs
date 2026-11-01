
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ARPinchZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 0.05f;
    [SerializeField] private float minFOV = 25f;
    [SerializeField] private float maxFOV = 70f;

    private Camera cam;
    private float previousDistance;
    private bool hasPreviousDistance;

    private void Awake()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
            cam = GetComponentInChildren<Camera>();

        if (cam == null)
            Debug.LogError("ARPinchZoom: No Camera component found!");
    }

    private void Update()
    {
        if (cam == null || Touchscreen.current == null)
        {
            hasPreviousDistance = false;
            return;
        }

        var touches = Touchscreen.current.touches;

        Vector2 firstTouch = Vector2.zero;
        Vector2 secondTouch = Vector2.zero;
        int activeTouches = 0;

        // Find the first two active touches.
        for (int i = 0; i < touches.Count; i++)
        {
            TouchControl touch = touches[i];

            if (!touch.press.isPressed)
                continue;

            if (activeTouches == 0)
                firstTouch = touch.position.ReadValue();
            else if (activeTouches == 1)
                secondTouch = touch.position.ReadValue();

            activeTouches++;

            if (activeTouches == 2)
                break;
        }

        // Reset when fewer than two fingers are touching.
        if (activeTouches < 2)
        {
            hasPreviousDistance = false;
            return;
        }

        float currentDistance =
            Vector2.Distance(firstTouch, secondTouch);

        if (!hasPreviousDistance)
        {
            previousDistance = currentDistance;
            hasPreviousDistance = true;
            return;
        }

        float difference = currentDistance - previousDistance;

        cam.fieldOfView = Mathf.Clamp(
            cam.fieldOfView - difference * zoomSpeed,
            minFOV,
            maxFOV
        );

        previousDistance = currentDistance;
    }
}
