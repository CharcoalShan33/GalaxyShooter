using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float _enSpeed = 5.0f;

    
    private Player _player;

    private Animator _enemyAnim;

    private AudioSource _audioExplode = null;

    [Header("Enemy Fire")]

    [SerializeField]
    private GameObject _enemyBullet;

    [SerializeField]
    private float _enemyFireRate = 0.5f;

    private float _nextFire;

    [Header("Search")]

    //[SerializeField]
    //private Transform _playerTransform;

    [Header("Direction")]

    private Vector3 _enemyDirection;

    private float _enemyDist;

    private float _angle;


    bool didCollide;

    // Start is called before the first frame update

    private void Awake()
    {
        //_playerTransform = GameObject.FindWithTag("Player").transform;
    }
    void Start()
    {

        _enemyAnim = GetComponentInChildren<Animator>();

        _audioExplode = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

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

    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _enSpeed * Time.deltaTime);

        if (transform.position.y < -7f)
        {
            float randomX = Random.Range(-14f, 14f);


            transform.position = new Vector2(randomX, 7);
        }
        //FacePlayer();
        //EnemyFire();
    }

    void EnemyFire()
    {
     

        if (Time.time > _nextFire)
        {
            
            _nextFire = Time.time + _enemyFireRate;

            Shoot();
        }
    }
    void Shoot()
    {
        

        GameObject _eLaser = Instantiate(_enemyBullet, transform.position, Quaternion.identity);

        Laser _laserMove = _eLaser.GetComponentInChildren<Laser>();

        _laserMove.AssignLaser();

      
        
    }
/*
    void FacePlayer()
    {
        if (_playerTransform != null)
        {
            _enemyDirection = _playerTransform.position - transform.position;
            _angle = Mathf.Atan2(_enemyDirection.y, _enemyDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle + 90f, Vector3.forward);
        }
        
    }
*/

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player")  && didCollide == false)
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                _player.TakeDamage();

            }
            didCollide = true;

            _enemyAnim.SetTrigger("OnEnemyDeath");
            
            _enSpeed = 0f;
            _audioExplode.Play();
            
            Destroy(this.gameObject, 2.4f);
            
        }
   

        else if (other.CompareTag("Laser"))
        {

            Destroy(other.gameObject);

            if (_player != null)
            {
                _player.AddScore(10);
            }

            _enemyAnim.SetTrigger("OnEnemyDeath");
            _enSpeed = 0f;
            _audioExplode.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.4f);

        }


        
    }

}
