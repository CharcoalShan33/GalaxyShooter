using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    [SerializeField]
    GameObject target; // who the missle is aming towards;

    [SerializeField]
    float _fireSpeed;

    Rigidbody2D rig;

    [SerializeField]
    float _rotateSpeed;

    Vector2 direction;

   

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        target = GameObject.FindWithTag("Enemy");

        if (rig == null)
        {
            Debug.LogError("This component is null.");
        }
    }

    // Update is called once per frame
    void Update()

    {
         
      

        
    }

    private void FixedUpdate()
    {
        FollowEnemy2();
    }

    void FollowEnemy1()
    {
        //transform.Translate(Vector3.up * _fireSpeed * Time.deltaTime);
        //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, _fireSpeed * Time.deltaTime);
    }
    void FollowEnemy2()
    {
        direction = target.transform.position - transform.position;

        direction.Normalize();

        float rotate = Vector3.Cross(direction, transform.up).z;

        rig.angularVelocity = -rotate * _rotateSpeed;

        rig.velocity = transform.up * _fireSpeed;
        // this will move, so 


        Destroy(this.gameObject, 5f);
    }

   
    

}
