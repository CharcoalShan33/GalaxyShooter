using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPuzzle : MonoBehaviour
{
    [SerializeField]float timer = 68.0f;
    [SerializeField] float seconds = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            CountdownUI countUI = GameObject.Find("UIDisplay").GetComponent<CountdownUI>();

            countUI.ActivatePuzzle(timer);
            StartCoroutine(StoppedActive());

        }
    }

    IEnumerator StoppedActive()
    {
        yield return new WaitForSeconds(3.0f);
        this.enabled = false;
    }
}
