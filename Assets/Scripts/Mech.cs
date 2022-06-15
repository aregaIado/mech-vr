using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Mech : MonoBehaviour
{

    [Header("Setup")]
    public InputActionAsset playerInputs;

    public Joystick rightStick;
    public Joystick leftStick;
    public Transform guns;

    Vector2 move = new Vector2();
    Vector2 look = new Vector2();

    CharacterController character;
    TargetingSystem targetingSystem;

    Vector3 storedEulers = new Vector3();

    [Header("Control Settings")]
    public float moveSpeed = 50f;
    public float horizontalTurnSpeed = 25f;
    public float verticalTurnSpeed = 10f;
    public float maxVerticalAngle = 30f;

    private void Start()
    {
        character = GetComponent<CharacterController>();
        targetingSystem = GetComponentInChildren<TargetingSystem>();
    }
    private void Update()
    {
        //match look values to joystick
        if (look != rightStick.vector2Inputs)
            look = -rightStick.vector2Inputs;

        //match movement values to joystick
        if (move != leftStick.vector2Inputs)
            move = leftStick.vector2Inputs;

        //horizontal look rotation
        transform.Rotate(Vector3.up, look.x * horizontalTurnSpeed * Time.deltaTime);
        
        //vertical look rotation
        storedEulers += new Vector3(0, 0, -look.y * verticalTurnSpeed * Time.deltaTime);
        storedEulers = new Vector3(storedEulers.x, storedEulers.y, Mathf.Clamp(storedEulers.z, -maxVerticalAngle/2, maxVerticalAngle/2));
        guns.localRotation = Quaternion.Euler(storedEulers);

        //update movement
        character.Move(transform.right * move.y * moveSpeed * Time.deltaTime);

    }

    public void GrabRightJoystick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Toggle Stick");
            if (rightStick.isTriggered)
                rightStick.isGrabbed = !rightStick.isGrabbed;
        }
    }
    public void GrabLeftJoystick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Toggle stick");
            if (leftStick.isTriggered)
                leftStick.isGrabbed = !leftStick.isGrabbed;
        }
    }

    public void FireWeapons(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Debug.Log("Started Firing...");
            targetingSystem.StartFiring();
        }
        if (context.canceled)
        {
            //Debug.Log("Stopped Firing...");
            targetingSystem.StopFiring();
        }
    }
}
