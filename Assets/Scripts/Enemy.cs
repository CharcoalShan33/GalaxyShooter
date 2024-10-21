using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]
    private float _speed;


    bool switchDirection = true;
     bool canFlipX = true;
     bool canFlipY;
    public enum MoveType { LinearH, LinearV, Sine, Cosine };

    public MoveType currentMovement;

    // frequency is the amount of fluctuation/ how fast the wave moves
    [SerializeField]
    float frequency;

    [SerializeField]
    float amplitude = 1f;
    // amplitude is the height of the wave
 
    // value is the offset or the placement value along the axis.
    [SerializeField]
    float value;
    Vector3 movePos = Vector3.zero;

    [Header("Enemy Types")]

    int randomType;
    public enum EnemyTypes { Normal, Shielded };

    public EnemyTypes currentEnemyType;

    private float defaultSpeed = 1.5f;
    private bool didCollide;
    private Animator _enemyAnim;
    private Player _player;
    private SpriteRenderer rend;
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

    [SerializeField]
    private GameObject _enemyLaser;

    [SerializeField]
    private GameObject _enemyBullet;

    [SerializeField]
    private float _enemyFireRate = 0.5f;


    private float _nextFire;

    [SerializeField]
    private Transform firePos;

     public Vector2 moveA = Vector2.left;
     public Vector2 moveB = Vector2.right;


    private void Start()
    {
        _enemyAnim = GetComponentInChildren<Animator>();

        _audioExplode = GetComponent<AudioSource>();

        spriteShield = _shield.GetComponent<SpriteRenderer>();

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();

        rend = GetComponent<SpriteRenderer>();
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
            Debug.LogError("Find the Sprite Shield Source Component");
        }

        if (_audioExplode == null)
        {
            Debug.LogError("Find the Audio Source Component");
        }
        if (rend == null)
        {
            Debug.LogError("Find the SpriteRenderer Component");
        }

        _enemyFireRate = Mathf.Clamp(_enemyFireRate, 1f, 4f);
        _speed = defaultSpeed;

        currentMovement = MoveType.LinearH;
        // Freeze();

        movePos = transform.position;

        
    }


    private void Update()
    {

        // BasicMovement();


        if (switchDirection)
        {
            //NewMovementH();
            HorizontalMovement();
        }

        if (!switchDirection)
        {
        
            VerticalMovement();
        }

        if (ableToShoot)
        {

            EnemyFire();

        }
       


        switch (currentEnemyType)
        {
            case EnemyTypes.Normal:

               
                break;


            case EnemyTypes.Shielded:

                isShieldActive = true;
                _shield.SetActive(true);
                break;
        }


        switch (currentMovement)
        {
            case MoveType.LinearH:

                switchDirection = true;

                break;


            case MoveType.LinearV:

                switchDirection = false;

                break;


            case MoveType.Sine:

                switchDirection = true;
                movePos = transform.position;
                movePos.y += Mathf.Sin(Time.time * frequency + value) * amplitude * Time.deltaTime;
                transform.position = movePos;
                break;

            case MoveType.Cosine:
                switchDirection = false;
                movePos = transform.position;
                movePos.x += Mathf.Cos(Time.time * frequency + value) * amplitude * Time.deltaTime;
                transform.position = movePos;
                break;
        }

        if(stopMoving)
        {
            Freeze();
        }
        else
        {
            NormalSpeed();
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


    void HorizontalMovement()
    {
        switchDirection = true;
       
        if (canFlipX == true)

        {
            MoveRight(); 

        }
        if (canFlipX == false )
        {
            MoveLeft();

        }

        if (transform.position.x >= 14f)
        {

            canFlipX = false;  
        }

        if (transform.position.x <= -14f)
        {

            canFlipX = true;
        }

    }

    void VerticalMovement()
    {
      
        switchDirection = false;
        
        if (canFlipY == true)

        {
            MoveUp();

        }

       if (canFlipY == false)
        {
           
            MoveDown();
        }


        if (transform.position.y >= 7f)
        {

            canFlipY = false;
          

        }

        if (transform.position.y <= -7f)
        {
            canFlipY = true;
            
        }

    }

   




    private void MoveUp()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.up);
    }

    void MoveDown()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.down);
    }

    void MoveLeft()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.left);
    }

    private void MoveRight()
    {
        transform.Translate(_speed * Time.deltaTime * Vector2.right );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);
           if(currentEnemyType == EnemyTypes.Shielded)
            {
                ActiveShield();
            }
           else
            {
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
            else
            {
                Explosion();
            }
            if (didCollide == false)
            {
                didCollide = true;
                Explosion();
            }
        }
    }

    // void BasicMovement()
    //{

    // transform.Translate(_speed * Time.deltaTime * Vector3.down);
    // }


    
    void ActiveShield()
    {
        if(isShieldActive)
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

               
               currentEnemyType = EnemyTypes.Normal;
            }  
        }
        
    }
    
    void Explosion()
    {
        if(_player != null)
        {
            _player.AddScore(10);

        }
        GetComponent<Collider2D>().enabled = false;
        _enemyAnim.SetTrigger("OnEnemyDeath");
        stopMoving = true;
        _speed = 0f;
        _audioExplode.Play();
        

        Destroy(this.gameObject, 2.4f);
    }


     void Freeze()
    {
        stopMoving = true;
        _speed = 0f;

    }

    void NormalSpeed()
    {
        stopMoving = false;
        _speed = defaultSpeed;
    }

    public void StartFreeze()
    {
        stopMoving = true;
    }

    public void StopFreeze()
    {
        stopMoving = false;
    }

}
