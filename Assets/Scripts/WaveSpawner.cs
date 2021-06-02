using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "WaveConfig")]
public class WaveSpawner : ScriptableObject
{
    [SerializeField]
    private GameObject[] enemy;
    [SerializeField]
    private int enemyCount;
    [SerializeField]
    private float speed;

    public int getEnemyCount()
    {
        return enemyCount;
    }

    public GameObject[] getEnemy()
    {
        return enemy;
    }

    public float EnemySpeed()
    {
        return speed;
    }
}


