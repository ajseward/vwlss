using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityScript.Core;

public class ScoreSubmit : MonoBehaviour
{
	public Button SubmitButton;
	public TMP_Text NameField;

	private void Start()
	{
		SubmitButton.onClick.AddListener(SubmitClicked);
	}

	private void SubmitClicked()
	{
		
		var user = NameField.GetParsedText();
		
		if (user.Length > GameSettings.Instance.MaxNameLength)
			return;

		var uiSwitch = GameObject.Find("MainUISwitch").GetComponent<UISwitchGroup>();

		var score = GameSession.Instance.Score;
		var hash = string.Concat(GameSession.Instance.FirstCharacter);
		var difficulty = GameSettings.Instance.Difficulty.ToString();
		try
		{
			Leaderboard.Instance.AddScore(user, score, hash, difficulty);
		}
		catch (Exception)
		{
			// ignored lol
		}
		
		uiSwitch.SwitchTo("LeaderboardUI");
		var diffSwitch = GameObject.Find("DifficultySwitch").GetComponent<UISwitchGroup>();
		diffSwitch.InitialSwitchableName = difficulty;
	}

	private void Update()
	{
		var length = NameField.GetParsedText().Length;
		var max = GameSettings.Instance.MaxNameLength;
		
		SubmitButton.interactable = length <= max && length > 2;
	}
}
