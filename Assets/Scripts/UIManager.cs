using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _scoreText;

    [SerializeField]
    Text _BestText;

    [SerializeField]
    Image _livesImage;

    [SerializeField]
    Sprite[] _livesSprites;

    [SerializeField]
    Text _gameOverText;

    [SerializeField]
    Text _restartLevelText;

    [SerializeField]
    float _flashingGameOverTextSpeed = 0.5f;

    [SerializeField] GameObject _pauseMenuPanle;

    GameManager _gameManager;


    int _score;
    int _bestScore;


    // Start is called before the first frame update
    void Start()
    {
        _bestScore = PlayerPrefs.GetInt("HighScore", 0);
        _BestText.text = string.Format("Best: {0}", _bestScore);


        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);

        _gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
    }

    public void CheckForBestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
            _BestText.text = string.Format("Best: {0}", _bestScore);
        }
    }

   

    public void UpdateScoreText(int playerScore)
    {
        _score = playerScore;
        _scoreText.text = string.Format("Score: {0}", _score);
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives <= 0)
            HandleGameOver();
    }

    private void HandleGameOver()
    {
        GameObject _player1 = GameObject.Find("Player 1");
        GameObject _player2 = GameObject.Find("Player 2");


        if (_player1)
            Destroy(_player1);

        if (_player2)
            Destroy(_player2);

        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(FlashingGameOverText());
        _gameManager.GameOver();
    }

    IEnumerator FlashingGameOverText()
    {
        while (true)
        {
            yield return new WaitForSeconds(_flashingGameOverTextSpeed);
            _gameOverText.enabled = !_gameOverText.enabled;
        }
    }
    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        _pauseMenuPanle.SetActive(false);
        Animator anim = _pauseMenuPanle.GetComponent<Animator>();
        anim.SetBool("isPaused", false);
    }

}
