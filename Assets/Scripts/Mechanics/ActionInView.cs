﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionInView : MonoBehaviour {
    public float maxDistance = 4;
    public Text lblMessage;
    private Activation target;
    private bool inSight;
    private Camera camera;

    

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private Ray getCameraRay()
    {
        Transform cameraTransform = camera.transform;
        return new Ray(cameraTransform.position, cameraTransform.forward);
    }

    private void DetectTarget()
    {
        RaycastHit hit;
        Ray ray = getCameraRay();
        if (Physics.Raycast(ray, out hit) && maxDistance >= hit.distance &&
            (hit.collider.gameObject.CompareTag("Activable")))
        {
            target = hit.collider.gameObject.GetComponent<Activation>();
            if (target)
            {
                lblMessage.text = target.text;
                inSight = true;
            }
        }
        else
        {
            lblMessage.text = "";
            target = null;
            inSight = false;
        }
    }

    public bool InSight()
    {
        return inSight;
    }

}