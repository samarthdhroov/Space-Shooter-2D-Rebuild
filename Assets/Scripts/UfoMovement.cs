using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoMovement : MonoBehaviour
{
    float frequency = 1.5f;
    int amplitude = 9;
    [SerializeField]
    private GameObject _UfoLaserPrefab;
    private float _fireRate;
    private float _canFire = -1.5f;
    private float _speed = 1.5f;

    Player player;


    private void Start()
    {
        transform.position = new Vector3(Random.Range(10, -10), 7.0f, 0);

        player = GameObject.Find("Player").GetComponent<Player>();

        if(player == null)
        {
            Debug.Log("Empty Player");
        }

    }
    void Update()
    {

        if (transform.position.y > 1.5f)
        {
            float x = Mathf.Cos(Time.time * frequency) * amplitude;
            transform.position = new Vector3(x, transform.position.y, 0);
            transform.Translate(Vector3.down * Time.deltaTime * _speed);
            FireLaser();
        }
         else if (transform.position.y < 1.5f)
        {
            float outspeed = 3.5f;

            if (transform.position.x > 0 && transform.position.x < 10)
            {
                transform.Translate(Vector3.right * outspeed * Time.deltaTime);

                if (transform.position.x > 10)
                {
                    Destroy(this.gameObject);
                }
            }

            else if (transform.position.x < 0 && transform.position.z > -10)
            {
                transform.Translate(Vector3.left * outspeed * Time.deltaTime);
                if (transform.position.x < -10)
                {
                    Destroy(this.gameObject);
                }
            }

        }
    }

    void FireLaser()
    {
        if (Time.time > _canFire && player.getPlayerLives()>0)
        {
            _fireRate = 2.0f;
            _canFire = Time.time + _fireRate;
            Instantiate(_UfoLaserPrefab,transform.position,Quaternion.identity);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            Destroy(this.gameObject);
            Destroy(collision.gameObject);
            player.AddScore(20);
         
        }
    }
}
