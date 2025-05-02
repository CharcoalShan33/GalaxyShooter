using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum EnemyType { Shielded, Normal }

    public EnemyType currentType;

    [SerializeField] float _enemySpeed;

    [SerializeField] GameObject explodeObject;

    [Header("Shield")]

    [SerializeField]
    int shieldHit;

    [SerializeField] GameObject _shield;

    SpriteRenderer spriteShield;
    Color lowShield;
    bool isShieldActive;


    private Animator _enemyAnim;
    private Player _player;

    private AudioSource _audioExplode; // for shooting

    private EnemyBehavior enemyBehavior;

    private void Start()
    {

        _enemyAnim = GetComponent<Animator>();

        _audioExplode = GetComponentInChildren<AudioSource>();

        spriteShield = _shield.GetComponent<SpriteRenderer>();

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        enemyBehavior = GetComponent<EnemyBehavior>();

        if (_enemyAnim == null)
        {
            Debug.LogError("Add an Animator component");
        }

        if (_audioExplode == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }
        if (spriteShield == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }

        if (_player == null)
        {
            Debug.LogError("Find the GameObject");
        }

        if (enemyBehavior == null)
        {
            Debug.LogError("Get the Enemy Behavior Script");
        }


        if (currentType == EnemyType.Shielded)
        {
            _shield.SetActive(true);
            isShieldActive = true;
            shieldHit = 2;
        }
        if (currentType == EnemyType.Normal)
        {
            _shield.SetActive(false);
            isShieldActive = false;
            shieldHit = 0;

        }

        // _shield.SetActive(true);
        // isShieldActive = true;
    }

    // Update is called once per frame
    void Update()
    {

        enemyBehavior.BasicMovement();


    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);

            if (currentType == EnemyType.Shielded)
            {
                ActiveShield();

            }

            else
            {
                Death();
            }


        }

        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                _player.TakeDamage();
            }
            if (currentType == EnemyType.Shielded)
            {
                ActiveShield();

            }
            else
            {
                Death();
            }

        }

        if (other.CompareTag("Explosive"))
        {

            Destroy(other);


            if (currentType == EnemyType.Shielded)
            {
                ActiveShield();

            }
            else
            {
                Death();
            }

        }
    }

    void ActiveShield()
    {
        if (isShieldActive)
        {
            shieldHit -= 1;

            lowShield = spriteShield.color;
            lowShield.a -= .35f;
            spriteShield.color = lowShield;

            if (shieldHit <= 0)
            {
                shieldHit = 0;
                isShieldActive = false;
                _shield.SetActive(false);
                currentType = EnemyType.Normal;
            }
        }
    }

    public void Death()
    {
        if (_player != null)
        {
            _player.AddScore(30);
        }
        if (gameObject != null)
        {
            GetComponent<Collider2D>().enabled = false;
            Instantiate(explodeObject, transform.position, Quaternion.identity);
            _enemyAnim.SetTrigger("OnEnemyDeath");
            enemyBehavior.StopMovement();
            Destroy(gameObject);
           
           

        }

    }
}
