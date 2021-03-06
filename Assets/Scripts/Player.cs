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

    //secondary player shot variable
    private bool _greenlaserActive = false;
    [SerializeField]
    private GameObject _secondLaserShot;

    // Thruster HUD variable
    public ThrustSlider _healthBar;

    //Shake Animation variable
    private Animator _shakeCamera;

    //Variables for LaserDamage
    
    [SerializeField]
    private Text _laserShockText;
    private bool LaserDamagerCollected = false;

    //Homming Missile Prefab
    [SerializeField]
    private GameObject missile;
    private int missileCount = 0;
    private bool missileFireEnabled = false;

    [SerializeField]
    GameObject FirePower;
   


    void Start()
    {
        transform.position = new Vector3(0, -2.94f, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _UIManager = GameObject.Find("UI Manager").GetComponent<UI_Manager>();
        _laserAudio = GetComponent<AudioSource>();
        _shakeCamera = GameObject.Find("Main Camera").GetComponent<Animator>();
        
       

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
        if(missileFireEnabled == true)
        {
            FireLaser(1);
        }
        else
        {
            FireLaser(0);
        }
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

    //Left Shift key mechanis and control for HUD in the following three methods.
    void LeftShiftSpeedThrust()
    {
        float currentSliderValue = _healthBar.slider.value;
        
            if (Input.GetKey(KeyCode.LeftShift))
                {
                   if (currentSliderValue > 0) 
                   { 
                    _speed = _shiftSpeedRate;
                    changeThrusterHUD();
                   }
                   else
                    { 
                    _speed = _normalSpeed; 
                    }
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    _speed = _normalSpeed;
                     StartCoroutine(updateThrusterHUD());
                }
    }

    void changeThrusterHUD()
    {
        if(_healthBar == null)
        {
            Debug.LogError("Empty Healthbar");
        }
        float newSliderValue = _healthBar.slider.value - 0.05f;
        _healthBar.ChangeSliderValue(newSliderValue);
    }

    IEnumerator updateThrusterHUD()
    {
       while(_healthBar.slider.value < 15) 
        {
            if (_healthBar.slider.value < 1)
            {
                yield return new WaitForSeconds(1);
            }

        yield return new WaitForSeconds(0.5f);
        float newSliderValue = _healthBar.slider.value + 0.2f;
        _healthBar.ChangeSliderValue(newSliderValue);
        yield return new WaitForSeconds(0.5f);
        }
    }


    void FireLaser(int type)
    {
        switch (type)
        {
            case 0:
                if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canfire)
                {
                    if (LaserDamagerCollected == false)
                    {

                        if (_totalLaserFired < 15)
                        {
                            _totalLaserFired++;
                            _noAmmoText.GetComponent<Text>().text = _totalLaserFired.ToString() + "/15"; // Updates text to current count of fired lasers.
                            _canfire = Time.time + _fireRate;
                            if (_tripleShotActive == true)
                            {
                                Instantiate(_tripleShot, transform.position, Quaternion.identity);
                            }
                            else if (_greenlaserActive == true)
                            {
                                Instantiate(_secondLaserShot, transform.position + new Vector3(0.0f, 1.5f, 0.0f), Quaternion.AngleAxis(90.0f, Vector3.forward));
                                StartCoroutine(GreenWiperEnable());
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
                            _noAmmoText.GetComponent<Text>().text = "No Ammo";                 // Changes the text to No Ammo.

                        }
                    }
                }
                break;
            case 1:
                if (missileFireEnabled == true && Input.GetKeyDown(KeyCode.Space))
                {
                    if (missileCount < 3)
                    {
                        GameObject FiredMissile = Instantiate(missile, transform.position, Quaternion.identity);
                        missileCount++;
                    }

                    if (missileCount == 3)
                    {
                        missileFireEnabled = false;
                    }
                }
                break;
                
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
        
        if (_shakeCamera != null)
        {
            _shakeCamera.SetTrigger("EnableShake");
        }
        else
        {
            Debug.LogError("Empty animator");
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

    public int getPlayerLives()
    {
        return _lives;
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

    // Ammo Code

    public void RefillAmmo()
    {
        _totalLaserFired = 0;                                                          //resets lase fire to 0.
        AmmoPrefab.GetComponent<RawImage>().color = new Color(1, 1, 1, 1);            //resets the prefab color to its original.
        _noAmmoText.GetComponent<Text>().text = _totalLaserFired.ToString() + "/15"; // Updates text to current count of fired lasers.          
    }

    public void HealthRefill()
    {
        if(_lives < 3) 
        { 
            _lives++;                                       


            switch (_lives) { 

                case 2:
                 _rightDamage.SetActive(false);
                 _UIManager.updateImage(2);
                  break;

                case 3:
                    _leftDamage.SetActive(false);
                    _rightDamage.SetActive(false);
                    _UIManager.updateImage(3);
                    break;
            }          
        }
        else
        {
            _lives = 3; 
        }   
    }


    public void GreenWiper()
    {
        _greenlaserActive = true; 
        /*Instantiate(_secondLaserShot, transform.position  + new Vector3(0.0f,1.5f,0.0f),Quaternion.AngleAxis(90.0f, Vector3.forward));*/

    }

    IEnumerator GreenWiperEnable()
    {
        while(_greenlaserActive == true)
        {
           yield return new WaitForSeconds(5.0f);
            _greenlaserActive = false;
        }
    }


    public void LaserDamagerPicked()
    {
        LaserDamagerCollected = true;   //This stops update method from starting the fire laser method.

        StartCoroutine(FireDisable());
    }
        
    IEnumerator FireDisable()
    {
        _laserShockText.enabled = true;
        int Counter = 5;
        while(Counter != 0)
        {
            
            _laserShockText.text = "Engine Shock. Hold for " + Counter.ToString();
            Counter--;
            yield return new WaitForSeconds(1.0f);
        }
        _laserShockText.enabled = false;
        LaserDamagerCollected = false; //This returns the control back to spacebar by allowing update method to call the fire method. 
    }

    public void FireMissile()
    {
        missileFireEnabled = true;
        StartCoroutine(StopMissileFire());
    }

    IEnumerator StopMissileFire()
    {
        yield return new WaitForSeconds(5.0f);
        missileFireEnabled = false;
        missileCount = 0;      //This has to be reset because the fire method updates it to 3 and it does not go back to 0 automatically.
    }

    public void redLaserDamager()
    {
        StartCoroutine(laserRotator());

    }

    IEnumerator  laserRotator()
    {
        for (int i = 0; i <= 360; i = i+5) //Added 5 as a jump here to mimic speed. I have to find a better way to add a speed to the rotation. 
        {
            transform.rotation = Quaternion.Euler(0, 0, i);
            yield return new WaitForEndOfFrame();
        }
    }

    public void MoveTowardsBossMagnet(Transform magnetTransform)
    {
        transform.position = Vector2.MoveTowards(transform.position, magnetTransform.position, Time.deltaTime * 2.5f);
        
    }

    public void ShootFire() //Initiates fire power at player's location.
    {
        Instantiate(FirePower, transform.position,Quaternion.identity);
    }
}
