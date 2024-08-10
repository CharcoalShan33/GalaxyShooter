using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6f;

    private SpriteRenderer rend;

    private Rigidbody2D rig;



   // private Vector3 direction;
    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
     
        transform.Translate(_speed * Time.deltaTime * Vector3.down);

        
        //Destroy(gameObject, 2f);
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            player.TakeDamage();
        }
    }

    private void OnBecameInvisible()
    {
        rend.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
