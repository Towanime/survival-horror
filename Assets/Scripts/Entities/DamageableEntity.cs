using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageableEntity : MonoBehaviour
{
    public bool ignoreDamage = false;
    public float life;
    public GameObject checkpoint;
    public bool destroyOnDeath;
    public BaseActivator onDeathActivator;
	public float currentLife;

    void Start()
    {
        currentLife = life;
    }

    public virtual bool OnDamage(GameObject origin, float damage, float delayDeath = 0)
    {
        //Debug.Log("Damage on object: " + gameObject.name);
        if (ignoreDamage) return false;
        ModifyCurrentLife(damage);
        if (currentLife <= 0)
        {
            //Debug.Log("Death for: " + gameObject.name);
            Invoke("OnDeath", delayDeath);
        }
        return true;
    }

    protected virtual void ModifyCurrentLife(float damage)
    {
        currentLife = Mathf.Max(currentLife - damage, 0);
    }

    protected virtual void OnDeath()
    {
        if (checkpoint)
        {
            Refresh();
            gameObject.transform.position = checkpoint.transform.position;
        }
        if (destroyOnDeath)
        {
            Destroy(gameObject);
        }
        if (onDeathActivator)
        {
            onDeathActivator.Activate(gameObject);
        }
    }

    public virtual void Refresh()
    {
        currentLife = life;
    }

    public virtual void SetCheckpoint(GameObject checkpoint)
    {
        this.checkpoint = checkpoint;
    }

    public float CurrentLife
    {
        get { return currentLife; }
    }

    public float Life
    {
        get
        {
            return life;
        }
        set
        {
            life = value;
            currentLife = value;
        }
    }
}
