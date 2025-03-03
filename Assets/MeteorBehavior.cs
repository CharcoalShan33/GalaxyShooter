using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class MeteorBehavior : MonoBehaviour
{

    [SerializeField] GameObject explodeParticlePrefab;
    private Player _player;


    void Start()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (transform.position.y < -9.3f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);
            Instantiate(explodeParticlePrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {

            if (other != null)
            {
                _player.TakeDamage();
            }
            Instantiate(explodeParticlePrefab, transform.position, transform.rotation);

            Destroy(gameObject);
        }


    }



}
