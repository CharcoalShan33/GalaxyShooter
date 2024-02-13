using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PuzzleManager : MonoBehaviour
{
    static PuzzleManager _instance;

    public static PuzzleManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("NULL!");
            }
            return _instance;
        }
    }

    private List<TestScript> _commandList = new List<TestScript>();

    private void Awake()
    {
        _instance = this;
    }

    

    public void AddCommand(TestScript test)
    {
        _commandList.Add(test);
    }

    public void PlayAll()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        foreach(var command in _commandList)
        {
            command.Execute();
            yield return new WaitForSeconds(1.0f);
        }
        Debug.Log("Finished");

    }

    public void RewindRoutine()
    {
        StartCoroutine(Rewind());
    }
    IEnumerator Rewind()
    {
        foreach(var command in Enumerable.Reverse(_commandList))
        {
            command.Undo();
            yield return new WaitForSeconds(1.0f);
        }
        Debug.Log("Reversed");
        SetColor();
    }

    public void Done()
    {
        var spritesPuzzles = GameObject.FindGameObjectsWithTag("Puzzle");
        foreach(var spriteObj in spritesPuzzles)
        {
            spriteObj.GetComponent<SpriteRenderer>().color = Color.white;
        }
        Debug.Log("We are Done");
    }
    public void ResetAll()
    {
        _commandList.Clear();
        Debug.Log("Reset");
    }
    public void SetColor()
    {
        foreach (var command in _commandList)
        {
            command.Required(true, Color.red);
           
        }
    }
}
