using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int collisionDamage = 1;

    [SerializeField]
    private float movementSpeed = 2f;

    [SerializeField]
    private float _wrapLimit;

    [SerializeField]
    private float _rightLimit;

    [SerializeField]
    private float _leftLimit;

    [SerializeField]
    private AudioClip _deathClip;

    [SerializeField] GameObject _enemyLaserPrefab;

    Player _player;
    Animator _animator;
    Collider2D _collider2D;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player 1").GetComponent<Player>();
        //_player = FindObjectOfType<Player>();

        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();

        HandleFire();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleFire()
    {
        StartCoroutine(FireCoroutine());
    }

    IEnumerator FireCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2,4));
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
        }
    }

    private void HandleMovement()
    {
        transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);

        if (transform.position.y < _wrapLimit)
            transform.position = new Vector3(Random.Range(_leftLimit, _rightLimit), _wrapLimit * -1, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);

            if (_player != null)
                _player.AddPointsToScore(10);

            Die();
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(collisionDamage);
            Die();
        }
    }

    private void Die()
    {
        movementSpeed /= 2;
        _collider2D.enabled = false;

        AudioSource.PlayClipAtPoint(_deathClip, Camera.main.transform.position);
        _animator.SetTrigger("OnEnemyDestroy");
        var deadAnimationLength = _animator.GetCurrentAnimatorStateInfo(0).length;
        Destroy(gameObject, deadAnimationLength);
    }
}
