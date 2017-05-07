using System;
using UnityEngine;

[Serializable]
public class HeadBobWrapper {

    [SerializeField]
    private CurveControlledBob headBobCurve = new CurveControlledBob();

    private Vector3 originalObjectPosition; 

    public GameObject headBobObject;

    public void Setup(float stepInterval)
    {
        originalObjectPosition = headBobObject.transform.localPosition;
        headBobCurve.Setup(headBobObject, stepInterval);
    }

    public void DoHeadBob(float speed)
    {
        Vector3 newPosition = headBobCurve.DoHeadBob(speed);
        newPosition.z = originalObjectPosition.z;
        headBobObject.transform.localPosition = newPosition;
    }
}
