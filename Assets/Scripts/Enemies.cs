using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    [SerializeField]
    float sightRange;

    float distance;

    private Rigidbody2D rig;

    [SerializeField]
    float speed;

    Vector3 moveDirection;
    // Start is called before the first frame update
    void Awake()
    {
        rig = GetComponent<Rigidbody2D> ();
        if(rig == null)
        {
            Debug.Log("Add the Component.");
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");

    }

    private void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);


        Vector2 direction = (player.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (distance < sightRange)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }
    }

    private void FixedUpdate()
    {
        
           
      
    }

    private void MoveToPlayer()
    {
        
    }
    // Update is called once per frame


    //rig.velocity = Vector3.down * speed * Time.deltaTime;

    /*
        private void Update()
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
            if(distanceToPlayer <= sightRange)
            {
                ChasePlayerVertical();
                //ChasePlayerHorizontal();
            }
            else
            {
                StopChase();
            }

        }
        void ChasePlayerVertical()
        {
            if(transform.position.y < player.position.y)
            {
                rig.velocity = new Vector2(0f, speed);
                transform.localScale = new Vector2(.3f, -.3f);
            }
            else 
            {
                rig.velocity = new Vector2(0f, -speed);
                transform.localScale = new Vector2(.3f, .3f);
            }



        }

        void ChasePlayerHorizontal()
        {

            if (transform.position.x < player.position.x)
            {
                rig.velocity = new Vector2(speed, 0f);
                transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
            }
            else
            {
                rig.velocity = new Vector2(-speed, 0f);
                transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            }

        }

        void StopChase()
        {
            rig.velocity = Vector2.zero;
        }

        */
}
