using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Player getPlayer;
    [SerializeField] GameObject Object;

    // Start is called before the first frame update
    void Start()
    {
        getPlayer = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(gameObject);
            getPlayer.weapons.Add(Object);
        }

    }
}
