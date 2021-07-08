using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour
{

        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private GameObject _bigGreenEnemyPrefab;
        [SerializeField]
        private GameObject _enemyContainer;
        [SerializeField]
        private GameObject _tripleShotPrefab;
        [SerializeField]
        private GameObject _speedShotPrefab;
        [SerializeField]
        private GameObject _powerupPrefab;
        [SerializeField]
        private GameObject _healthPowerUp;
        [SerializeField]
        private GameObject _greenWiperPower;
        [SerializeField]
        private GameObject _LaserDamager;
        [SerializeField]
        private GameObject _HommingMissilePowerUp;
        [SerializeField]
        private GameObject _heatPowerUp;
        [SerializeField]
        private Text _WaveText;
        [SerializeField]
        private UI_Manager uI_Manager;

        [SerializeField]
        private GameObject[] powerup;
    
        [SerializeField]
        private GameObject gameManager;
        int powerupId;

        private bool _stopSpawn = false;

        [SerializeField]
        WaveSpawner[] waveSpawner;
        int startingIndex = 0;

        float waveWaitingTimeFrame = 5.0f;

        Player player;

        int instanceCounter = 0;

    [SerializeField]
    ThrustSlider _bossHealthBar;

    public bool BossMagnetActivated = false;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        if(player == null)
        {
            Debug.Log("Player is empty");
        }

    }

  


    public void powerUpCoroutineStarter()
        {
            StartCoroutine(startEnemyWaves());

        if (startingIndex == 5)
            StartCoroutine(PowerupRoutine(1));
        else
            StartCoroutine(PowerupRoutine(3));

            StartCoroutine(SecondaryPowerUp());
    }

    IEnumerator startEnemyWaves()
         {
           while(_stopSpawn == false)
            {
                    for (int i = startingIndex; i < waveSpawner.Length; i++)
                    {
              
                       var currentWave = waveSpawner[startingIndex];
                       if(_stopSpawn == false)
                         {
                           uI_Manager.showWaveText(startingIndex);
                         }
                        yield return StartCoroutine(EnemySpawnRoutine(currentWave));
                    }
            } 
        
         }

        IEnumerator EnemySpawnRoutine(WaveSpawner waveConfig)
        {
            while (_stopSpawn == false)
            {

                for (int i = 0; i < waveConfig.getEnemyCount(); i++)
                {


                    foreach (GameObject item in waveConfig.getEnemy())
                    {
                        if (_stopSpawn == false)
                        {
                            GameObject NewEnemy = Instantiate(item);
                            instanceCounter++;
                            NewEnemy.transform.parent = _enemyContainer.transform;
                            if (item.tag == "Enemy")
                            {
                                NewEnemy.GetComponent<Enemy>().setEnemySpeed(waveConfig.EnemySpeed());
                            }
                            else if (item.tag == "Big Green Enemy")
                            {
                                NewEnemy.GetComponent<RestrictedEnemyMovement>().SetEnemySpeed(waveConfig.EnemySpeed());
                                Vector3 currentPosition = NewEnemy.transform.position;
                                NewEnemy.GetComponent<RestrictedEnemyMovement>().getInstancesNumber(instanceCounter);
                            }
                            else if(item.tag == "Rotating Enemy")
                            {
                                NewEnemy.GetComponent<RotatingEnemyScript>().SetEnemySpeed(waveConfig.EnemySpeed());                        
                            }
                            else if(item.tag=="Boss Enemy")
                            {
                                uI_Manager.DisplayArrowAndText();
                                yield return new WaitUntil(() => EnemyDead()); //I have understood this as in I did pass a method to the constructor of this method. To be digged more later on. 
                                waveWaitingTimeFrame = 2.0f;    
                            }
                        }
                        yield return new WaitForSeconds(1.0f);
                    }
                }
                    yield return new WaitForSeconds(waveWaitingTimeFrame);
                    break;
            }
                startingIndex++;

                if(startingIndex >= waveSpawner.Length && player.getPlayerLives()>0)
                {
                    _stopSpawn = true;
                    StartCoroutine(ExitPlan());
                }
        }

       

        IEnumerator ExitPlan()
        {
            _WaveText.enabled = true;
            _WaveText.text = "You have won this bad looking game !";
            yield return new WaitForSeconds(6.0f);
            gameManager.GetComponent<GameManager>().LoadNewGame();
        }

    bool EnemyDead()
    {
        if (_bossHealthBar.GetComponent<ThrustSlider>().GetSliderValue() < 0.2f)
            return true;
        else
            return false;
    }

    

    void choosePowerUp()
    {

        int _weightedTotal = 0;

        int[] powerupTable =
        {
            30,
            25,
            15,
            10,
            8,
            6,
            3
        };

        int[] poweruptostart =
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6
        };

        foreach(int item in powerupTable)
        {
            _weightedTotal += item;
        }

        int randomNumber = Random.Range(0, _weightedTotal);

        int i = 0;

        foreach(int weight in powerupTable)
        {
            if(randomNumber <= weight)
            {
                powerupId = poweruptostart[i];
                return;
                
            }
            else
            {
                i++;
                randomNumber -= weight;
            }
        }
    }

        IEnumerator PowerupRoutine(int value)
        {
        int waitPeriod = value;
            while (_stopSpawn == false)
            {
                Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
                choosePowerUp();
                Instantiate(powerup[powerupId], location, Quaternion.identity);
                yield return new WaitForSeconds(value);
            }
        }

 
        IEnumerator SecondaryPowerUp()
        {
            while (_stopSpawn == false)
            {

                Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
                Instantiate(_greenWiperPower, location, Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(30.0f, 35.0f));
            }
        }

        public void OnPlayerDeath()
        {
            _stopSpawn = true;

        }


   
      public IEnumerator HeatPowerUp()
    {
        while (_stopSpawn == false && BossMagnetActivated == true)
        {

            Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
            Instantiate(_heatPowerUp, location, Quaternion.identity);
            yield return new WaitForSeconds(3.0f);
        }
    }
      

}