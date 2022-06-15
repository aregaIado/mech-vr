using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public bool isFollowing = true;

    private void Update()
    {
        if(target != null && isFollowing)
        {
            transform.position = target.position + offset;
        }
    }
}
