using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Tooltip("Current key configuration.")]
    public KeyboardMouseConfig config;
    [Tooltip("Game object that will be used to know where the camera is facing.")]
    public GameObject cameraAnchor;
    [Tooltip("Direction to where the player will move next.")]
    public Vector3 direction;
    [Tooltip("The first direction detected without merging multiple orientations.")]
    public Vector3 rawDirection;
    [Tooltip("Rotation from the mouse to apply on the camera.")]
    public Vector3 rotation;
    public bool action;
    public bool crystal;

    void Update()
    {
        // update values depending on the input
        this.SetDirection();
        this.SetRotation();
        this.SetActions();
    }

    private void SetDirection()
    {
        // merge these vars later
        this.direction = Vector3.zero;

        if (Input.GetKey(this.config.forward))
        {
            this.direction += Vector3.forward;
        }
        else if (Input.GetKey(this.config.backwards))
        {
            this.direction += Vector3.back;
        }

        if (Input.GetKey(this.config.left))
        {
            this.direction += Vector3.left;
        }
        else if (Input.GetKey(this.config.right))
        {
            this.direction += Vector3.right;
        }
        this.direction = this.direction.normalized;
    }

    private void SetRotation()
    {
        float yaw = Input.GetAxis("Mouse X") * this.config.mouseXSensitivity;
        float pitch = Input.GetAxis("Mouse Y") * this.config.mouseYSensitivity;

        if (this.config.invertY)
        {
            pitch *= -1;
        }

        this.rotation = new Vector3(yaw, pitch, 0f);
    }

    private void SetActions()
    {
        this.action = Input.GetKey(this.config.action);
        this.crystal = Input.GetKeyDown(this.config.crystal);
    }    
}
