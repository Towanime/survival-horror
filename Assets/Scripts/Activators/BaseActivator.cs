using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BaseActivator : MonoBehaviour
{
    /// <summary>
    /// Is it active or not?
    /// </summary>
    public bool active;

    public virtual void Activate(GameObject trigger)
    {
        // default behaviour
        active = true;
    }

    public virtual void Desactivate()
    {
        active = false;
    }

}
