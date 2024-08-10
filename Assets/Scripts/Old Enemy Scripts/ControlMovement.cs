using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMovement : MonoBehaviour
{
  

    [SerializeField]
    float frequency;

    [SerializeField]
    float amplitude = 1f;
    // amplitude is the height of the wave
    // frequency is the amount of fluctuation/ how fast the wave moves

    Vector3 movePos = Vector3.zero;

    // value is the offset or the placement value along the axis.

    [SerializeField]
    float value;
   

   
    private void Start()
    {
        movePos = transform.position;


        //target = GameObject.FindGameObjectWithTag("Player");
        value = transform.position.y;
    }

  


    public void UpDown()
    {
      
       movePos = transform.position;
        movePos.y += Mathf.Sin(Time.time * frequency + value) * amplitude * Time.deltaTime;
        transform.position = movePos;

    }
    
    public void LeftRight()
    {
         movePos = transform.position;
         movePos.x += Mathf.Cos(Time.time * frequency + value) * amplitude * Time.deltaTime;
         transform.position = movePos;

         
    }



}
