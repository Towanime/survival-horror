using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterActivator : BaseActivator {
    public int requiredActivations;
    public int currentActivations;
    public BaseActivator activator;


    public override void Activate(GameObject trigger)
    {
        currentActivations = Mathf.Clamp(currentActivations + 1, 0, requiredActivations);
        //Debug.Log(gameObject.name + " Activation - " + currentActivations);
        if (currentActivations >= requiredActivations)
        {
            activator.Activate(gameObject);
        }
    }

    public override void Desactivate()
    {
        currentActivations = Mathf.Clamp(currentActivations - 1, 0, requiredActivations);
        //Debug.Log(gameObject.name + " Desactivation - " + currentActivations);
        if (currentActivations < requiredActivations)
        {
            activator.Desactivate();
        }
    }
}
