using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    public Vector3 begin;
    public Vector3 end;

    void Start()
    {

    }

    void Update()
    {
        var diff = (begin - end);
        var center = new Vector3(begin.x + end.x, transform.position.y, begin.z + end.z) / 2;

        transform.position = center;
        transform.localScale = new Vector3(0.1f, diff.magnitude / 2, 0.1f);
        //  Because I can.
        var θ = diff.z == 0
            ? 0
            : Mathf.Atan(diff.z / diff.x) * (180 / Mathf.PI);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -θ, transform.eulerAngles.z);
    }
}
