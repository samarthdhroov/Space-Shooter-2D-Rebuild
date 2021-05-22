using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
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
    private GameObject[] powerup;

    private bool _stopSpawn = false;



    public void powerUpCoroutineStarter()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupRoutine());
        StartCoroutine(SecondaryPowerUp());
    }

    IEnumerator EnemySpawnRoutine()
    {
        while (_stopSpawn == false)
        {
            Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
            GameObject NewEnemy = Instantiate(_enemyPrefab, location, Quaternion.identity);
            NewEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator PowerupRoutine()
    {
        while (_stopSpawn == false)
        {
            Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
            int powerupId = Random.Range(0, 5);
            Instantiate(powerup[powerupId], location, Quaternion.identity); 
            yield return new WaitForSeconds(Random.Range(3, 8));
            
        }

    }

    IEnumerator SecondaryPowerUp()
    {
        while(_stopSpawn == false)
        {
            
            Vector3 location = new Vector3(Random.Range(-9.61f, 9.61f), 7.6f, 0);
            Instantiate(_greenWiperPower, location, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(30.0f,35.0f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
