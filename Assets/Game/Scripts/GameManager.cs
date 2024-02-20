using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameUIManager gameUIManager;
    public Character playerCharacter;
    private bool _gameOver;

    private void Awake()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }
    private void gameOver()
    {
        gameUIManager.showGameOverUI();
    }
    public void gameWin()
    {
        gameUIManager.showGameWin();
    }
    private void Update()
    {
        if (_gameOver)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameUIManager.TogglePauseUI();
        }
        if (playerCharacter.currentState == Character.CharacterState.Dead)
        {
            _gameOver = true;
            gameOver();
        }
    }
    public void returnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
