using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStateController : MonoBehaviour
{
    [SerializeField]
    GameObject[] lifeIcons;

    [SerializeField]
    GameObject losePanel, waveClearPanel, winPanel, pausePanel;

    [SerializeField]
    TextMeshProUGUI waveClearMsg;
    
    private int livesLeft = 3, maxLives = 5;
    public bool isGamePaused = false;

    // Start is called before the first frame update
    void Start()
    {
        losePanel.SetActive(false);
        winPanel.SetActive(false);
        waveClearPanel.SetActive(false);
        pausePanel.SetActive(false);

        int i;
        for (i = 0; i < livesLeft; i++)
        {
            lifeIcons[i].SetActive(true);
        }
        for (; i<lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(false);
        }
        ResumeGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void ReduceLife()
    {
        livesLeft = Mathf.Clamp(--livesLeft, 0, maxLives);

        if(livesLeft <=0)
        {
            PauseGame();
            losePanel.SetActive(true);
        }
        lifeIcons[livesLeft].gameObject.SetActive(false);
    }

    public void AddLife()
    {
        livesLeft = Mathf.Clamp(++livesLeft, 0, maxLives);
        lifeIcons[livesLeft-1].gameObject.SetActive(true);
    }

    public void OnWaveCompleted(string waveCompletionMsg)
    {
        PauseGame();
        waveClearMsg.text = waveCompletionMsg;
        waveClearPanel.SetActive(true);
    }

    public void OnWin()
    {
        PauseGame();
        winPanel.SetActive(true);
    }

    public void OnContinueAfterWaveCleared()
    {
        waveClearPanel.SetActive(false);
        ResumeGame();
    }

    public void OnGamePaused()
    {
        PauseGame();
        pausePanel.SetActive(true);
    }

    public void OnGameResumed()
    {
        pausePanel.SetActive(false);
        ResumeGame();        
    }

    /*public void OnGoingBackFromSubmission()
    {
        submissionPanel.SetActive(false);
        ResumeGame();
    }*/
}
