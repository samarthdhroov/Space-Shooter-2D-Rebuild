using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RestrictedEnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float min_X, max_X;

    [SerializeField]
    private float min_Y, max_Y;

    //[SerializeField]
    private float moveSpeed ;

    [SerializeField]
    private GameObject _greenEnemyLaser;

    private Player player;
    private Animator _explosion;
    private AudioSource _explosionAudio;

    private float _fireRate;
    private float _canFire = -1.5f;

    private Vector3 targetPosition;

    float ExitTimeRemaining = 5;

    [SerializeField]
    private Vector3 exitPoint;

    private void Start()
    {
        transform.position = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);

        
         player = GameObject.Find("Player").GetComponent<Player>();
         if (player == null)
            {
                Debug.Log("Player componenet is NUll");
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

    private void Update()
    {
       
            Move();

            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3.0f, 5.0f);
                _canFire = Time.time + _fireRate;
                GameObject EnemyLaser = Instantiate(_greenEnemyLaser, transform.position, Quaternion.identity); //We are first getting hold of the prefab and then extracting its children. 
                Laser[] enemyLaserChild = EnemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < enemyLaserChild.Length; i++)
                {
                    enemyLaserChild[i].SetEnemyLaser();
                }
            }

        
    }

    public void SetEnemySpeed(float value)
    {
        moveSpeed = value;
    }


    void SetTargetPosition()
    {
        targetPosition = new Vector3(Random.Range(min_X, max_X),
            Random.Range(min_Y, max_Y), 0f);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            targetPosition, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            SetTargetPosition();

        ExitTimeRemaining -= Time.deltaTime;

        if(ExitTimeRemaining < 1)
        {
            ExitMethod();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(this.gameObject);
            player.damage();
        }

        if(collision.tag == "Laser")
        {
            Destroy(collision.gameObject);
            player.AddScore(15);
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

    public void _TriggerAnimation()
    {
        _explosion.SetTrigger("OnEnemyDeath");
        moveSpeed = 0;
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(this.gameObject, 1.5f);

    }


    public void ExitMethod()
    {
        float outSpeed = 0.1f;

        if(transform.position.x >0 && transform.position.x < 12)
        {
            transform.Translate(Vector3.right * outSpeed );
            if(transform.position.x >= 12)
            {
                Destroy(this.gameObject);
            }
        }
        else if(transform.position.x <0 && transform.position.x > -12)
        {
            transform.Translate(Vector3.left * outSpeed );
            if(transform.position.x <= -12)
            {
                Destroy(this.gameObject);
            }
        }
        
    }

}