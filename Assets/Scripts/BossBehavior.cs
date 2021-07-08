using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    float moveSpeed = 8.5f;
    private float _fireRate;
    private float _canFire = -1.5f;

    
    ThrustSlider _bossHealthBar;
    [SerializeField]
    GameObject _bossLaser;
    [SerializeField]
    GameObject _bossRedLaser;
    [SerializeField]
    GameObject _bossSecondLaser;
    GameObject bosslocationPoint;
    SpawnManager spawnManager;


    private Animator bossAnimator;
    private Player player;

    private int fireCounter = 1;


    void Start()
    {
        transform.position = new Vector3(12.0f, 4.6f, 0);
        bosslocationPoint = GameObject.FindGameObjectWithTag("LocationPoints");

        _bossHealthBar = GameObject.Find("BossHealthBar").GetComponent<ThrustSlider>();
        _bossHealthBar.transform.GetChild(0).gameObject.SetActive(true);  //Enabling Fill child
        _bossHealthBar.transform.GetChild(1).gameObject.SetActive(true);  //Enabling Border child


        bossAnimator = transform.GetComponent<Animator>();


        if (GameObject.Find("Player")!= null) //Saves from Null Reference Exception.
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }

        spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        
    }

    void Update()
    {
        if (_bossHealthBar.GetComponent<ThrustSlider>().GetSliderValue() < 0.2f)
        {
            _bossHealthBar.transform.GetChild(0).gameObject.SetActive(false);
            _bossHealthBar.transform.GetChild(1).gameObject.SetActive(false);
            DestroyAnimation(); // Always avoid calling something that spans over time from update method. But in this case, animation gets triggered which is fine for now. The audio source would not play from here. I have used animation component to enable it as soon as the trigger turns on. 
          
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, bosslocationPoint.transform.position, moveSpeed * Time.deltaTime); //Moving towards the target position.

            if(Vector2.Distance(transform.position, bosslocationPoint.transform.position) < 0.1f && player.getPlayerLives()>0) //Fire begins after the enemy reaches its target position.
            {
                if (fireCounter % 6 == 0)
                    fireRedLaser();
                else if (fireCounter % 10 == 0)
                    fireSecondLaser();
                else
                    fireLaser();
            }
        }

        if (player.getPlayerLives() == 0) //To stop the boss from firing after the death of player.
        {
            _bossHealthBar.transform.GetChild(0).gameObject.SetActive(false);
            _bossHealthBar.transform.GetChild(1).gameObject.SetActive(false);
            spawnManager.OnPlayerDeath();
            transform.Translate(Vector2.down * Time.time * 0.05f); //Gets the boss out of the screen.
        }

        if (transform.position.y > 12.0f) //Destroys the boss after it is out of the screen.
            Destroy(this.gameObject); 
    }

    void DestroyAnimation()
    {
        bossAnimator.SetBool("OnBossDeath", true);
        Destroy(this.gameObject, 2.5f);
    }


    void fireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1.0f,2.0f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(_bossLaser, transform.position , Quaternion.identity);
            fireCounter++;
        }
    }

    void fireRedLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1.0f, 2.0f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(_bossRedLaser, transform.position, Quaternion.identity);
            fireCounter++;
        }
       
    }

    void fireSecondLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1.0f, 2.0f);
            _canFire = Time.time + _fireRate;
            GameObject EnemyLaser = Instantiate(_bossSecondLaser, transform.position - new Vector3(0,4.8f,0), Quaternion.identity);
            fireCounter++;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        float health = _bossHealthBar.GetSliderValue();

        if (collision.tag == "Laser")
        {
            health -= 0.5f;
            _bossHealthBar.ChangeSliderValue(health);
            player.AddScore(10);
        }

        if(collision.tag == "GreenLaser")
        {
            health -= 1.0f;
            _bossHealthBar.ChangeSliderValue(health);
            player.AddScore(20);
        }

        if(collision.tag == "Missile")
        {
            health -= 1.5f;
            _bossHealthBar.ChangeSliderValue(health);
            player.AddScore(30);
        }
    }

}


// If you would like to make the enemy move across the screen using location points, Use the following code. 

// int positionNumber = 0;
// private Vector2 targetPositionofPoint;
/*  void SetTargetPosition()
    {

        targetPositionofPoint = new Vector2(locationPoints[positionNumber].transform.position.x, locationPoints[positionNumber].transform.position.y);

    }*/
/*
    void Move()
    {
        SetTargetPosition();

        transform.position = Vector3.MoveTowards(transform.position,targetPositionofPoint, moveSpeed * Time.deltaTime);


        if (Vector2.Distance(transform.position, targetPositionofPoint) < 0.1f)
        {
           
            SetTargetPosition();
            positionNumber++;
        }

        if(positionNumber == locationPoints.Length)
        {
            positionNumber =0;
        }
        
    }*/