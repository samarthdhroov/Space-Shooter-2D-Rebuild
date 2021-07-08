using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFireBomb : MonoBehaviour
{
    Transform magnetPos;
    float _speed = 6.5f;
  

    void Update()
    {

        if (GameObject.FindGameObjectWithTag("BossMagnet") != null)
        {
            magnetPos = GameObject.FindGameObjectWithTag("BossMagnet") .GetComponent<BossSecondLaserBehavior>().transform;
            float zAngle = RotateTowardsMagnet(magnetPos);
            transform.rotation = Quaternion.Euler(0, 0, zAngle);
            transform.position = Vector2.MoveTowards(transform.position, magnetPos.position, Time.deltaTime * _speed);

            if (Vector2.Distance(transform.position, magnetPos.position) < 0.1f)
                Destroy(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y > 7.0f || transform.position.y < -6.0f)
            Destroy(this.gameObject);
    }

    float RotateTowardsMagnet(Transform target)
    {
        float xDistance = target.position.x - transform.position.x;
        float yDistance = target.position.y - transform.position.y;
        float angle = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg;
        return angle;
    }

}
