using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Cast : MonoBehaviour
{
    Color color;
    SpriteRenderer sprite1;
    
    // Start is called before the first frame update
    void Start()
    {

        color = new(Random.value, Random.value, Random.value);
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKey(KeyCode.K))
        {
            PuzzleManager.Instance.PlayAll();
            
        }
        if(Input.GetKey(KeyCode.Tab))
        {

            PuzzleManager.Instance.RewindRoutine();
        }
        if (Input.GetKey(KeyCode.M))
        {
            PuzzleManager.Instance.Done();

        }
        if (Input.GetKey(KeyCode.R))
        {

            PuzzleManager.Instance.ResetAll();
        }

        
    }
}
