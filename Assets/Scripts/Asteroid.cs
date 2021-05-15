using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    [SerializeField]
    private GameObject _AsteroidExplosion;
    private SpawnManager _spawnManager;



    private void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

  

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            ExplosionAnimation();
            Destroy(this.gameObject,0.20f);
            Destroy(collision.gameObject);
            _spawnManager.powerUpCoroutineStarter();            
        }

    }

    void ExplosionAnimation()
    {
        Instantiate(_AsteroidExplosion, transform.position, Quaternion.identity);
    }
}
