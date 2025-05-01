using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BombScript : MonoBehaviour

{

    [Header("Explosions")]
    [SerializeField] float explosionRadius; // range of the blast
    [SerializeField] GameObject explosionObject; // particle explosion

    [Header("Size and Effects")]
    [SerializeField] float blinkTime; // time when the bomb is going to blink
    private float minSize = 0.5f;
    private float maxSize = 0.6f;
    //[SerializeField] float expandSpeed = .01f;
    private bool expand;
    SpriteRenderer spr;
    private Rigidbody2D rig;
    Vector3 origin;
    Player _player;


    public float distance; // the value of the position of the player is to the object

    // Start is called before the first frame update
    void Start()
    {

        // possibily add a count down.
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        origin = transform.position;
        //set scale
        transform.localScale = new(minSize, minSize, minSize);
        StartCoroutine(Detonate());
        spr = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {


        if (expand)
        {
            transform.localScale = new(maxSize, maxSize, maxSize);

        }
        else
        {
            transform.localScale = new(minSize, minSize, minSize);
        }
    }

    public IEnumerator Detonate()
    {
        // for each 25 seconds, it will loop between expanding, blinking colors and not expanding.
        // until the max amount of time... which is blink time.

        for (float i = 0; i < blinkTime; i += 1.25f)
        {
            // we are expanding and not every .25 sec;
            expand = !expand;
            //we will also blink for that time.
            StartCoroutine(ColorBlink());
            yield return new WaitForSeconds(1.20f);
            // this is 
        }

        yield return new WaitForSeconds(1.5f);


        RaycastHit2D hit = Physics2D.CircleCast(transform.position, explosionRadius, Vector2.down, distance, layerMask: 6);
        //float minDistance = Mathf.Infinity;
        if (hit.collider != null)
        {



            if (hit.collider.CompareTag("Player")) // hitLayer
            {


                Destroy(hit.collider.gameObject);
                Instantiate(explosionObject, transform.position, transform.rotation);
                Destroy(gameObject);
                if (_player != null)
                {
                    // _player.InstantDeath();
                    _player.TakeDamage();
                }

            }

        }
        Instantiate(explosionObject, transform.position, transform.rotation);
        // no audio due to the explosion object having audio already
        Destroy(gameObject);

    }

    IEnumerator ColorBlink()
    {

        yield return new WaitForSeconds(1.1f);
        spr.color = Color.yellow;
        yield return new WaitForSeconds(1.1f);
        spr.color = Color.red;
        yield return new WaitForSeconds(1.1f);
        spr.color = Color.blue;
        yield return new WaitForSeconds(1.1f);
        spr.color = Color.white;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }


}
