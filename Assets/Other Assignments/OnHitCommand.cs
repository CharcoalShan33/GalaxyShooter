using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnHitCommand : TestScript
{
    private GameObject _obj;
    //private SpriteRenderer _obj;
    private Color _color;
    private Color _previous;

    Color requiredColor = Color.red;
    bool isRequired = false;

    
    public OnHitCommand(GameObject sprite, Color col)
    {
        _obj = sprite;
        _color = col;
    }

    public void Execute()
    {
        _previous = _obj.GetComponent<SpriteRenderer>().color;


        _obj.GetComponent<SpriteRenderer>().color = _color;
    }

    public bool Required(bool isSet, Color col)
    {
        isRequired = isSet;

        _obj.GetComponent<SpriteRenderer>().color = requiredColor;
        requiredColor = col;
        return true;
    }

    public void Undo()
    {
        _obj.GetComponent<SpriteRenderer>().color = _previous;
    }
    
}
