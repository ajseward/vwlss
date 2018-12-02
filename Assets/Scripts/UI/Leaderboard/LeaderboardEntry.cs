using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardEntry : MonoBehaviour
{

	public TMP_Text NameText;
	public TMP_Text ScoreText;

	public string Name
	{
		get { return _name; }
		set
		{
			_name = value; 
			NameText.SetText(value); 
			NameText.ForceMeshUpdate();
		}
	}

	private string _name;

	public int Score
	{
		get { return _score;  }
		set
		{
			_score = value;
			ScoreText.SetText(value.ToString());
			ScoreText.ForceMeshUpdate();
		}
	}
	private int _score;
	public string Hash;
	public int HashScore;
	public string Difficulty;

	public bool Verify()
	{
		var testScore = 0;

		foreach (var c in Hash)
		{
			testScore += c;
		}

		return testScore == HashScore && Hash.Length == Score;
	}
}
