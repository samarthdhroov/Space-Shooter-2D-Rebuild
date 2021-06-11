using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemyScript : MonoBehaviour
{

    float _speed;

    Player player;

    Transform _Playerlocation;

    int _Ydir = -1;

    int laserDirection;

    [SerializeField]
    GameObject Laser;
    private float _fireRate;
    private float _canFire = -1f;

    float RotateTest = -1f;

    bool rotatedOnce = false;

    private Animator _explosion;
    private AudioSource _explosionAudio;


    void Start()
    {
        transform.position = new Vector3(Random.Range(-6.5f, 6.5f), 7.6f, 0);

        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.Log("Empty Player");
        }

        _Playerlocation = GameObject.Find("Player").GetComponent<Transform>();

        if (_Playerlocation == null)
        {
            Debug.Log("Empty player location");
        }

        _explosion = GetComponent<Animator>();

        if (_explosion == null)
        {
            Debug.LogError("Empty Animator");
        }

        _explosionAudio = GetComponent<AudioSource>();

        if (_explosionAudio == null)
        {
            Debug.LogError("Empty explosion Audio.");
        }

    }
    public void SetEnemySpeed(float value)
    {
        _speed = value;
    }

    void Update()
    {
        transform.Translate(new Vector3(0,_Ydir,0) * _speed * Time.deltaTime);

        if (player != null)
        {
            if (transform.position.y - _Playerlocation.transform.position.y < RotateTest)
            {
                rotate();
                rotatedOnce = true;
            }
            else
            {
                if(rotatedOnce == true)
                {
                    return;
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    _Ydir = -1;
                }
                
            }
        }

        if(transform.position.y < -5.3f) 
        { 
            Destroy(this.gameObject); 
        }


        if(transform.position.x>11 || transform.position.x < -11)
        {
            Destroy(this.gameObject);
        }

        fireLaser();

    }

    void rotate()
    {
        if (player != null)
        {
            if(transform.position.x < _Playerlocation.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
                laserDirection = 1;
            }

            if(transform.position.x > _Playerlocation.transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, -90);
                laserDirection = -1;
            }
         
            _Ydir = 1;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Laser")
        {
            player.AddScore(10);
            _TriggerAnimation();
            _explosionAudio.Play();

        }

        if (collision.tag == "Player")
        {
            player.damage();
            _TriggerAnimation();
            _explosionAudio.Play();
        }

        if (collision.tag == "GreenLaser")
        {

            player.AddScore(10);
            _TriggerAnimation();
            _explosionAudio.Play();

        }
        
    }

    void fireLaser()
    {

        if (Time.time > _canFire)
        {
            _fireRate = 1.0f;
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(Laser, transform.position, Quaternion.identity); //We are first getting hold of the prefab and then extracting its children. 
            EnemyLaser.GetComponent<RotatingEnemyLaserScript>().rotatedFire(laserDirection);
        
        }
    }

    public void _TriggerAnimation()
    {
        _explosion.SetTrigger("OnEnemyDeath");
        _speed = 3.0f;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, 1.5f);

    }
}