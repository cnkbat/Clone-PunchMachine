using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] Material mat;
    float value;
    [SerializeField] bool leftPlatformMat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if(leftPlatformMat)
        {
            value += Time.deltaTime * 3;
        }
        else 
        {
            value -= Time.deltaTime;
        }
        mat.mainTextureOffset = new Vector2(1,value);
    }
}
