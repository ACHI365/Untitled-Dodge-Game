using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject square;

    private float startinterval = 5f;

    [SerializeField] private Transform[] spawnPoints;
    public static int EnemyCounter = 0;
    
    void Start()
    {
        EnemyCounter = 0;
        StartCoroutine(spawn(startinterval, square));
    }

    IEnumerator spawn(float interval, GameObject enemy)
    {
        if (ScoreManager.score > 1000)
            interval = 2.5f;
        if (EnemyCounter < 30)
        {
            Vector2 spawnPoint1 = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            Vector2 spawnPoint2 = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        
            yield return new WaitForSeconds(interval);
            Instantiate(enemy, spawnPoint1, Quaternion.identity);
            Instantiate(enemy, spawnPoint2, Quaternion.identity);
            StartCoroutine(spawn(interval, enemy));
        }
        
    }
}
