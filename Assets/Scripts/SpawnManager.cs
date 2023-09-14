using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Enemy")]

    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _enemyObject;


    [Header("Power-Ups")]
    [SerializeField]
    private GameObject[] _powerUp;

    private bool _isPlayerDead = false;

    float startTimeValue = 3.0f;

    [SerializeField]
    float spawnTime = 2.0f;

    [SerializeField]
    float timer = 0.0f;


    
   
    // Start is called before the first frame update
    void Start()
    {
        //chanceDrop = Random.Range(0, 100);
        enabled = false;
    }

    // Update is called once per frame
    public void Update()
    {
        
        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            RareSpawn();
            timer -= spawnTime;
        }

      

    }
    IEnumerator EnemySpawning()
    {
        yield return new WaitForSeconds(startTimeValue);

        while (_isPlayerDead == false)
        {
            Vector2 enemyPosition = new Vector2(Random.Range(-8f, 8f), 7);
            GameObject NewEnemy = Instantiate(_enemyObject, enemyPosition, Quaternion.identity);
            NewEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);

        }
    }



    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(startTimeValue);
        while (_isPlayerDead == false)
        {
            Vector2 powerPosition = new Vector2(Random.Range(-8f, 8f), 7);

            int randomValue = Random.Range(0, 2);
      
            Instantiate(_powerUp[randomValue], powerPosition, Quaternion.identity);
            

            yield return new WaitForSeconds(Random.Range(3, 5));

        }
    }


    void RareSpawn()
    {

      //  Vector2 powerPosition1 = new Vector2(Random.Range(-8f, 8f), 7);
        //Instantiate(_powerUp[5], powerPosition1, Quaternion.identity);
        
    }




    public void StartSpawning()
    {
        StartCoroutine(EnemySpawning());
        StartCoroutine(SpawnPowerUp());
        StartCoroutine(StartUpdate());

    }



    public void OnPlayerDeath()
    {
        _isPlayerDead = true;
        StartCoroutine(StopUpdate());
    }


    IEnumerator StartUpdate()
    {
        yield return new WaitForSeconds(startTimeValue);
        enabled = true;
    }

    IEnumerator StopUpdate()
    {
        yield return new WaitForSeconds(.2f);

        enabled = false;


    }
}
