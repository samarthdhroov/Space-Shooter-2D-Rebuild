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


    private bool _hasAgression = false;
    private bool _hasAgressionEnabled = false;
    [SerializeField]
    private int _aggressionWeight = 20;

    private void Start()
    {
       
         player = GameObject.Find("Player").GetComponent<Player>();
         if (player == null)
            {
                Debug.LogError("Player componenet is NUll");
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

        transform.position = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);

        float weightRequired = Random.Range(0, 101);
        //Debug.Log("Random Weight Required: " + weightRequired);
        if (_aggressionWeight >= weightRequired)
        {
            _hasAgression = true;
        }
        else
        {
            _hasAgression = false;
        }

    }

    private void Update()
    {
        EnemyMovement();
       

    }

    public void setEnemySpeed(float value)
    {
        _speed = value;
    }

    public void EnemyMovement()
    {
        if (_hasAgressionEnabled == true && player != null)
        {
            rammybehavior();
        }


        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
           
        }

        if (Time.time > _canFire)
    {
        _fireRate = Random.Range(3.0f, 8.0f);
        _canFire = Time.time + _fireRate;
        GameObject EnemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity); //We are first getting hold of the prefab and then extracting its children. 
        Laser[] enemyLaserChild = EnemyLaser.GetComponentsInChildren<Laser>();

        for (int i = 0; i < enemyLaserChild.Length; i++)
        {
            enemyLaserChild[i].SetEnemyLaser();
        }
    }
}
    
   /* public  void AngularEnemyMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        StartCoroutine(ForwardAngleChange());

        if (transform.position.y < -5.3f)
        {
            Destroy(this.gameObject);
            
        }

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3.0f, 8.0f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(_BigGreenEnemyLaserPrefab, transform.position, Quaternion.identity); //We are first getting hold of the prefab and then extracting its children. 
            Laser[] enemyLaserChild = EnemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < enemyLaserChild.Length; i++)
            {
                enemyLaserChild[i].SetEnemyLaser();
            }
            
        }
        
    }

    IEnumerator ForwardAngleChange()
    {
             
            Vector3 newRotationAngles = transform.rotation.eulerAngles;
            
            newRotationAngles.z -= 1;
            yield return new WaitForSeconds(0.1f);
            transform.rotation = Quaternion.Euler(newRotationAngles);

            if (transform.position.y >=0 && transform.position.y <=1)
            {
            newRotationAngles.z = -45.0f;
            transform.rotation = Quaternion.Euler(newRotationAngles);
            }
              
    }*/

    void rammybehavior()
    {

        float xValue;
        float distance1 = Vector3.Distance(new Vector3(transform.position.x + 1.0f, transform.position.y , transform.position.z), player.transform.position);
        float distance2 = Vector3.Distance(new Vector3(transform.position.x - 1.0f, transform.position.y , transform.position.z), player.transform.position);
        if (distance1 < distance2)
        {
            xValue = 0.2f;
        }
        else if (distance1 > distance2)
        {
            xValue = -0.2f;
        }
        else
        {
            xValue = 0f;
        }

        Vector3 movement = new Vector3(xValue, -1, 0);
        transform.Translate(movement * _speed * Time.deltaTime);

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
        
        if(other.tag == "GreenLaser")
        {
           
            player.AddScore(10); 
            _TriggerAnimation();
            _explosionAudio.Play();

        }

        if(other.tag == "PlayerPresenceBox")
        {
            if (_hasAgression==true)
            {
                _hasAgressionEnabled = true;
            }

            if(_hasAgression == false)
            {
                _hasAgressionEnabled = false;
            }
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
