using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField]
    float _speed;

    private Rigidbody2D rig;



    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();

        if (rig == null)
        {
            Debug.LogError("THIS COMPONENT IS NULL! FIND THE COMPONENT");
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            //if (transform.parent == null)
            {
                //transform.parent.gameObject.SetActive(false);
            }

            Destroy(gameObject);
            //gameObject.SetActive(false);


        }






    }
}
