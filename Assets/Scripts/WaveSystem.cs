using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [System.Serializable]
    public class Wave
   {
       
       
       public GameObject enemy;
  
       public int count;
   
       public float spawnRate;
     
       public string name;
    }

    public Wave[] waves;

    int nextWave = 0;

    [SerializeField]
    float timeBtwnWaves = 5f;


    [SerializeField]
    float countdown;

    bool spawning = false;


    void Start()
    {
        countdown = timeBtwnWaves;
    }


    void Update()
    {
        

        if(countdown <= 0)
        {
            if(!spawning)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
                
            }
        }
        else
        {
            countdown -= Time.deltaTime;
        }
    }

    bool isEnemyAlive()
    {
        if(GameObject.FindGameObjectsWithTag("Enemy") == null)
        {
            return false;
        }
        return true;
    }

   IEnumerator SpawnWave(Wave newWave)
    {
        spawning = true;

        for(int i = 0; i < newWave.count; i++)
        {
            SpawnEnemies(newWave.enemy);
            yield return new WaitForSeconds(1f / newWave.spawnRate);
        }

        spawning = false;

        yield break;

    }
    void SpawnEnemies(GameObject obj)
    {
        Debug.Log("Enemy Here" + obj.name);
    }
}


