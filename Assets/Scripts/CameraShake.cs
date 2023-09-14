using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraShake : MonoBehaviour
{
    private Vector3 shakePos;

    private Vector3 originalPos;

    private Vector3 shakeRot;


    [SerializeField]
    float duration = 0f; // how long the shake lasts. Lower the number, the shorter the time. 

    [SerializeField]
    float magnitude = .5f; // how strong the shake. value from 0 to 1. IMPACT

    [SerializeField]
    float frequency; // the speed in which the wave happens. Large number, Smaller waves.

    [SerializeField]
    int shakeID;

   

    private void Start()
    {
        shakePos = Vector3.one;
        originalPos = Vector3.zero;
        shakeRot = Vector3.one;
        duration = magnitude = frequency = 0;

 
       

    }

    private void Update()
    {
        shakeID = Mathf.Clamp(shakeID, 1, 2);

        magnitude = Mathf.Clamp01(magnitude);

        frequency = Mathf.Clamp(frequency, 0, 35);

      

        switch (shakeID)
        {
            case 1:
                if (duration > 0)
                {
                    Shake();
                    duration -= Time.deltaTime;
                }

                else
                {
                    transform.localPosition = originalPos;
                }

                break;
            case 2:
                if (duration > 0)
                {
                    ImpactHit();
                    duration -= Time.deltaTime;
                }
                else
                {
                    transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                }
                break;

        }
    }

    void Shake()
    {
        transform.localPosition = new Vector3(shakePos.x * Mathf.PerlinNoise(0.0f, Time.time * frequency), 0, 0) * magnitude ;
        //Transform Local positions for X and Y Axis only.
    }

    void ImpactHit()
    {
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, shakeRot.z * Mathf.PerlinNoise(0f, Time.time * frequency))* magnitude) ;
        // Rotation is for Z axis only.
    }

    public void Tremor(float intensity, float speed, float timeAmount, int type)
    {
        magnitude = intensity;

        frequency = speed;

        duration = timeAmount;

        shakeID = type;
    }

}
