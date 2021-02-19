using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    bool _isGameOver = false;

    [SerializeField] GameObject _pauseMenuPanle;

    public bool isCooppMode;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = _pauseMenuPanle.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenuPanle.SetActive(true);
            anim.SetBool("isPaused", true);

            Time.timeScale = 0;
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
    }

    
}
