using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigCannon : MonoBehaviour
{
    CameraShake camShaker;

    [SerializeField]
    float _bcSpeed;

    // Start is called before the first frame update
    void Start()
    {
        camShaker = GameObject.Find("ShakeManager").GetComponent<CameraShake>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _bcSpeed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            
            Destroy(gameObject);
           //gameObject.SetActive(false);

        }
    }
    private void OnTriggerEnter2D(Collider2D other)

    {

        if (other.CompareTag("Enemy"))
        {

            camShaker.Tremor(.65f, 30f, .35f, 1);
        }

        Destroy(gameObject);

        //gameObject.SetActive(false);

    }


}
