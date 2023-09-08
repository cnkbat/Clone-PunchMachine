using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectDMG : DamagableObject, IDamagable
{
    public Transform testTransform;
    public void TakeDamage(float dmg)
    {
        Debug.Log("did hit");
    }

}
