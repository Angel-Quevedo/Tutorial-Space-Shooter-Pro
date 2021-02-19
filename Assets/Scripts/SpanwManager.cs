using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanwManager : MonoBehaviour
{
    [SerializeField]
    GameObject _enemyPrefab;

    [SerializeField]
    GameObject[] _powerupsPrefab;

    [SerializeField]
    GameObject _enemyContainer;

    [SerializeField]
    float _minSpawnEnemyTime = 2f;

    [SerializeField]
    float _maxSpawnEnemyTime = 5f;

    [SerializeField]
    float _minSpawnPowerupTime = 3f;

    [SerializeField]
    float _maxSpawnPowerupTime = 7f;

    [SerializeField]
    private float _rightLimit;

    [SerializeField]
    private float _leftLimit;

    bool _canSpawn = true;

    // Start is called before the first frame update
    void Start()
    {
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerup());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(3f);
        while (_canSpawn)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(_leftLimit, _rightLimit), transform.position.y), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            float time = Random.Range(_minSpawnEnemyTime, _maxSpawnEnemyTime);
            yield return new WaitForSeconds(time);
        }
    }

    IEnumerator SpawnPowerup()
    {
        yield return new WaitForSeconds(3f);
        while (_canSpawn)
        {
            var randomPowerUpNum = Random.Range(0, _powerupsPrefab.Length);

            Instantiate(_powerupsPrefab[randomPowerUpNum], new Vector3(Random.Range(_leftLimit, _rightLimit), transform.position.y), Quaternion.identity, transform);

            float time = Random.Range(_minSpawnPowerupTime, _maxSpawnPowerupTime);
            yield return new WaitForSeconds(time);
        }
    }

    public void StopSpawn()
    {
        _canSpawn = false;
    }

}
