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
    private GameObject[] powerup;

    private bool _stopSpawn = false;



    public void powerUpCoroutineStarter()
    {
        StartCoroutine(EnemySpawnRoutine());
        StartCoroutine(PowerupRoutine());
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
            int powerupId = Random.Range(0, 4);
            Instantiate(powerup[powerupId], location, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }

    }
    public void OnPlayerDeath()
    {
        _stopSpawn = true;
    }
}
