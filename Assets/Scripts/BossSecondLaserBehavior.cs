using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSecondLaserBehavior : MonoBehaviour
{
    private float _speed = 5.0f;
    private Player player;
    private bool MagnetEnabled = false;
    private Transform magnetLastPos;
    private SpawnManager spawnManager;

    private void Start()
    {
        if (GameObject.Find("MagnetEndLocation") != null)
        {
            magnetLastPos = GameObject.Find("MagnetEndLocation").transform;
        }

        if(GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        if (GameObject.Find("Spawn_Manager") != null)
        {
            spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        }


        StartCoroutine(MagnetClock());

    }
    private void Update()
    {
        if (player != null && MagnetEnabled == true)
        {

           float angle = RotateTowardsEnemy(player.transform);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            player.MoveTowardsBossMagnet(transform);
        }
        else
        {
            MoveOutOfScreen();
        }

        if (transform.position.y < -5.6f)
        {
            Destroy(this.gameObject);
        }
            
    }

    float RotateTowardsEnemy(Transform target)
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;
        float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;
        return angle + 90;
    }

    void MoveOutOfScreen()
    {
        transform.position = Vector2.MoveTowards(transform.position, magnetLastPos.position, Time.deltaTime * _speed);
    }

   
   IEnumerator MagnetClock()
    {
        MagnetEnabled = true;
        spawnManager.BossMagnetActivated = true;
        StartCoroutine(spawnManager.HeatPowerUp());
        yield return new WaitForSeconds(5.0f);
        MagnetEnabled = false;
        spawnManager.BossMagnetActivated = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerFire")
        {
            MagnetEnabled = false;
            spawnManager.BossMagnetActivated = false;
        }
    }


}
