using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _speed;

    /// bool canFlip;
    //int randomMovement;

    // public enum moveType { vWave, hWave, };
    //public moveType currentMovement;

    [Header("Enemy Types")]

    int randomType;

    public enum EnemyTypes { Normal, Shielded };

    public EnemyTypes currentEnemyType;



    private float defaultSpeed = 1.5f;
    private bool didCollide;
    private Animator _enemyAnim;
    private Player _player;
    private AudioSource _audioExplode = null;
    private bool stopMoving;
    [Header("Shield")]

    [SerializeField]
    int shieldHit;

    [SerializeField] GameObject _shield;

    SpriteRenderer spriteShield;
    Color lowShield;
    bool isShieldActive;

    [Header("Enemy Fire")]

    [SerializeField]
    private bool ableToShoot;

    public bool isBeam;

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private GameObject _enemyBullet;

    [SerializeField]
    private float _enemyFireRate = 0.5f;


    private float _nextFire;

    [SerializeField]
    private Transform firePos;




    private void Start()
    {
        _enemyAnim = GetComponentInChildren<Animator>();

        _audioExplode = GetComponent<AudioSource>();

        spriteShield = _shield.GetComponent<SpriteRenderer>();

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();


        if (_enemyAnim == null)
        {
            Debug.LogError("Add an Animator component");
        }
        if (_player == null)
        {
            Debug.LogError("Find the GameObject");
        }

        if (spriteShield == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }

        if (_audioExplode == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }
        _enemyFireRate = Mathf.Clamp(_enemyFireRate, 1f, 4f);
        _speed = defaultSpeed;

        Freeze();
    }


    private void Update()
    {
        BasicMovement();

        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-14f, 14f);
            transform.position = new Vector2(randomX, 7);
        }

        if (ableToShoot)
        {
            if (!isBeam)
            {
                EnemyFire();
            }
            else if (isBeam)
            {
                StartCoroutine(LaserBeam());
            }
        }

        switch (currentEnemyType)
        {
            case EnemyTypes.Normal:

                isShieldActive = false;
                _shield.SetActive(false);
                break;


            case EnemyTypes.Shielded:
                isShieldActive = true;
                _shield.SetActive(true);
                break;
        }
    }

    void EnemyFire()
    {


        Vector3 offset = new(0f, -0.9f, 0f);

        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _enemyFireRate;
            Instantiate(_enemyBullet, firePos.position + offset, Quaternion.identity);
        }



    }

    IEnumerator LaserBeam()
    {

        _enemyLaser.SetActive(false);
        yield return new WaitForSeconds(_enemyFireRate);

        Vector3 offset = new(0f, -0.9f, 0f);
        _enemyLaser.SetActive(true);
        Instantiate(_enemyLaser, firePos.position + offset, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);
            if (currentEnemyType == EnemyTypes.Shielded)
            {

                ActiveShield();
            }
            else
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                Explosion();
            }
        }

        if (other.CompareTag("Player"))
        {

            if (other != null)
            {
                _player.TakeDamage();
            }

            if (currentEnemyType == EnemyTypes.Shielded)
            {
                ActiveShield();
            }
            else if (didCollide == false)
            {
                didCollide = true;
                Explosion();
            }
        }
    }

    void BasicMovement()
    {

        transform.Translate(_speed * Time.deltaTime * Vector3.down);
    }



    void ActiveShield()
    {
        if (isShieldActive)
        {
            shieldHit--;

            lowShield = spriteShield.color;
            lowShield.a -= .35f;
            spriteShield.color = lowShield;

            if (shieldHit <= 0)
            {
                isShieldActive = false;
                _shield.SetActive(false);
                currentEnemyType = EnemyTypes.Normal;

                if (_player != null)
                {
                    _player.AddScore(5);
                }

            }
        }

    }

    void Explosion()
    {
        GetComponent<Collider2D>().enabled = false;
        _enemyAnim.SetTrigger("OnEnemyDeath");
        stopMoving = true;
        _speed = 0f;
        _audioExplode.Play();
        Destroy(this.gameObject, 2.4f);
    }


    public void Freeze()
    {
        stopMoving = true;
        _speed = 0f;

    }

    public void NormalSpeed()
    {
        stopMoving = false;
        _speed = defaultSpeed;
    }

}
