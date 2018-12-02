using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityIntEvent : UnityEvent<int>
{
}
public class GameSession : UnsavedSingleton<GameSession>
{
    public UnityIntEvent ScoreChanged = new UnityIntEvent();
    public UnityIntEvent TimeChanged = new UnityIntEvent();
    public UnityEvent BadKeyPressed = new UnityEvent();
    
    public bool CanType = true;

    public int Score
    {
        get { return _score; }
        set { _score = value; ScoreChanged?.Invoke(_score); }
    }

    private int _score;
    public List<char> FirstCharacter = new List<char>();

    public int GameTime
    {
        get { return _gameTime; }
        set { _gameTime = value; TimeChanged?.Invoke(_gameTime); }
    }

    private int _gameTime;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        GameTime = GameSettings.Instance.GameTimeSeconds;
        CanType = true;
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (GameTime > 0)
        {
            yield return new WaitForSeconds(1f);
            if (CanType)
                GameTime -= 1;
        }

        CanType = false;
        GameTime = 0;

        var difficulty = GameSettings.Instance.Difficulty;
        var difficultyInt = (int) difficulty;
        
        if (PlayerPrefs.GetInt("UnlockedDifficulty") <= difficultyInt)
            PlayerPrefs.SetInt("UnlockedDifficulty", ++difficultyInt);

        GameSettings.Instance.UnlockedDifficulty = (GameSettings.EDifficulty) difficultyInt;
        EndGame();
    }

    private void EndGame()
    {
        var obj = GameObject.Find("MainUISwitch");

        var switchGroup = obj.GetComponent<UISwitchGroup>();
        
        switchGroup.SwitchTo("SubmitUI");
    }

    public void BadKeyPenalty()
    {
        GameTime -= GameSettings.Instance.WordErrorNegativeTime;
        BadKeyPressed?.Invoke();
    }

    public void AddCompleteWordTime()
    {
        GameTime += GameSettings.Instance.WordCompleteBonusTime;
    }
}