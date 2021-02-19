using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 5f;

    [SerializeField]
    GameObject explosionVFX;

    SpanwManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = FindObjectOfType<SpanwManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
            _spawnManager.StartSpawning();
            Destroy(other.gameObject);
            Destroy(gameObject, 0.25f);
        }

    }
}
