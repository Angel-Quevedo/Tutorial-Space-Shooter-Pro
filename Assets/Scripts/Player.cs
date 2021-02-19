using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isPlayer1;
    public bool isPlayer2;


    [SerializeField] 
    private float _movementSpeed = 2f;

    [SerializeField]
    private float _rightLimit;

    [SerializeField]
    private float _leftLimit;

    [SerializeField]
    private float _wrapLimit;

    [SerializeField]
    private float _topLimit;

    [SerializeField]
    private float _bottomLimit;

    [SerializeField]
    private GameObject _rightEnginePrefab;

    [SerializeField]
    private GameObject _leftEnginePrefab;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private AudioClip _laserSFX;

    [SerializeField]
    private GameObject _shields;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private float _laserYOffset = 0.8f;

    [SerializeField]
    private float _fireRate = 0.5f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private bool _isTripleShotActive = false;


    private bool _canPlayer1Fire = true;
    private bool _canPlayer2Fire = true;
    private bool _isShieldsActive = false;
    SpanwManager _spanwManager;
    int _score = 0;
    UIManager _uiManager;
    GameManager _gameManager;


    // Start is called before the first frame update
    void Start()
    {

        _gameManager = FindObjectOfType<GameManager>();

        if (!_gameManager.isCooppMode)
            transform.position = Vector3.zero;


        _spanwManager = GameObject.Find("Spawn Manager").GetComponent<SpanwManager>();
        //_spanwManager = FindObjectOfType<SpanwManager>();
        if (_spanwManager == null)
            Debug.LogError("The Spawn Manager is NULL");

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("The UI Manager is NULL");

        _uiManager.UpdateScoreText(_score);
        _uiManager.UpdateLives(_lives);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayer1)
        {
            Player1HandleMovement();
            Player1HandleFire();
        }

        if (isPlayer2)
        {
            Player2HandleMovement();
            Player2HandleFire();
        }
    }

    private void Player1HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _canPlayer1Fire)
            StartCoroutine(Player1Fire());

        //float _canFire2 = -1f;
        //if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire2)
        //{
        //    _canFire2 = Time.time + _fireRate;
        //    Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserYOffset, 0), Quaternion.identity);
        //}
    }

    IEnumerator Player1Fire()
    {
        _canPlayer1Fire = false;

        if (_isTripleShotActive)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserYOffset, 0), Quaternion.identity);

        AudioSource.PlayClipAtPoint(_laserSFX, Camera.main.transform.position);

        yield return new WaitForSeconds(_fireRate);
        _canPlayer1Fire = true;
    }

    private void Player2HandleFire()
    {
        if (Input.GetKeyDown(KeyCode.RightControl) && _canPlayer2Fire)
            StartCoroutine(Player2Fire());
    }

    IEnumerator Player2Fire()
    {
        _canPlayer2Fire = false;

        if (_isTripleShotActive)
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(_laserPrefab, transform.position + new Vector3(0, _laserYOffset, 0), Quaternion.identity);

        AudioSource.PlayClipAtPoint(_laserSFX, Camera.main.transform.position);

        yield return new WaitForSeconds(_fireRate);
        _canPlayer2Fire = true;
    }

    private void Player1HandleMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal");
        float verticalAxis = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalAxis, verticalAxis, 0) * _movementSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _bottomLimit, _topLimit));

        if (transform.position.x > _wrapLimit || transform.position.x < (_wrapLimit * -1))
            transform.position = new Vector3(((_wrapLimit * Mathf.Sign(transform.position.x)) + 0.01f * Mathf.Sign(transform.position.x) * -1) * -1, transform.position.y);
    }

    private void Player2HandleMovement()
    {
        float horizontalAxis = Input.GetAxis("Horizontal2");
        float verticalAxis = Input.GetAxis("Vertical2");

        transform.Translate(new Vector3(horizontalAxis, verticalAxis, 0) * _movementSpeed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _bottomLimit, _topLimit));

        if (transform.position.x > _wrapLimit || transform.position.x < (_wrapLimit * -1))
            transform.position = new Vector3(((_wrapLimit * Mathf.Sign(transform.position.x)) + 0.01f * Mathf.Sign(transform.position.x) * -1) * -1, transform.position.y);
    }




    public void TakeDamage(int damage)
    {
        if (_isShieldsActive)
        {
            DeactivateShields();
            return;
        }

        _lives -= damage;
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
            _rightEnginePrefab.SetActive(true);

        if (_lives == 1)
            _leftEnginePrefab.SetActive(true);

        if (_lives <= 0)
        {
            _uiManager.CheckForBestScore();
            _spanwManager.StopSpawn();
            Destroy(gameObject);
        }
    }


    public void ActivateTripleShot(float duration)
    {
        StartCoroutine(ActivateTripleShotCoroutine(duration));
    }

    IEnumerator ActivateTripleShotCoroutine(float duration)
    {
        _isTripleShotActive = true;
        yield return new WaitForSeconds(duration);
        _isTripleShotActive = false;
    }


    public void ActivateSpeed(float duration)
    {
        StartCoroutine(ActivateSpeedCoroutine(duration));
    }

    IEnumerator ActivateSpeedCoroutine(float duration)
    {
        _movementSpeed *= 2;
        yield return new WaitForSeconds(duration);
        _movementSpeed /= 2;
    }

    public void ActivateShields()
    {
        _isShieldsActive = true;
        _shields.SetActive(true);
    }

    void DeactivateShields()
    {
        _isShieldsActive = false;
        _shields.SetActive(false);
    }

    public void AddPointsToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScoreText(_score);
    }

    public int GetCurrentScore()
    {
        return _score;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyLaser"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
