using UnityEngine;
using UnityEngine.InputSystem;

public class Movimiento: MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.

        //movimiento delante, atras, izq, derecha
        Vector2 move = gamepad.leftStick.ReadValue();

        //rotacion
        Vector2 rotate = gamepad.rightStick.ReadValue();

        Vector3 movement = new Vector3(move.x, 0, move.y);

        transform.Translate(
           movement * movementSpeed * Time.deltaTime,
           Space.Self
       );

        transform.Rotate(
            Vector3.up,
            rotate.x * rotationSpeed * Time.deltaTime
        );
    }
}
