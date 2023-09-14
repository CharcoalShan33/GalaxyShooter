using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 2.4f);
      
    }


    
}
