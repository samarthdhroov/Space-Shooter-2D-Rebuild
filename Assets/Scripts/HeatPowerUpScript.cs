using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatPowerUpScript : MonoBehaviour
{
    float _speed = 2.5f;
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -6.0f)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Player>().ShootFire();
            Destroy(this.gameObject);
        }
    }
        
    
}
