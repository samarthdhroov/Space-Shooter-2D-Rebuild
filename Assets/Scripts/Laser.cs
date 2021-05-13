using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private float _speed = 8.5f;

    // enemy laser identification variable
    private bool _isEnemyLaser = false;

    void Update()
    {
        if(_isEnemyLaser == true)
        {
            DownwardMovement();
        }
        else 
        { 
             UpwardMovement();
        }
    }

    private void UpwardMovement()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 6.89f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    public void DownwardMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -6.89f)
        {

            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }

    }

    public void SetEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && _isEnemyLaser == true)
        {
            collision.GetComponent<Player>().damage();
            Destroy(this.gameObject);
        }
    }
}
