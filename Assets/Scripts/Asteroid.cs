using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _AsteroidExplosion;

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            ExplosionAnimation();
            Destroy(this.gameObject,0.20f);
            Destroy(collision.gameObject);
           
            
        }

        if (collision.tag == "Enemy")
        {

            Destroy(collision.gameObject);
            ExplosionAnimation();
            
        }
    }

    void ExplosionAnimation()
    {
        Instantiate(_AsteroidExplosion, transform.position, Quaternion.identity);
     //   gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
