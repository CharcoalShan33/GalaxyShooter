using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPoolItem
{

    public GameObject bulletToPool;
   

    public int amountToCreate;


    public bool addMoreBullets;
}

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("PoolManager is NULL");
            return _instance;
        }

    }
    private void Awake()
    {
        _instance = this;
    }
    
    List<GameObject> pooledObjects;

    [SerializeField]
    List<ObjectPoolItem> objectsToPool;

    [SerializeField]
    GameObject bulletContainer;

  
    void Start()
    {
        pooledObjects = new List<GameObject>();
        foreach(ObjectPoolItem item in objectsToPool)
        {
            for(int i = 0; i <item.amountToCreate; i++)
            {
                GameObject obj = Instantiate(item.bulletToPool);
                obj.transform.parent = bulletContainer.transform;
                obj.SetActive(false);
                pooledObjects.Add(obj);

            }
        }
    }

    public GameObject RetrieveObject(string tag)
    {
        for(int i = 0; i < pooledObjects.Count; i++)
        {
            if (pooledObjects[i].activeInHierarchy == false && pooledObjects[i].tag == tag )
            {
              
                return pooledObjects[i];
            }
        }
        foreach (ObjectPoolItem item in objectsToPool)
        {
            if(item.bulletToPool.tag == tag)
            {
                if(item.addMoreBullets)
                {
                    GameObject obj = Instantiate(item.bulletToPool);
                    obj.SetActive(false);
                    obj.transform.parent = bulletContainer.transform;
                    pooledObjects.Add(obj);
                    return obj;
                }
                else
                {
                    return null;
                }
            }
        }

        return null;
    }
    
}
