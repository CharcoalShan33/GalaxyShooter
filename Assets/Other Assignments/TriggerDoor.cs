using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    int countDoor;
    public int required = 4;
    [SerializeField] GameObject[] puzzles;
    // Start is called before the first frame update
    void Start()
    {
        puzzles = GameObject.FindGameObjectsWithTag("Puzzle");
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var puzzle in puzzles)
        {
            if(countDoor == required)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;

                Destroy(puzzle, 2.0f);
            }
            // puzzle.GetComponent<SpriteRenderer>().color = Color.red;

        }
    }

    public void PuzzleCount()
    {countDoor++;
        
    }
}
