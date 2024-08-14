using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeamScript : MonoBehaviour
{
    [SerializeField]
    float increase;

    [SerializeField]
    float growthSpeedT;


    // speed
    [SerializeField]
    float growthSpeedA;


    SpriteRenderer spr;
    Color col = Color.clear;

    private float currentA;
    private float desiredA;


    [SerializeField]
    float duration;

    private bool stop;// damage

    bool done;// growing and stopping rays;

    [SerializeField]
    float coolDown;

    [SerializeField]
    int cycleComplete;
    // Start is called before the first frame update

    Player play;
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();

        spr.color = col;

        play = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        growthSpeedA = Mathf.Clamp(growthSpeedA, .01f, .5f);
        growthSpeedT = Mathf.Clamp(growthSpeedT, .5f, 5f);

        col.a = currentA;
        //StartCoroutine(RayCoroutine());

        StartCoroutine(GoRay());


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!done)
        {
            GrowRay();
        }

        if (done)
        {
            StopRay();
        }

        */

        //GrowOutline();


        if(play == null)
        {
            Destroy(gameObject);
        }

        if (increase > 10f)
        {
            increase = 10f;

        }




    }
    IEnumerator RayCoroutine() /// complete one
    {
       
        yield return new WaitForSeconds(coolDown);

        StartCoroutine(GoRay());




    }



    IEnumerator GoRay()
    {
        Debug.Log("GoRay Started");
        while (!done)
        {
            
            GrowRay();
            yield return null;
        }
        this.gameObject.GetComponent<Collider2D>().enabled = true;
        spr.color = Color.red;
        yield return new WaitForSeconds(duration);
        spr.color = Color.green;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        while (done)
        {

            StopRay();
            yield return null;
        }
       

        StartCoroutine(RayCoroutine());
    }


    private void GrowOutline()
    {
        transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(increase, 75f), growthSpeedT * Time.deltaTime);
    }

    void GrowRay()
    {
        desiredA = 1;
        //done = false;
        currentA = Mathf.MoveTowards(currentA, desiredA, growthSpeedA * Time.deltaTime);
        col = new(0f, 1f, 0f, currentA);

        spr.color = col;

        if (currentA == 1)
        {
            done = true;


        }

    }


    void StopRay()
    {
        //done = true;


        desiredA = 0f;
        if (duration <= 0)
        {
            
            currentA = Mathf.MoveTowards(currentA, desiredA, growthSpeedA * Time.deltaTime);
            col = new(0f, 1f, 0f, currentA);
            spr.color = col;

            if (currentA <= 0f)
            {
                done = false;

                currentA = 0;
                duration = 2f;


            }
        }
        else
        {
            duration -= Time.deltaTime;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player play = collision.GetComponent<Player>();
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Hit");
            play.TakeDamage();
        }
    }

    

}
