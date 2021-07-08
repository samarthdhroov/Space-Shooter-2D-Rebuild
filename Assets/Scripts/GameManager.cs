using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool _isGameOver = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(Input.GetKey(KeyCode.C) && _isGameOver == false)
        {
            GameObject[] powerups = GameObject.FindGameObjectsWithTag("PowerupForPlayer");
            
            foreach(GameObject item in powerups)
            {
                    item.GetComponent<Powerup>()._speed = 6.0f;
                    item.GetComponent<Powerup>().MoveTowardsPlayer();
            }
        }
    }

    public void gameOver()
    {
        _isGameOver = true;
    }

    public void LoadNewGame()
    {
        SceneManager.LoadScene(0);
    }

    
   
}
