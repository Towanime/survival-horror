using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activation : MonoBehaviour {
    public string text;
    public string toInvoke;
    public bool multipleActivation;
    // 
    private bool activated;

    public void Activate()
    {
        if (multipleActivation || !activated)
        {
            gameObject.SendMessage(toInvoke);
            activated = true;
        }
    }

    public bool CanActivate()
    {
        return multipleActivation || !activated;
    }
}
