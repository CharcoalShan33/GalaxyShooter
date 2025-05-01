using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemy : MonoBehaviour
{

    private Rigidbody2D rig;
    private Animator anim;

    [Header("General")]
    Player _player;

    AudioSource _audioExplode;
    [SerializeField] GameObject _laser;

    [SerializeField] GameObject _explosion;


    [SerializeField] float _speed = 2f;


    [Header("Range/Dodge")]

    [SerializeField] float raycastRange;
    [SerializeField] float dodgeDelay;





    [Header("Bools")]
    [SerializeField] bool canDodge;
    [SerializeField] bool canSneakAttack;
    [SerializeField] bool stopMoving;


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _audioExplode = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (stopMoving)
        {
            Freeze();
        }
        else
        {
            NormalSpeed();
        }

    }

    private void NormalSpeed()
    {
        stopMoving = false;
        _speed = 2f;
    }

    private void Freeze()
    {
        stopMoving = true;
        _speed = 0f;
    }

    void FixedUpdate()
    {
        rig.velocity = Vector3.down * _speed;
    }


    void DetectPowerUP()
    {

    }

    void DetectLazer()
    {

    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser") || other.CompareTag("Explosive"))
        {
            Destroy(other);
            Death();

        }
        if (other.CompareTag("Player"))
        {
            if (_player != null)
            {
                _player.TakeDamage();
            }
            Death();
        }

       
    }

    public void Death()
    {
        if (_player != null)
        {
            _player.AddScore(10);

        }
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("OnEnemyDeath");
        stopMoving = true;
        _speed = 0f;
        _audioExplode.Play();


        Destroy(this.gameObject, 2.4f);
    }
}
