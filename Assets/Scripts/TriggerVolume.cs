using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerVolume : MonoBehaviour
{
    //public bool isTriggered = false;
    public Transform controllerTransform;

    private Joystick parentStick;

    private void Awake()
    {
        parentStick = transform.parent.GetComponent<Joystick>();
    }
    private void Start()
    {
        //transform.SetParent(transform.parent.parent);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Controller") //XRBaseController //other.gameObject.GetComponent<SphereCollider>()
        {
            //isTriggered = true;
            parentStick.isTriggered = true;
            controllerTransform = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Controller") ////XRBaseController
        {
            //isTriggered = false;
            parentStick.isTriggered = false;
            parentStick.isGrabbed = false;
            controllerTransform = null;
        }
    }
}
