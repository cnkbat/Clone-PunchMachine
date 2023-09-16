using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShaker : MonoBehaviour
{
    public static CameraShaker instance {get; private set;}

    [SerializeField] float shakeTimer, shakeIntensity;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }
    void Start()
    {
        StartCoroutine(StopShake(0));
    }

    // Update is called once per frame

    public void StartShake()
    {
        CinemachineBasicMultiChannelPerlin cbmcp = 
            GameManager.instance.currentCam.GetComponent<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        cbmcp.m_AmplitudeGain = shakeIntensity;

        StartCoroutine(StopShake(shakeTimer));
    }

    IEnumerator StopShake(float timer)
    {
        yield return new WaitForSeconds(timer);

        CinemachineBasicMultiChannelPerlin cbmcp = 
            GameManager.instance.currentCam.GetComponent<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        
        cbmcp.m_AmplitudeGain = 0f;

    }
}
