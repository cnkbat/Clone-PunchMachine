using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObjectDMG : MonoBehaviour, IDamagable
{
    public void TakeDamage(float dmg)
    {
        Debug.Log("did hit");
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
