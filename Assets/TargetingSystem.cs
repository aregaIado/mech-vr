using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetingSystem : MonoBehaviour
{
    public bool targetingEnabled = true;
    public Transform rayOrigin;
    public Transform target;
    public Gun rightGun;
    public Gun leftGun;

    private LineRenderer leftRend;
    private LineRenderer rightRend;

    public GameObject targetingWindow;
    public Transform targetLens;
    public Transform playerHead;
    public Transform targetingSource;
    public LayerMask targetingLayer;
    public float minRange, maxRange;

    private bool lensIsActive = false;

    private void Start()
    {
        targetingSource.SetParent(null);

        leftRend = leftGun.GetComponent<LineRenderer>();
        rightRend = rightGun.GetComponent<LineRenderer>();
    }
    private void Update()
    {
        //logic for targeting
        if (targetingEnabled)
        {
            //move target transform to hit position
            if (Physics.Raycast(rayOrigin.position, transform.right, out RaycastHit hitInfo, maxRange))
            {
                //move target to hit position
                target.position = hitInfo.point;
                //Debug.DrawRay(rayOrigin.position, transform.right * hitInfo.distance, Color.red);
                //Debug.Log(hitInfo.transform.name);

                //move targeting lens to correct position on targeting window
                targetingSource.position = playerHead.position;
                targetingSource.LookAt(target);
                if (Physics.Raycast(targetingSource.position, targetingSource.forward, out RaycastHit eyeHit, 1f, targetingLayer))
                {
                    //Debug.DrawRay(targetingSource.position, targetingSource.forward * eyeHit.distance, Color.blue);
                    //Debug.Log(eyeHit.transform.name);

                    if (eyeHit.collider.gameObject == targetingWindow)
                    {
                        //Debug.Log("moving targetingLens");
                        if(lensIsActive == false)
                            targetLens.gameObject.SetActive(true);
                        lensIsActive = true;
                        targetLens.position = eyeHit.point;
                    }
                    else
                    {
                        targetLens.gameObject.SetActive(false);
                        lensIsActive = false;
                    }
                }
            }
            else
            {
                //Debug.DrawRay(rayOrigin.position, transform.right * maxRange, Color.green);
            }

            //rotate guns to aim at hit position
            leftRend.SetPosition(1, new Vector3(Vector3.Distance(leftGun.transform.position, target.position), 0, 0));
            rightRend.SetPosition(1, new Vector3(Vector3.Distance(rightGun.transform.position, target.position), 0, 0));

            leftGun.transform.right = target.position - leftGun.transform.position;
            rightGun.transform.right = target.position - rightGun.transform.position;

            //leftGun.LookAt(target);
            //rightGun.LookAt(target);

        }
    }

    public void StartFiring()
    {
        leftGun.TriggerDown();
        rightGun.TriggerDown();
    }
    public void StopFiring()
    {
        leftGun.TriggerUp();
        rightGun.TriggerUp();
    }
}
