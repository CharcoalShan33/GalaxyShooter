using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    /// <summary>
    /// This will have the behaviors: Agressive, Dodging, attacking from back
    /// Switch Movements By ID
    /// Death is Here with score
    /// </summary>
    
    [Header("Default Settings")]

    [SerializeField] int moveID;

    [SerializeField] float _enemySpeed;


    [Header("Behavior")]
    [SerializeField] bool canShoot;
    [SerializeField] bool canDodge;
    [SerializeField] bool isAggressive;

    [Header("In Range")]
    [SerializeField] float inAttackRange; // for dashing using circle raycasts
    [SerializeField] float inShootRange; // for hitting pickups /// line range

    [SerializeField] float dodgeRange; // for dodging using circle raycasts

    [SerializeField] float hitRange; // for smart hitting



    private Rigidbody2D rig;
   
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        moveID = Mathf.Clamp(moveID, 0, 5);
        switch(moveID)
        {
            case 0: 
            BasicMovement();
            break;

            case 1:
            
        /// horizontal movement()

            break;

            case 2:
        
        // horizontal Sine
           
            break;

            case 3:

        // vertical cosine
            break;


        }
        
        BasicMovement();

    }


  

    public void BasicMovement()
    {
       rig.velocity = _enemySpeed * Time.fixedDeltaTime * Vector3.down;
    }

    private void Dash()
    {
      moveID = 0;

    }

    public void StopMovement()
    {
        _enemySpeed = 0f;
        rig.velocity = Vector3.zero;
    }
}
