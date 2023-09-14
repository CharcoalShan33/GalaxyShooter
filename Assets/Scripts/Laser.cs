using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;

    private bool _isEnemyLaser = false;

    [SerializeField]
    private Rigidbody2D rig;

    // Start is called before the first frame update
    void Start()
    {
        //Hide();

    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }

    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            if (transform.parent == null)
            {
                
                Destroy(transform.parent.gameObject);
            }

            Destroy(gameObject);

          
            
        }
    }

    void MoveDown()
    {
        //rig.velocity = Vector3.down * _speed;
        transform.Translate(Vector3.down * _speed * Time.deltaTime);


        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {

                //HideParent();
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void Hide()
    {
        this.gameObject.SetActive(false);
    }
    void HideParent()
    {
        transform.parent.gameObject.SetActive(false);
    }

    public void AssignLaser()
    {
        _isEnemyLaser = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _isEnemyLaser == true)
        {
            Player play = collision.gameObject.GetComponent<Player>();

            play.TakeDamage();

            Destroy(this.gameObject);
        }

    }
}
