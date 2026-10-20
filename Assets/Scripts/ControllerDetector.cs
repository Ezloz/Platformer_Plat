using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerDetector : MonoBehaviour
{
    public string currentDevice {get; private set;} = "";

    void Update()
    {
        if (Gamepad.current != null &&
            Gamepad.current.wasUpdatedThisFrame)
        {
            currentDevice = "Controller";
        }

        if (Keyboard.current != null &&
            Keyboard.current.anyKey.wasPressedThisFrame)
        {
            currentDevice = "Keyboard";
        }
    }
}