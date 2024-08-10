using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissle : MonoBehaviour
{
    [SerializeField]
    float _fireSpeed;

    Rigidbody2D rig;

     GameObject _target;

    [SerializeField]
    Transform _shootPoint;
   
    [SerializeField]
    float _rotateSpeed;

    Vector3 direction;


    Vector2 V2 = new();

    Vector3 V3 = new();

    [SerializeField]
    float _detectRadius;

    [SerializeField]
    LayerMask mask;

    float distance1 = 10f;

   RaycastHit2D[] hits;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();


        V3 = transform.position;

        V2 = new(V3.x, V3.y);


        _shootPoint = GameObject.FindWithTag("Player").transform;


        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var enemy in enemies)
        if (enemy != null)
        {
            _target = enemy;
        }
        else
        {
            Debug.Log("No enemy was found");
        }




        if (rig == null)
        {
            Debug.LogError("This component is null.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmos()
    {
        Color color = Gizmos.color;
        Gizmos.color = Color.red;
        Physics2D.CircleCastAll(transform.position, _detectRadius, Vector2.down, distance1, mask);
        Gizmos.color = color;
    }
    private void FixedUpdate()
    {
     


        if (_target == null)
        {

           hits = Physics2D.CircleCastAll(_shootPoint.transform.position, _detectRadius, Vector2.down, distance1, mask );
           
            foreach(var hit in hits)
            {
                if(hit.collider == CompareTag("Enemy"))
                {

                     _target  = hit.collider.gameObject ;
                }

            }


                //transform.Translate(Vector3.up * _fireSpeed * Time.deltaTime);
                rig.velocity = transform.up * _fireSpeed;
            transform.rotation = Quaternion.identity;
        }
        else
        {
             FollowEnemy();
        }

        if (transform.position.y > 7f || transform.position.y < -7f)
        {
            Destroy(gameObject);
        } 
    }

    void FollowEnemy()
    {
        if (_target != null)
        {
        
          direction = _target.transform.position - rig.transform.position;

            direction.Normalize();
            float rotate = Vector3.Cross(direction, transform.up).z;

            rig.angularVelocity = -_rotateSpeed * rotate;

            rig.velocity = transform.up * _fireSpeed;
            // this will move, so
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {

            Destroy(gameObject);
        }
    }

}
