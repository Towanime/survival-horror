using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour {

    private ParticleSystem[] particleSystems;
    private float[] emissionRateOverTime;

	void Start () {
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        emissionRateOverTime = new float[particleSystems.Length];
        for (int i=0; i<particleSystems.Length; i++)
        {
            ParticleSystem particleSystem = particleSystems[i];
            emissionRateOverTime[i] = particleSystem.emission.rateOverTime.constant;
        }
    }

    public void Enable()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem particleSystem = particleSystems[i];
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
            rateOverTime.constant = emissionRateOverTime[i];
            emission.rateOverTime = rateOverTime;
        }
    }

    public void Disable()
    {
        for (int i = 0; i < particleSystems.Length; i++)
        {
            ParticleSystem particleSystem = particleSystems[i];
            ParticleSystem.EmissionModule emission = particleSystem.emission;
            ParticleSystem.MinMaxCurve rateOverTime = emission.rateOverTime;
            rateOverTime.constant = 0;
            emission.rateOverTime = rateOverTime;
        }
    }
}
