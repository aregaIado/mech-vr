using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCockpit : MonoBehaviour
{
    [Header("Setup")]
    public Joystick rightStick;
    public Joystick leftStick;
    public Transform guns;

    Vector2 move = new Vector2();
    Vector2 look = new Vector2();

    CharacterController character;
    Vector3 storedEulers = new Vector3();

    [Header("Control Settings")]
    public float moveSpeed = 50f;
    public float horizontalTurnSpeed = 25f;
    public float verticalTurnSpeed = 10f;
    public float maxVerticalAngle = 30f;

    private void Start()
    {
        character = GetComponent<CharacterController>();
    }
    private void Update()
    {
        //match look values to joystick
        if (look != rightStick.vector2Inputs)
            look = rightStick.vector2Inputs;

        //match movement values to joystick
        if (move != leftStick.vector2Inputs)
            move = leftStick.vector2Inputs;

        //horizontal look rotation
        transform.Rotate(Vector3.up, look.x * horizontalTurnSpeed * Time.deltaTime);
        //vertical look rotation
        storedEulers += new Vector3(look.y * verticalTurnSpeed * Time.deltaTime, 0, 0);
        storedEulers = new Vector3(Mathf.Clamp(storedEulers.x, -maxVerticalAngle, maxVerticalAngle), storedEulers.y, storedEulers.z);
        guns.localRotation = Quaternion.Euler(storedEulers);


        //update movement
        character.Move(transform.forward * move.y * moveSpeed * Time.deltaTime);
    }
}
