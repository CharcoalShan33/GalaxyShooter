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

    //Player player;

   
    
    GameObject target;

  public static bool seenTarget;


    Vector3 direction;

   // [SerializeField]
    //private GameObject _audio;

    //private AudioManager aManager;
    
    void Start()
    {
        //direction = GameObject.FindGameObjectWithTag("Player").transform.position;
        //aManager = GameObject.Find("Audio_Manager").GetComponent<AudioManager>();

        target = GameObject.FindGameObjectWithTag("Player");
        rig = GetComponent<Rigidbody2D>();

        if(rig == null)
        {
            Debug.LogError("This Component is NULL.");
        }
    }

    // Update is called once per frame


    private void Update()
    {
    

        transform.Translate(Vector2.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }


       if(seenTarget)
        {
            MoveToPlayer();
        }

    }

  
    void MoveToPlayer()
    {
      
       transform.position = Vector3.Lerp(transform.position, target.transform.position, _speed * Time.deltaTime); 
    }

   
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //aManager.PlayClip(_pClip);

        AudioSource.PlayClipAtPoint(_pClip, transform.position);

        //Instantiate(_audio, transform.position, Quaternion.identity);


        if (other.tag == "Player")
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
                        player.RefillAmmo();
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
                }
                
            }

        }
        Destroy(this.gameObject);
        
    }
}
