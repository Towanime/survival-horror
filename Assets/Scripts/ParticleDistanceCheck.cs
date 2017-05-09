using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDistanceCheck : MonoBehaviour
{
    public GameObject emitter;

    public void OnTriggerEnter(Collider other)
    {
        emitter.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        emitter.SetActive(false);
    }
}
