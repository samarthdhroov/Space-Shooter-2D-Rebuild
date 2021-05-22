using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    void Update()
    {
       
        StartCoroutine(scaleUp());

        if (transform.position.y >= 8.0f)
        {
            Destroy(this.gameObject);    
        }

    }

    IEnumerator scaleUp()
    {
        float speed = 0.2f;
        while(transform.position.y < 8.0f) 
        {
                transform.Translate(Vector3.right * Time.deltaTime * speed);
                transform.localScale += new Vector3(0.0f, 0.02f, 0.0f);
                yield return new WaitForSeconds(0.2f);
        }
    }

   
    
}
