using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _enSpeed = 5.0f;

    [SerializeField]
    int moveID;



    private Player _player;

    private Animator _enemyAnim;

    private AudioSource _audioExplode = null;

    [Header("Enemy Fire")]

    [SerializeField]
    private GameObject _enemyBullet;

    [SerializeField]
    private float _enemyFireRate = 0.5f;

    private float _nextFire;

    [SerializeField]
    private Transform firePos;

    [Header("Search")]

    [SerializeField]
    private GameObject _playerTransform;

    [Header("Direction")]

    private Vector3 _enemyDirection;

    private float _enemyDist;

    private float _angle;

    private int moveDefault = 3;

    Rigidbody2D rig;

    ControlMovement ctrlMove;

    //this was to prevent an object for colliding twice.
    bool didCollide;

    // Start is called before the first frame update

    [SerializeField]
    float delayTime;

    private void Awake()
    {


     _playerTransform = GameObject.FindWithTag("Player");
    }
    void Start()
    {

        ctrlMove = GetComponent<ControlMovement>();
        _enemyAnim = GetComponentInChildren<Animator>();

        _audioExplode = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();
        rig = GetComponent<Rigidbody2D>();
        gameObject.SetActive(true);

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

        if (ctrlMove == null)
        {
            Debug.LogError("Find the Movement Component");
        }

        if (rig == null)
        {
            Debug.LogError("Find the Rigidbody");
        }
        ctrlMove.enabled = true;
        moveID = moveDefault;

    }
    // Update is called once per frame
    void Update()
    {



        //BasicMovement();
        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-14f, 14f);
          

            transform.position = new Vector2(randomX, 7);
        }



        else if (transform.position.x > 14)
        {
            transform.position = new Vector2(-14, 0);
        }

        else if (transform.position.x < -14)
        {
            transform.position = new Vector2(14, 0);
        }


    
        EnemyFire();
    }

    private void FixedUpdate()
    {

        switch (moveID)
        {


            case 1:
                ctrlMove.UpDown();
                OtherMovement();
                break;
            case 2:
                ctrlMove.LeftRight();
                BasicMovement();
                break;

            case 3:
                BasicMovement();
                break;

        }



    }

    void BasicMovement()
    {
        //rig.velocity = new Vector3(0, -1 * _enSpeed * Time.fixedDeltaTime, 0);

        transform.Translate(_enSpeed * Time.fixedDeltaTime * Vector3.down);
    }
    void OtherMovement()
    {
        //rig.velocity = new Vector3(1 * _enSpeed * Time.fixedDeltaTime, 0,0);
        transform.Translate(_enSpeed * Time.fixedDeltaTime * Vector3.right);

    }


    public void MoveDirection(int move)
    {
        moveID = move;
    }

    void EnemyFire()
    {


        if (Time.time > _nextFire)
        {

            _nextFire = Time.time + _enemyFireRate;

            GameObject _eLaser = Instantiate(_enemyBullet, transform.position, Quaternion.identity);

        }
    }




    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("Player") && didCollide == false)
        {

            Player player = other.transform.GetComponent<Player>();


            if (player != null)
            {
                _player.TakeDamage();
            }

            didCollide = true;

            ctrlMove.enabled = false;
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _enSpeed = 0f;
            _audioExplode.Play();
            Destroy(this.gameObject, 2.4f);

        }

        else if (other.CompareTag("Laser"))
        {

            Destroy(other);


            if (_player != null)
            {
                _player.AddScore(10);
            }

            Destroy(GetComponent<Collider2D>());
            ctrlMove.enabled = false;
            _enemyAnim.SetTrigger("OnEnemyDeath");
            _enSpeed = 0f;
            _audioExplode.Play();

            Destroy(this.gameObject, 2.4f);


        }



    }



}
