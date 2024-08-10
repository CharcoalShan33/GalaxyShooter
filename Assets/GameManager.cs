using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    [SerializeField]
    private bool isGameOver;
    // Start is called before the first frame update void Start()

    // this number is multiplied. Adds Difficulty
    public int difficulty;

    public void GameOver()
    {
        isGameOver = true;
    }

    // Update is called once per frame void Update()
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && isGameOver == true)
        {
            RestartLevel();
        }
        QuitGame();
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }

}