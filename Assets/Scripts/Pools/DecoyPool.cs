using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyPool : BasicPool
{
    public static DecoyPool instance;

    void Awake()
    {
        instance = this;
    }
}
