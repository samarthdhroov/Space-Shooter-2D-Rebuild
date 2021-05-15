using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float _speed = 5.5f;
    private Player player;

    private Animator _explosion;

    //AudioSource for Explosion
    private AudioSource _explosionAudio;

    // enemy laser variables
    private float _fireRate;
    private float _canFire = -1.5f;
    [SerializeField]
    private GameObject _enemyLaserPrefab;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(player == null)
        {
            Debug.LogError("Player componenet is NUll");
        }

        _explosion = GetComponent<Animator>();

        if(_explosion == null)
        {
            Debug.LogError("Empty Animator");
        }

        _explosionAudio = GetComponent<AudioSource>();

        if(_explosionAudio == null)
        {
            Debug.LogError("Empty explosion Audio.");
        }

        }
    void Update()
    {
        EnemyMovement();

        if(Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 8.0f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity); //We are first getting hold of the prefab and then extracting its children. 
            Laser[] enemyLaserChild = EnemyLaser.GetComponentsInChildren<Laser>();
            
            for(int i=0; i< enemyLaserChild.Length; i++)
            {
                enemyLaserChild[i].SetEnemyLaser();
            }
            

        }


    }

    void EnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
            //transform.position = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {      
            player.damage();
            _TriggerAnimation();
            _explosionAudio.Play();
        }

        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            player.AddScore(10);
            _TriggerAnimation();
            _explosionAudio.Play();
        }
        
    }

    public void _TriggerAnimation()
    {
        _explosion.SetTrigger("OnEnemyDeath");
        _speed = 4.0f;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, 1.5f);

    }

}
