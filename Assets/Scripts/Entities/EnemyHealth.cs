using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : DamageableEntity {

    public GameObject deathPrefab;

    protected override void OnDeath()
    {
        Instantiate(deathPrefab, transform.position, transform.rotation);
        base.OnDeath();
    }
}
