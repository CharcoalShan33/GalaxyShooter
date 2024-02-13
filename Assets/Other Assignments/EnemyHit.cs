using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    
    [SerializeField]
    Color rColor;

    private void Start()
    {
        rColor = GameObject.FindGameObjectWithTag("Puzzle").GetComponent<SpriteRenderer>().color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            //Destroy(gameObject);


            Debug.Log("Hit");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.up), 40.0f);
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector2.up) * 10f, Color.red);


           // TestScript test = new OnHitCommand(hit.collider.gameObject, new Color(color.r, 0, 0));


            if (hit.collider.tag == "Puzzle")
            {
               
                    Debug.Log("Change" + hit.collider.name);
                     hit.transform.GetComponent<SpriteRenderer>().color = rColor;

                    //test.Execute();
                    //PuzzleManager.Instance.AddCommand(test);


                    TriggerDoor trig = GameObject.Find("Door").GetComponent<TriggerDoor>();
                    trig.PuzzleCount();
                



            }
        }


    }
}
