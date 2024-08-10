using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleBomb : MonoBehaviour
{

    [SerializeField]
    float maxSize, growSpeed;

    [SerializeField]
    bool canGrow;

    public List<GameObject> targets;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J) && canGrow)
        {
            ShootBomb(); 


        }
       
    }

    void ShootBomb()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {

            targets.Add(collision.gameObject);
        }
    }



}
