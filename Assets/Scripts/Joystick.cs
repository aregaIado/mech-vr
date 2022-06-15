using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StickDirection {Center, Left, Right, Up, Down};
public class Joystick : MonoBehaviour
{
    private TriggerVolume triggerVolume;
    private Transform tracker;
    private StickDirection sDirection;

    public bool isGrabbed = false;
    public bool isTriggered = false;
    public Vector2 vector2Inputs;

    private void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        foreach(var c in children)
        {
            if(c.tag == "Tracker")
            {
                tracker = c;

            }
        }
        
        triggerVolume = GetComponentInChildren<BoxCollider>().gameObject.GetComponent<TriggerVolume>();
        triggerVolume.transform.SetParent(triggerVolume.transform.parent.parent);
        vector2Inputs = new Vector2();
    }

    private void Update()
    {
        //if player grabs joystick, rotate stick
        if (isTriggered && isGrabbed) 
        {
            //Debug.Log("stepping...");
            Vector3 adjustedUp = triggerVolume.controllerTransform.position - transform.position;

            //calculate vectors for joystick output
            Vector3 finalTransform = transform.parent.InverseTransformPoint(tracker.position) - transform.localPosition;
            vector2Inputs = RoundVectorValues(finalTransform.z, finalTransform.x, 0.035f);

            //rotate joystick
            transform.up = adjustedUp;

            ///
            /// update this to use a different system for calculating joystick position
            /// probably use 4 colliders or something
            /// or use a single gameobject at the end of the joystick and track position
            /// ended up using local position to fix turning
            ///
            /// old method:
            /// float x = ReturnClamped(-adjustedUp.z);
            /// float y = ReturnClamped(adjustedUp.x);
            /// vector2Inputs = new Vector2(x, y);
        }
        else if(transform.localRotation != Quaternion.Euler(Vector3.zero))
        {
            //Debug.Log("stepping...");
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            vector2Inputs = new Vector2(0, 0);
        }
    }
    private Vector2 RoundVectorValues(float x, float y, float deadzone)
    {

        if (Mathf.Abs(x) < deadzone)
            x = 0;
        if (Mathf.Abs(y) < deadzone)
            y = 0;

        if (x > 0)
            x = 1;
        else if (x < 0)
            x = -1;
        else if (y > 0)
            y = 1;
        else if (y < 0)
            y = -1;

        Vector2 cV2 = new Vector2(x, y);

        return cV2;
    }
    private float ReturnAdjusted(float rawValue)
    {
        //float n = 1 / rawValue;
        float n = 6 + (2 / 3);
        float expected = rawValue * n;

        if (expected > 1)
            expected = 1;
        else if (expected < -1)
            expected = -1;

        return expected;
    }
    private float ReturnClamped(float rawValue)
    {
        float n = 6 + (2 / 3);
        float expected = rawValue * n;


        if (expected > 0.25)
            expected = 1;
        else if (expected < -0.25)
            expected = -1;
        else
            expected = 0;

        return expected;
    }
}
