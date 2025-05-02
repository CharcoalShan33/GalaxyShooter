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
   // private AudioSource _audioExplode;

    Rigidbody2D rig;

    private bool didCollide;
    private bool stopMoving;


    [SerializeField] int scoreAmount;


    [SerializeField]
    private bool ableToShoot;

    [SerializeField]
    private GameObject _enemyBullet;

    [SerializeField] GameObject explodeObject;

    [SerializeField]
    private float _enemyFireRate = 0.5f;

    [SerializeField]
    private Transform firePos;


    private float _nextFire;


    // Start is called before the first frame update
    void Start()

    {


      //  _enemyAnim = GetComponentInChildren<Animator>();

       // _audioExplode = GetComponent<AudioSource>();

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        rend = GetComponent<SpriteRenderer>();

        rig = GetComponent<Rigidbody2D>();

        
        if (_player == null)
        {
            Debug.LogError("Find the GameObject");
        }

       
        if (rend == null)
        {
            Debug.LogError("Find the SpriteRenderer Component");

        }
        if (rig == null)
        {
            Debug.LogError("Find the Rigidbody2D Component");
        }
    

   //Debug.Log($"My audioSource is enabled: {_audioExplode.isActiveAndEnabled}" + $"My audioSource is active: {_audioExplode.gameObject.activeInHierarchy}");

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


        if (ableToShoot)
        {

            EnemyFire();

        }

    }

    void EnemyFire()
    {
        Vector3 offset = new(0f, -0.9f, 0f);
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _enemyFireRate;
            Instantiate(_enemyBullet, firePos.position + offset, Quaternion.identity);
           // _audioExplode.Play();
        }
    }

    void FixedUpdate()
    {
        rig.velocity = Vector3.down * _speed;
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Laser"))
        {
            Destroy(other);
            Death();

        }

        if (other.CompareTag("Explosive"))
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

    public void Death()
    {

        if (_player != null)
        {
            _player.AddScore(scoreAmount);

        }

        //Debug.Log($"My audioSource is enabled: {_audioExplode.isActiveAndEnabled}");
       // Debug.Log($"My audioSource is enabled: {_audioExplode.isActiveAndEnabled}" + $"My audioSource is active: {_audioExplode.gameObject.activeInHierarchy}");
        GetComponent<Collider2D>().enabled = false;
       
        Instantiate(explodeObject, transform.position, Quaternion.identity);

       // _enemyAnim.SetTrigger("OnEnemyDeath");
        stopMoving = true;
        _speed = 0f;
       

       // _audioExplode.Play();

        Destroy(gameObject);


    }


}
