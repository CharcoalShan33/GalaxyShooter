using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private int powerUpId;

    [SerializeField]
    private AudioClip _pClip;

    private Rigidbody2D rig;

    Player _player;
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        
        if(rig == null)
        {
            Debug.LogError("This Component is NULL.");
        }

        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    private void Update()
    {
        if(_player.isMagnetActive)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);

            if(powerUpId == 6 )
            {
                //transform.position = 
            }
        }
        else if(_player.isMagnetActive == false)
        { transform.Translate(Vector2.down * _speed * Time.deltaTime);
            if (transform.position.y < -9.3f)
            {
                Destroy(this.gameObject);
            }
        }
    }
  
    private void OnTriggerEnter2D(Collider2D other)
    {
        AudioSource.PlayClipAtPoint(_pClip, transform.position);

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                switch (powerUpId)
                {
                    case 0:
                        player.TripleShotActive();
                        Debug.Log("Collected TripleShot");
                        break;

                    case 1:
                        player.SpeedBoostActive();
                        Debug.Log("Collected SpeedBoost");
                        break;

                    case 2:
                        player.ShieldBoostActive();
                        player.AddShieldStrength(3);
                        Debug.Log("Collected ShieldBoost");
                        break;

                    case 3:
                        player.AddLives();
                        Debug.Log("Gained Health");
                        break;

                    case 4:
                        player.AddAmmo(player.maxReserve);
                        Debug.Log("Gained more ammo");
                        break;

                    case 5:
                        player.NewFireActive();
                        Debug.Log("The.....The Cannon!");
                        break;

                    case 6:
                        player.Negated();
                       
                        break;
                    case 7:
                        Debug.Log("Homing Missle!");
                        break;

                    case 8:
                        Debug.Log("Magnet Activated");
                        break;
                }
                
            }
                
        }
       Destroy(gameObject);
        
    }
}
