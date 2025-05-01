using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Timers;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class Grenade : MonoBehaviour
{

    [Header("Explosions")]
    [SerializeField] float explosionRadius; // range where the blast originates 
    [SerializeField] float countDown;

    [Header("Effects")]
    [SerializeField] GameObject explosionObject;

    [Header("Movement")]
    
    [SerializeField] float force;
    [SerializeField] float movement;
    public Rigidbody2D rig;
    public SpriteRenderer spr;
  

    float colorChange = .1f;

    Color a1 = Color.white;

    void Start()
    {
        

        spr = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
        StartCoroutine(StartThrow());
        
    }

    private IEnumerator StartThrow()
    {
    yield return new WaitForSeconds(1f);
     var direction = transform.up + Vector3.up;
     rig.AddForce(direction * force);
     yield return new WaitForSeconds(.1f);
     StartCountDown();
    }

    void Update()
    { 
        StartCountDown();
       
    }

    public void StartCountDown()
    {
        if (countDown > 0)
        {
            countDown -= Time.deltaTime;
           a1 = Color.Lerp(Color.white, Color.black,colorChange);
           spr.color = a1;
        }
         if (countDown == 0f)
        {
            Explode();
        }
        if(countDown < 0)
        {
            countDown = 0f;
        }
    }

    void FixedUpdate()
    {
        rig.velocity = movement * Time.deltaTime * Vector3.up;
    }

     void Explode()
    {
        Instantiate(explosionObject, transform.position, transform.rotation);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider != null)
            {
                if ( collider.CompareTag("Enemy"))
                {
                    NewEnemy newOne = collider.GetComponent<NewEnemy>();
                    newOne.Death();
                    
                }
                if (collider.CompareTag("Hazard"))
                {
                    MeteorBehavior meteor = collider.GetComponent<MeteorBehavior>();
                    meteor.ExplodeObject();
                }
            }
        }
        Destroy(gameObject);
    }

}
