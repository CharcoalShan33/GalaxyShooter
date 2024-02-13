using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTwo : MonoBehaviour
{


    [SerializeField] GameObject[] puzzles;

    [SerializeField] int puzzleNumber = 6;
    bool isCorrect;

    
    int puzzleAssign;

    [SerializeField] Transform spawnPoint;

    EnemyHitTwo enemTwo;
    // Start is called before the first frame update
    void Start()
    {
       
        puzzles = GameObject.FindGameObjectsWithTag("Puzzle");
       
      
    }

    // Update is called once per frame
    void Update()
    {
        
        foreach (var puzzle in puzzles)

           
            if (isCorrect)
            {
                puzzle.GetComponent<Collider2D>().enabled = false;

                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                
                Destroy(puzzle, 2.0f);

            }
            // puzzle.GetComponent<SpriteRenderer>().color = Color.red;

        
    }

    public void CreatePuzzleGame()
    {
        for (int i = 0; i < puzzleNumber ; i++)
        {
        Instantiate(puzzles[i], spawnPoint.transform.position + new Vector3(i * 2.0f, 0,0), Quaternion.identity);

            Debug.Log("Summon");
            
            if(puzzles.Length == puzzleNumber)
            {
                
                CancelInvoke("CreatePuzzleGame");
                
            }

        }

    }

    public bool IsCorrect()
    {
        return true;
        
    }
}
