using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _speed = 3.0f;
    private Player player;
    [SerializeField]
    private int _powerupId;

    [SerializeField]
    AudioClip _powerupAudio;

 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if (player == null)
        {
            Debug.LogError("Null Player component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.80f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            switch (_powerupId)
            {
                case 0:
                    player.isTripleShotActive();
                    break;
                case 1:
                    player.isSpeedShotActive();
                    break;
                case 2:
                    player.ShieldPowerUp();
                    break;
                case 3:
                    player.RefillAmmo();
                    break;
                default:
                    Debug.Log("Default Value");
                    break;
            }
            AudioSource.PlayClipAtPoint(_powerupAudio, transform.position);
            Destroy(this.gameObject);
        }

    }
}
