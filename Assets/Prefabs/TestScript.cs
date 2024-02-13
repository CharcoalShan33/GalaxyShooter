using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public interface TestScript
{
    void Execute();
    void Undo();
    bool Required(bool isSet, Color col);
}
