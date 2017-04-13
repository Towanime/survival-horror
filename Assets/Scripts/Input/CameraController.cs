using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput playerInput;
    public GameObject target;
    public GameObject dummy;
    public GameObject parentX;
    public GameObject parentY;
    public float maxAngleY = 50f;
    public float minAngleY = 355f;
    // aiming mode
    public bool aiming;
    public GameObject aimAnchor;


    void Start()
    {
        // lock the cursor
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        parentX.transform.rotation *= Quaternion.Euler(0f, this.playerInput.rotation.x, 0f);
        // limit y
        parentY.transform.rotation *= Quaternion.Euler(this.playerInput.rotation.y, 0f, 0f);
        Vector3 angles = parentY.transform.localRotation.eulerAngles;
        //Debug.Log(angles);
        // clamp Y
        if (angles.x > maxAngleY && angles.x < 180)
        {
            angles = new Vector3(maxAngleY, 0, 0);
        }
        else if (angles.x < minAngleY && angles.x > 180)
        {
            angles = new Vector3(minAngleY, 0, 0);
        }
        parentY.transform.localRotation = Quaternion.Euler(angles);
        // update dummy
        dummy.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

    public void ActivateAimingMode()
    {
        aiming = true;
        Camera.main.transform.position = aimAnchor.transform.position;
    }
}