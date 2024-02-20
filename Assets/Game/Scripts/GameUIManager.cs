using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public GameManager gameManager;
    public TextMeshProUGUI coinText;
    public Slider healthSlider;
    public GameObject UI_Pause;
    public GameObject UI_GameOver;
    public GameObject UI_Win;
    private enum GameUIState
    {
        GamePlay, Pause, GameOver, Win
    }

    GameUIState currentState;
    private void Start()
    {
        switchUIState(GameUIState.GamePlay);
    }
    private void Update()
    {
        healthSlider.value = gameManager.playerCharacter.GetComponent<Health>().currentHealthPercentage;
        coinText.text = gameManager.playerCharacter.coin.ToString();
    }
    private void switchUIState(GameUIState state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_Win.SetActive(false);
        currentState = state;

        Time.timeScale = 1;

        switch (state)
        {
            case GameUIState.GamePlay:
                break;
            case GameUIState.Pause:    
                Time.timeScale = 0;
                UI_Pause.SetActive(true);
                break;
            case GameUIState.GameOver:
                UI_GameOver.SetActive(true);
                break;
            case GameUIState.Win:
                UI_Win.SetActive(true);
                break;
        }
    }
    
    public void TogglePauseUI()
    {
        if (currentState == GameUIState.GamePlay)
        {
            switchUIState(GameUIState.Pause);            
        }
        else if (currentState == GameUIState.Pause)
        {
            switchUIState(GameUIState.GamePlay);
        }           
    }
    public void button_MainMenu()
    {
        gameManager.returnToMainMenu();
    }
    public void button_Restart()
    {
        gameManager.restart();
    }
    public void showGameOverUI()
    {
        switchUIState(GameUIState.GameOver);
    }
    public void showGameWin()
    {
        switchUIState(GameUIState.Win);
    }
}
