using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemyScript : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField]
    private float _speed;

   
    private Animator _enemyAnim;
    private Player _player;
    private SpriteRenderer rend;
    private AudioSource _audioExplode = null;

    Rigidbody2D rig;

     private bool didCollide;
    private bool stopMoving;

    [SerializeField] bool canShoot;


   [SerializeField] int scoreAmount;

    // Start is called before the first frame update
    void Start()
    {
     
        _enemyAnim = GetComponentInChildren<Animator>();

        _audioExplode = GetComponent<AudioSource>();

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        rend = GetComponent<SpriteRenderer>();

        rig = GetComponent<Rigidbody2D>();

        if (_enemyAnim == null)
        {
            Debug.LogError("Add an Animator component");
        }
        if (_player == null)
        {
            Debug.LogError("Find the GameObject");
        }

        if (_audioExplode == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }
        if (rend == null)
        {
            Debug.LogError("Find the SpriteRenderer Component");
            
        }
        if (rig == null)
        {
            Debug.LogError("Find the Rigidbody2D Component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.down);

        if (stopMoving)
        {
            stopMoving = true;
            _speed = 0f;
        }


    }

     void FixedUpdate()
    {
        rig.velocity = Vector3.down * _speed;
    }

    public void Death()
    {

        if (_player != null)
        {
            _player.AddScore(scoreAmount);

        }
        GetComponent<Collider2D>().enabled = false;
        _enemyAnim.SetTrigger("OnEnemyDeath");
        stopMoving = true;
        _speed = 0f;
        _audioExplode.Play();


        Destroy(this.gameObject, 2.4f);
      
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser") || other.CompareTag("Missile"))
        {
            Destroy(other);


            Death();

        }

         if(other.CompareTag("Bomb"))
        {
            Destroy(other);
            Death();
        }


        if (other.CompareTag("Player"))
        {

            if (other != null)
            {
                _player.TakeDamage();
            }
            // Death();
            if (didCollide == false)
            {
                didCollide = true;
                Death();
            }
        }
    }
}
