using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] TMP_Text timeText;
    [SerializeField] float coolDownSeconds;
    [SerializeField] float maxCoolDown;

    private TriggerTwo trig;

    bool willStop;
    // Start is called before the first frame update
    void Start()
    {
        timeText.gameObject.SetActive(false);
        trig = GameObject.Find("NewOne").GetComponent<TriggerTwo>();
    }

    // Update is called once per frame
    void Update()
    {
        timeText.text = coolDownSeconds.ToString("0.0");

        if (coolDownSeconds > 0)
        {
            willStop = false;
            coolDownSeconds -= Time.deltaTime;
        }

        else if (coolDownSeconds <= 0)
        {
            willStop = true;
            coolDownSeconds = 0;
        }
    }

    public void ActivatePuzzle(float count)
    {
      
            coolDownSeconds = maxCoolDown;
            timeText.gameObject.SetActive(true);
            coolDownSeconds = count;
            //TriggerTwo trig = GameObject.Find("Door").GetComponent<TriggerTwo>();
            trig.CreatePuzzleGame();
            
            StartCoroutine(CheckIfCorrect());
            if (count <= 0)
            {
                willStop = true;
                timeText.gameObject.SetActive(false);
                Debug.Log("Nothing Here");
            }
        
    }


    IEnumerator CheckIfCorrect()
    {
        if ( coolDownSeconds > 0 && willStop == false)
        {
            trig.IsCorrect();
            yield return new WaitForSeconds(1.0f);
            willStop = true;
            //timeText.enabled = false;
            enabled = false;
            Debug.Log("Done");
            coolDownSeconds = 0;
        }

    }
}
