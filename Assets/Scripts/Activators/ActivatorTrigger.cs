using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorTrigger : MonoBehaviour
{
    /// <summary>
    /// Activator to turn on/off.
    /// </summary>
    public BaseActivator activator;


    public void OnTriggerEnter(Collider other)
    {
        activator.Activate(gameObject);
    }

    public void OnTriggerStay(Collider other)
    {
        activator.Activate(gameObject);
    }

    public void OnTriggerExit(Collider other)
    {
        activator.Desactivate();
    }
}