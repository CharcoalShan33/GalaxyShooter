using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BombScript : MonoBehaviour

{
    [Header("Explosions")]
    [SerializeField] float explosionRadius; // were the area is going to hit
    [SerializeField] GameObject explosionObject; // particle explosion

    [Header("Size and Effects")]
    [SerializeField] float blinkTime; // time when the bomb is going to blink
    private float minSize = 0.5f;
    private float maxSize = 0.6f;
    //[SerializeField] float expandSpeed = .01f;
    private bool expand;
    SpriteRenderer spr;

    [SerializeField] float speed;
    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {


        //set scale
        transform.localScale = new(minSize, minSize, minSize);
        StartCoroutine(Explode());
        spr = GetComponent<SpriteRenderer>();
        rig = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    void Update()
    {
        rig.velocity = Vector3.up * speed;

        if (expand)
        {
            transform.localScale = new(maxSize, maxSize,maxSize);
            //transform.localScale = new(Mathf.MoveTowards(minSize, maxSize, expandSpeed), Mathf.MoveTowards(minSize, maxSize, expandSpeed), Mathf.MoveTowards(minSize, maxSize, expandSpeed));
            //transform.localScale = new(Mathf.MoveTowards(minSize, maxSize, expandSpeed * Time.deltaTime), Mathf.MoveTowards(minSize, maxSize, expandSpeed * Time.deltaTime), Mathf.MoveTowards(minSize, maxSize, expandSpeed * Time.deltaTime));
            //transform.localScale = new(Mathf.Lerp(minSize, maxSize, expandSpeed), Mathf.Lerp(minSize, maxSize, expandSpeed), Mathf.Lerp(minSize, maxSize, expandSpeed));
            //transform.localScale = new(Mathf.Lerp(minSize, maxSize, expandSpeed * Time.deltaTime), Mathf.Lerp(minSize, maxSize, expandSpeed * Time.deltaTime), Mathf.Lerp(minSize, maxSize, expandSpeed * Time.deltaTime));
        }
        else
        {
            transform.localScale = new(minSize, minSize,minSize);
            //transform.localScale = new(Mathf.MoveTowards(maxSize, minSize, expandSpeed), Mathf.MoveTowards(maxSize, minSize, expandSpeed), Mathf.MoveTowards(maxSize, minSize, expandSpeed));
            //transform.localScale = new(Mathf.MoveTowards(maxSize, minSize, expandSpeed * Time.deltaTime), Mathf.MoveTowards(maxSize, minSize, expandSpeed * Time.deltaTime), Mathf.MoveTowards(maxSize, minSize, expandSpeed * Time.deltaTime));
            // transform.localScale = new(Mathf.Lerp(maxSize, minSize, expandSpeed), Mathf.Lerp(maxSize, minSize, expandSpeed), Mathf.Lerp(maxSize, minSize, expandSpeed));
            //transform.localScale = new(Mathf.Lerp(maxSize, minSize, expandSpeed * Time.deltaTime), Mathf.Lerp(maxSize, minSize, expandSpeed * Time.deltaTime), Mathf.Lerp(maxSize, minSize, expandSpeed * Time.deltaTime));
        }
    }

    public IEnumerator Explode()
    {
        // for each 25 seconds, it will loop between expanding, blinking colors and not expanding.
        // until the max amount of time... which is blink time.

        for (float i = 0; i < blinkTime; i += .25f)
        {
            // we are expanding and not every .25 sec;
            expand = !expand;
            //we will also blink for that time.
            StartCoroutine(ColorBlink());
            yield return new WaitForSeconds(.20f);
            // this is 
        }

        yield return new WaitForSeconds(.5f);
        Vector3 origin = new(0,0,0);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(origin, explosionRadius );

        foreach(Collider2D cols in colliders)
        {
            if(cols.CompareTag("Enemy") || cols.CompareTag("Meteor"))
            {
                Destroy(cols.gameObject);
                Instantiate(explosionObject, transform.position,transform.rotation);
                Destroy(gameObject);
            }
        }

         Instantiate(explosionObject, transform.position,transform.rotation);
        Destroy(gameObject);

    }

    IEnumerator ColorBlink()
    {

        yield return new WaitForSeconds(.1f);
        spr.color = Color.yellow;
        yield return new WaitForSeconds(.1f);
        spr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        spr.color = Color.blue;
        yield return new WaitForSeconds(.1f);
        spr.color = Color.white;
    }

void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawSphere(transform.position, explosionRadius);
}


}
