using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoolObjects
    {

        public GameObject spawnToPool;


        public int amountToCreate;


        public bool addMoreSpawns;
    }
    private static SpawningPool _instance;
    public static SpawningPool Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("SpawningPool is NULL");
            return _instance;
        }

    }
    // overall list of gameobjects that are populated.
    List<GameObject> allPooledObjects;

    // List that will populate the type of objects that are in the SpawnPoolObjects class
    [SerializeField]
    List<SpawnPoolObjects> objectsToPool;

    // a gameobject to hold all spawns
    [SerializeField]
    GameObject spawnContainer;
    private void Awake()
    {
        _instance = this;
    }




    void OnEnable()
    {

        allPooledObjects = new List<GameObject>();
        foreach (SpawnPoolObjects item in objectsToPool)
        {
            for (int i = 0; i < item.amountToCreate; i++)
            {
                GameObject obj = Instantiate(item.spawnToPool);
                obj.transform.parent = spawnContainer.transform;
                obj.SetActive(false);
                allPooledObjects.Add(obj);
            }
        }
    }

    public GameObject RetrieveObject(string tag)
    {
        for (int i = 0; i < allPooledObjects.Count; i++)
        {
            if (allPooledObjects[i].activeInHierarchy == false && allPooledObjects[i].tag == tag)
            {

                return allPooledObjects[i];
            }
        }
        foreach (SpawnPoolObjects item in objectsToPool)
        {
            if (item.spawnToPool.tag == tag)
            {
                if (item.addMoreSpawns)
                {
                    GameObject obj = Instantiate(item.spawnToPool);
                    obj.SetActive(false);
                    allPooledObjects.Add(obj);
                    return obj;
                }


            }

        }


        return null;

    }

}