using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotationAxis : MonoBehaviour {

    public bool x = true;
    public bool y = true;
    public bool z = true;

    Vector3 rotation;
    void Awake()
    {
        rotation = transform.rotation.eulerAngles;
    }
    void LateUpdate()
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(
            x ? rotation.x : currentRotation.x, 
            y ? rotation.y : currentRotation.y, 
            z ? rotation.z : currentRotation.z);
    }
}
