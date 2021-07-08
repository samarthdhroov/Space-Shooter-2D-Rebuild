using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    float _speed = 10.5f;
    Enemy[] allEnemies;

    BossBehavior bossEnemy;

    
    private void Update()
    {
        if(GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            allEnemies = GameObject.FindObjectsOfType<Enemy>();
        }
        
        if(GameObject.FindGameObjectWithTag("Boss Enemy") != null)
        {
            bossEnemy = GameObject.FindObjectOfType<BossBehavior>();
        }

        if (allEnemies == null)
        {
            Debug.Log("NO ENEMIES ON SCREEN");
        }

        if(bossEnemy == null)
        {
            Debug.Log("No boss found");
        }

        if(allEnemies != null)
        {
            foreach(Enemy item in allEnemies)
            {
                if(item.GetComponent<Enemy>().IamAlive() == true)
                {
                    MoveToEnemy(FindClosestEnemy());
                }

                if(item.GetComponent<Enemy>().IamAlive() == false)
                {
                    MoveOutOfScreen();
                }
            }
            
        }
        else if(bossEnemy != null)
        {
            MoveTowardsBoss();  
        }
        else
        {
            MoveOutOfScreen();
        }


        if(transform.position.y < -5.3f || transform.position.y >7.0f)
        {
            Destroy(this.gameObject);
        }
    }


    Enemy FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;

        foreach (Enemy currentEnemy in allEnemies)
        {

            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;

            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        return closestEnemy;
    }

    void MoveToEnemy(Enemy enemy)
    {
        float step = _speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, step);
        float angle = RotateTowardsEnemy(enemy.transform);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    float RotateTowardsEnemy(Transform target)
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;
        float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;
        return angle - 90;
    }

    void MoveTowardsBoss()
    {
        transform.position = Vector3.MoveTowards(transform.position, bossEnemy.transform.position, Time.deltaTime * _speed);
        float angle = RotateTowardsEnemy(bossEnemy.transform);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }

        if(collision.tag == "Boss Enemy")
        {
            Destroy(this.gameObject);
        }
    }

    void MoveOutOfScreen()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 8.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
