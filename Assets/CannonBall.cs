using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] float speed;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    transform.Translate(Vector3.up * speed * Time.deltaTime );

    if(transform.position.y >= 9.3f)
    {
        Destroy(gameObject);
    }
    }
}
