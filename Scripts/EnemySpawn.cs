using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public BirdPool pool;
    public bool isLeft;

    [Range(1.5f, 2f)]
    public float timeBetweenEnemies;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= timeBetweenEnemies)
        {
            SpawnRandomEnemy();
            ReduceTimeToSpawn();
        }
    }

    private void SpawnRandomEnemy()
    {
        GameObject bird = pool.GetPooledBird();

        if (bird == null)
        {
            Debug.LogError("Couldn't find an inactive bird, not enough birds on the pool");
            return;
        }

        float randomY = Random.Range(4, -1.8f);
        bird.transform.position = new Vector3(transform.position.x, randomY, 0);

        if (!isLeft)
            bird.GetComponent<Bird>().Reverse();

        bird.SetActive(true);
        _timer = 0;
    }

    private void ReduceTimeToSpawn()
    {
        if (timeBetweenEnemies > 0.65f)
            timeBetweenEnemies -= 0.03f;
    }
}
