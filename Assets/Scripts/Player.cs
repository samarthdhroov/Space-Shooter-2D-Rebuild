using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.5f;
    private int _speedMultiplier = 2;
    private float _shiftSpeedRate = 15.5f;
    private float _normalSpeed = 10.5f;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShot;

    //Cooldown System Variables
    private float _fireRate = 0.15f;
    private float _canfire = -1;

    //Lives variable
    [SerializeField]
    private int _lives = 3;

    //Spawning Varible
    public bool _stopSpawn = false;

    //tripleShot variable
    private bool _tripleShotActive = false;

    //speedShot variable
    private bool _speedShotActive = false;

    //ShieldShot variable
    private bool _shieldShotActive = false;
    [SerializeField]
    private GameObject _shieldVisual;

    private SpawnManager _spawnManager;

    /*//UI manager variable
    [SerializeField]
    private Image[] _livesImage;*/

    //score variable
    [SerializeField]
    private int _Score;

    //UI manager handle
    private UI_Manager _UIManager;


    // Damage Variable
    [SerializeField]
    private GameObject _rightDamage, _leftDamage;

    private AudioSource _laserAudio;

    //Sprite Color Variable
    [SerializeField]
    private int _shieldMethodCallCount = 0;

    //Laser shot count variable
    [SerializeField]
    private int _totalLaserFired = 0;
    [SerializeField]
    private GameObject AmmoPrefab;
    [SerializeField]
    private Text _noAmmoText;
    


    void Start()
    {
        transform.position = new Vector3(0, -3.89f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        _laserAudio = GetComponent<AudioSource>();
        

        if (_spawnManager == null)
        {
            Debug.LogError("Null spawn manager.");
        }

        if (_UIManager == null)
        {
            Debug.LogError("Null UI Manager.");
        }
    }


    void Update()
    {

        Translate();
        Movement();
        LeftShiftSpeedThrust();
        FireLaser();
    }

    void Movement()
    {


        if (transform.position.x > 9.18f)
        {
            transform.position = new Vector3(-9.18f, transform.position.y, 0);
        }
        else if (transform.position.x < -9.18f)
        {
            transform.position = new Vector3(9.18f, transform.position.y, 0);
        }

        if (transform.position.y > -1.5f)
        {
            transform.position = new Vector3(transform.position.x, -1.5f, 0);
        }

        if (transform.position.y < -3.91f)
        {
            transform.position = new Vector3(transform.position.x, -3.91f, 0);
        }
    }

    private void Translate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 _direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(_direction * _speed * Time.deltaTime);
    }


    void LeftShiftSpeedThrust()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _shiftSpeedRate;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed = _normalSpeed;
        }
    }


    void FireLaser()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
        {
            if (_totalLaserFired < 15)
            {
                _totalLaserFired++;
                _canfire = Time.time + _fireRate;
                if (_tripleShotActive == true)
                {
                    Instantiate(_tripleShot, transform.position, Quaternion.identity);
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
                }
                _laserAudio.Play();
            }
            else
            {
                AmmoPrefab.GetComponent<RawImage>().color = new Color(1, 0, 0, 1); // Changes the ammo image color to red.
                _laserAudio.Stop();                                                // Stops the laser fire sound since we have run out of ammo.
                _noAmmoText.GetComponent<Text>().enabled = true;                   //Enables the text to true.

            }

        }
    }

    

    public void damage()
    {

        if (_shieldShotActive == true)
        {
            _shieldMethodCallCount = 0;
            _shieldShotActive = false;
            _shieldVisual.SetActive(false);
            return;
        }
        
        _lives--;

        if (_lives == 2)
        {
            _rightDamage.SetActive(true);
            _UIManager.updateImage(2);
        }

        if (_lives == 1)
        {
            _leftDamage.SetActive(true);
            _UIManager.updateImage(1);
        }

        if (_lives < 1)
        {
            Destroy(this.gameObject);
            _UIManager.updateImage(0);
            _spawnManager.OnPlayerDeath();
        }
    }

    public void isTripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerUp());
    }

    IEnumerator TripleShotPowerUp()
    {
        while (_tripleShotActive == true) {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
    }

    public void isSpeedShotActive()
    {
        _speedShotActive = true;
        //_speed = 20.5f;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerUp());
    }

    IEnumerator SpeedPowerUp()
    {
        while (_speedShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _speedShotActive = false;
            // _speed = 10.5f;
            _speed /= _speedMultiplier;
        }
    }

    public void ShieldPowerUp()
    {
      
       _shieldMethodCallCount++;
        ChangeShieldColor();
        _shieldShotActive = true;
        _shieldVisual.SetActive(true);
    }

    public void AddScore(int points)
    {
        _Score += points;
    }

    public int UpdateScore()
    {
        return _Score;
    }

    void ChangeShieldColor()
    {
      
        float[] alpha = {0, 0.33f, 0.66f, 1.0f }; // I had to put a 0 for the 0 index since the method call starts from 1. The 0 here helps the alpha to begin at 0.33f.

        if (_shieldMethodCallCount < 4) 
        { 
            for(int i =0; i<= _shieldMethodCallCount; i++)
            {
            _shieldVisual.GetComponent<SpriteRenderer>().color = new Color(1,1,1,alpha[_shieldMethodCallCount]);
            }
        }
        else
        {
            _shieldVisual.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        }

    }


}
