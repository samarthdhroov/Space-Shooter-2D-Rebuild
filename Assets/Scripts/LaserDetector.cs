using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            return;
        }

        if(collision.tag == "Laser")
        {
            float value;

            if(this.transform.position.x< collision.transform.position.x)
            {
                value = -1.2f;
                this.GetComponentInParent<Enemy>().dodge(value);
            }

            if(this.transform.position.x > collision.transform.position.x)
            {
                value = 1.2f;
                this.GetComponentInParent<Enemy>().dodge(value);
            }
        }
    }
}
