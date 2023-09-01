using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [SerializeField] Material mat;
    float value;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    void Update()
    {
        value -= Time.deltaTime;
        mat.mainTextureOffset = new Vector2(1,value);
    }
}
