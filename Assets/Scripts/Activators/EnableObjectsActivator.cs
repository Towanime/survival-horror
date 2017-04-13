using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableObjectsActivator : BaseActivator {
    public List<GameObject> toEnable;


    public override void Activate(GameObject trigger)
    {
        foreach (GameObject obj in toEnable)
        {
            obj.SetActive(true);
        }
    }

    public override void Desactivate()
    {
        foreach (GameObject obj in toEnable)
        {
            obj.SetActive(false);
        }
    }
}
