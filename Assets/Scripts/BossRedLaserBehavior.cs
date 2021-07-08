using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRedLaserBehavior : MonoBehaviour
{
    private float speed = 10.5f;
    private Player player;

    void Start()
    {
        if(GameObject.Find("Player")!= null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        if (player == null)
        {
            Debug.Log("Player is Null");
        }
    }

    void Update()
    {
        if (player != null)
            Movetotheplayer();
        else
            moveToTheEndOfScreen();
       
    }

    public void Movetotheplayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, Time.deltaTime * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            player.redLaserDamager();
            Destroy(this.gameObject);
        }

        if(collision.tag == "GreenLaser")
        {
            Destroy(this.gameObject);
        }

    }

    public void moveToTheEndOfScreen()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        if (transform.position.y < -5.0f)
            Destroy(this.gameObject);

    }
}
