using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : MonoBehaviour
{
    [SerializeField]
    private float _enSpeed = 5.0f;

    [SerializeField]
    int shieldHit;

    [SerializeField] GameObject _shield;

    SpriteRenderer spriteShield;
    Color lowShield;

    private Animator _enemyAnim;

    bool isShieldActive;

    private Player _player;

    private AudioSource _audioExplode = null;

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
        _shield.SetActive(true);
        isShieldActive = true;
    }

    private void Update()
    {
        BasicMovement();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other);
            ActiveShield();    
        }

        if (other.CompareTag("Player"))
        {
            if (other != null)
            {
                _player.TakeDamage();
            }
            ActiveShield();
        }
    }

    void BasicMovement()
    {

        transform.Translate(_enSpeed * Time.deltaTime * Vector3.down);
    }

    void ActiveShield()
    {
        if(isShieldActive)
        {
            shieldHit--;

            lowShield = spriteShield.color;
            lowShield.a -= .35f;
            spriteShield.color = lowShield;
            
            if (shieldHit <= 0)
            {
                isShieldActive = false;
                _shield.SetActive(false);

            }
        }
        else
        {
           Explosion();
        }  
    }

 void Explosion()
    {
        if (_player != null)
        {
            _player.AddScore(30);
        }
        _enemyAnim.SetTrigger("OnEnemyDeath");
        _enSpeed = 0f;
        _audioExplode.Play();
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2.4f);
    }
}
