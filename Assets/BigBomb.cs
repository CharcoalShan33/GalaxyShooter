using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BigBomb : MonoBehaviour
{
    [SerializeField] float explosionRange;
    [SerializeField] float detonateTime; // bombTime for explosion and display

    [SerializeField] float timeToDestroyObject; // actual destroying the bomb game object

    [SerializeField] TMP_Text detonateTimeText;

    [SerializeField] LayerMask hitLayer;


    SpriteRenderer spr;
    Animator anim;
    // float totalDestroyTime; // total time it takes.

    // Start is called before the first frame update
    void Start()
    {   
        detonateTime = Mathf.Clamp(detonateTime, 0f, 600f);
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if(anim == null)
        {
            Debug.Log("Add Animator.");
        }
         if(spr == null)
        {
            Debug.Log("Add SpriteRenderer.");
        }
        // totalDestroyTime = detonateTime + timeToDestroyObject;

        anim.SetBool("isExplode", false);

    }


    // Update is called once per frame
    void Update()
    {
        if (detonateTime <= 0f)
        {
            Destroy(gameObject, timeToDestroyObject);
            detonateTime = 0f;
            Explode();
        }
        else
        {
            if (detonateTime > 0f)
            {
                detonateTime -= Time.deltaTime;
            }

            if (detonateTime <= 120 && detonateTime > 0f)
            {
                anim.SetBool("isExplode", true);
                // all for the bomb
                StartCoroutine(Blink());
            }
        }

        
        /// if boss is dead, disarm this object while there
        /// is time left and set the timer to zero.
        /// also,  destroy the object after a few seconds.


        int minutes = Mathf.FloorToInt(detonateTime / 60);
        int seconds = Mathf.FloorToInt(detonateTime % 60);

        detonateTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);




    }

    private void Explode()
    {
         anim.SetBool("isExplode", true);
          Collider2D col = Physics2D.OverlapCircle(transform.position, explosionRange, hitLayer);

            if (col != null)
            {
                if (col.CompareTag("Player"))
                {
                    col.GetComponent<Player>().InstantDeath();
                }
            }
    }

    private void ExplodeAll()
    {
      
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, explosionRange, hitLayer);

        for (int i = 0; i < col.Length; i++)

            if (col != null)
            {
                if (col[i].CompareTag("Player"))
                {
                    col[i].GetComponent<Player>().InstantDeath();
                }
                if (col[i].CompareTag("Enemy"))
                {
                    col[i].GetComponent<NewEnemy>().Death();
                }
                if (col[i].CompareTag("Hazard"))
                {
                    col[i].GetComponent<MeteorBehavior>().ExplodeObject();
                }

                if(col[i].CompareTag("Boss"))
                {
                    Destroy(col[i]);
                }
                 if(col[i].CompareTag("Collectable"))
                {
                    Destroy(col[i]);
                }



            }
    }

    void Disarm()
    {
        spr.color = Color.grey;
        Destroy(this.gameObject, 1f);
        anim.SetTrigger("Disarm");

    }

    IEnumerator Blink()
    {
        
        while (true)
        {
            
            detonateTimeText.color = Color.yellow;
            yield return new WaitForSeconds(.1f);
            detonateTimeText.color = Color.black;
            yield return new WaitForSeconds(.1f);
            detonateTimeText.color = Color.blue;
            yield return new WaitForSeconds(.1f);
            if (detonateTime <= 0)
            {
                detonateTimeText.color = Color.red;
                yield break;
            }
        }


    }

}
