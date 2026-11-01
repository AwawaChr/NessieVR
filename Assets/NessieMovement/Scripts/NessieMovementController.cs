
using UnityEngine;

public class NessieMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0.08f;
    [SerializeField] private float turnSpeed = 35f;
    [SerializeField] private TemporaryJoystick joystick;

    [Header("Swim Area")]
    [SerializeField] private BoxCollider swimArea;
    [SerializeField, Min(0f)] private float boundaryPadding = 0.1f;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    private void Update()
    {
        if (joystick != null)
        {
            Vector2 input = joystick.Direction;

            // Swim forward and backward.
            transform.Translate(
                Vector3.forward * input.y * moveSpeed * Time.deltaTime,
                Space.Self
            );

            // Turn around the Image Target's up axis.
            Vector3 upAxis = swimArea != null
                ? swimArea.transform.up
                : Vector3.up;

            transform.Rotate(
                upAxis,
                input.x * turnSpeed * Time.deltaTime,
                Space.World
            );
        }

        KeepInsideSwimArea();
    }

    private void KeepInsideSwimArea()
    {
        if (swimArea == null)
            return;

        Transform areaTransform = swimArea.transform;

        // Convert Nessie's position into SwimArea local coordinates.
        Vector3 localPosition =
            areaTransform.InverseTransformPoint(transform.position);

        Vector3 center = swimArea.center;
        Vector3 halfSize = swimArea.size * 0.5f;

        // Keep Nessie inside the X and Z boundaries.
        localPosition.x = Mathf.Clamp(
            localPosition.x,
            center.x - halfSize.x + boundaryPadding,
            center.x + halfSize.x - boundaryPadding
        );

        localPosition.z = Mathf.Clamp(
            localPosition.z,
            center.z - halfSize.z + boundaryPadding,
            center.z + halfSize.z - boundaryPadding
        );

        // Convert the corrected position back to world coordinates.
        transform.position = areaTransform.TransformPoint(localPosition);
    }

    public void SetSpeed(float sliderValue)
{
    moveSpeed = Mathf.Lerp(0f, 0.16f, sliderValue);
}
    public void ResetPosition()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
    }
}
