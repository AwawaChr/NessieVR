
using UnityEngine;

public class NessieMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float turnSpeed = 60f;
    [SerializeField] private TemporaryJoystick joystick;

    [Header("Distance")]
    [SerializeField] private float distanceStep = 0.5f;
    [SerializeField] private float minDistance = -2f;
    [SerializeField] private float maxDistance = 2f;

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;
    private float distanceOffset;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
        distanceOffset = 0f;
    }

    private void Update()
    {
        if (joystick == null)
            return;

        Vector2 input = joystick.Direction;

        // Forward and backward.
        transform.Translate(
            Vector3.forward * input.y * moveSpeed * Time.deltaTime,
            Space.Self
        );

        // Turn left and right.
        transform.Rotate(
    Vector3.up * input.x * turnSpeed * Time.deltaTime,
    Space.World
);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = Mathf.Max(0f, speed);
    }

    public void MoveCloser()
    {
        distanceOffset = Mathf.Max(
            minDistance, distanceOffset - distanceStep
        );
        ApplyDistance();
    }

    public void MoveFarther()
    {
        distanceOffset = Mathf.Min(
            maxDistance, distanceOffset + distanceStep
        );
        ApplyDistance();
    }

    private void ApplyDistance()
    {
        transform.localPosition =
            initialLocalPosition +
            initialLocalRotation * Vector3.forward * distanceOffset;
    }

    public void ResetPosition()
    {
        transform.localPosition = initialLocalPosition;
        transform.localRotation = initialLocalRotation;
        distanceOffset = 0f;
    }
}
