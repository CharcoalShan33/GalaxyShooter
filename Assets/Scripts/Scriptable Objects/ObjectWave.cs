using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newWave.asset",menuName = "ScriptableObjects/new Wave")]
public class ObjectWave : ScriptableObject
{
    public List<GameObject> objects;
}
