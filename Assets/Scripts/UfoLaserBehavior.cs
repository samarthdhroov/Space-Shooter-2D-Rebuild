using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoLaserBehavior : MonoBehaviour
{
    private float frequency = 10.0f;  
    private float amplitude = 1.5f; 
    private float _speed = 3.5f;

    //Player Reference
    Player player;

    //SpawnManager Reference
    SpawnManager spawnManager;

    private float baseX;

    private void Start()
    {
        baseX = transform.position.x;

        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.Log("Empty Player");
        }

        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(spawnManager == null)
        {
            Debug.Log("Empty Spawn Manager");
        }
    }

    void Update()
    {
        float x =  Mathf.Cos(Time.time * frequency ) * amplitude;
        transform.position = new Vector3(x + baseX, transform.position.y, 0);
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
        }

        if (player.getPlayerLives() == 0)
        {
            spawnManager.OnPlayerDeath();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
            player.damage();
        }
    }
   
}
