using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDetector : MonoBehaviour
{
    private string currentDevice = "";

    void Update()
    {
        if (Gamepad.current != null &&
            Gamepad.current.wasUpdatedThisFrame)
        {
            currentDevice = "Controller";
        }

        if (Mouse.current != null &&
            Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            currentDevice = "Mouse";
        }

        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            currentDevice = "Keyboard";
        }
    }
}