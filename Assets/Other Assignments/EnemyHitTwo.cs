using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitTwo : MonoBehaviour
{

    Color correctColor = Color.green;



    TriggerTwo trig;

    Color randomCol;


    public int value;

    private void Start()
    {
        correctColor = GameObject.FindGameObjectWithTag("Puzzle").GetComponent<SpriteRenderer>().color;

        randomCol = GameObject.FindGameObjectWithTag("Puzzle").GetComponent<SpriteRenderer>().color;
        trig = GameObject.Find("NewOne").GetComponent<TriggerTwo>();
    }


    private void Update()
    {
        switch (value)
        {

            case 1:
                correctColor = Color.green;
                break;
            case 2:
                randomCol = Color.red;
                break;
            case 3:
                randomCol = Color.blue;
                break;
            case 4:
                randomCol = Color.yellow;
                break;

        }
           if(trig.IsCorrect())
        {
            enabled = false;
        }
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
                if (value == 1)
                {
                    
                    Debug.Log("Change" + hit.collider.name);
                    hit.transform.GetComponent<SpriteRenderer>().color = correctColor;

                    //test.Execute();
                    //PuzzleManager.Instance.AddCommand(test);

                    trig.IsCorrect();
                   
                    //trig.PuzzleCount()
                    
                    //hit.collider.enabled = false;

                }
                else if(value != 1)
                {
                    hit.transform.GetComponent<SpriteRenderer>().color = randomCol;


                }

            }


        }

    }
    
}
