using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    public MeshRenderer[] renderers;
    public bool showRenderers = true;

    private void Awake()
    {
        if (!showRenderers)
        {
            foreach(var r in renderers)
            {
                r.enabled = false;
            }
        }
    }

}
