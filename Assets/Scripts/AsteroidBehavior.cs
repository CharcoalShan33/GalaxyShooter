using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidBehavior : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3f;

    [SerializeField]
    private GameObject explodeObject;

    private SpawnManager spawnManager;

  

    // Start is called before the first frame update
    void Start()
    {
        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Laser")
        {
            
            Instantiate(explodeObject,transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            spawnManager.StartSpawning();
           
            Destroy(this.gameObject, .35f);

           
          
            
        }
    }

    
    
}
