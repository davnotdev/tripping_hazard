using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joint : MonoBehaviour
{
    public Vector3 begin;
    public Vector3 end;

    private const float joint_length_multiplier = 1.1f;

    void Start()
    {

    }

    void Update()
    {
        var diff = (begin - end);
        var center = new Vector3(begin.x + end.x, transform.position.y, begin.z + end.z) / 2;

        transform.position = center;
        transform.localScale = new Vector3(transform.localScale.x, (diff.magnitude / 2) * joint_length_multiplier, transform.localScale.z);
        //  Because I can.
        var θ = diff.z == 0
            ? 0
            : Mathf.Atan(diff.z / diff.x) * (180 / Mathf.PI);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, -θ, transform.eulerAngles.z);
    }
}
