using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserMovement : MonoBehaviour
{

    float _speed = 5.5f;

    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(this.gameObject);
            collision.GetComponent<Player>().damage();
        }

       
    }
}
