using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * My Billboard Renderer
 * Wessel Roelofse
 * 23/12/2021
 * 
 * This script lets planes render like a billboard (because the default unity billboard renderer sucks).
 * Ive also added scaling as an extra functionality (even though billboards would not have it).
 */
public class MyBillboardRenderer : MonoBehaviour
{
    public Transform target;
    public Transform graphic;
    public float scale = 1;

    private Vector3 startScale;

    private void Start()
    {
        startScale = graphic.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        float dst = Vector3.Distance(transform.position, target.position);
        dst *= scale;
        graphic.localScale = startScale * dst;
    }
}
