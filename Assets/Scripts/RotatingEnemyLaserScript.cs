using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemyLaserScript : MonoBehaviour
{

    float _speed = 7.5f;
    int _dir = -1;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, _dir, 0) * _speed * Time.deltaTime);

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.x > 11 || transform.position.x < -11)
        {
            Destroy(this.gameObject);
        }
    }

    public void rotatedFire(int value)
    {
        switch (value)
        {
            case 1:
                transform.rotation = Quaternion.Euler(0, 0, 90);
                break;
            case -1:
                transform.rotation = Quaternion.Euler(0, 0, -90);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Destroy(this.gameObject);
            collision.GetComponent<Player>().damage();
        }

        if (collision.tag == "GreenLaser")
        {
            Destroy(this.gameObject);
        }
    }
}
