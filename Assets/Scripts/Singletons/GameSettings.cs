using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : Singleton<GameSettings> {
	
	[Serializable]
	public enum EDifficulty
	{
		Easy,
		Medium,
		Hard,
		Insane,
		Full,
	}

	public List<Color> TriangleColors; 
	
	public EDifficulty Difficulty = EDifficulty.Medium;

	public TextAsset GetDifficultyFile(EDifficulty difficulty)
	{
		switch(difficulty)
		{
			case EDifficulty.Easy:
				return Resources.Load<TextAsset>("easy");
			
			case EDifficulty.Medium:
				return Resources.Load<TextAsset>("medium");
			
			case EDifficulty.Hard:
				return Resources.Load<TextAsset>("hard");
			
			case EDifficulty.Insane:
				return Resources.Load<TextAsset>("insane");
			
			case EDifficulty.Full:
				return Resources.Load<TextAsset>("full");
			
			default:
				throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
		}
	}

	public int GameTimeSeconds = 60;

	public int WordCompleteBonusTime = 2;

	public int WordErrorNegativeTime = 1;

	public char[] Vowels = {'a', 'e', 'i', 'o', 'u'};

	public int MaxNameLength = 10;

	public EDifficulty UnlockedDifficulty = EDifficulty.Easy;

	private void Awake()
	{
		UnlockedDifficulty = (EDifficulty)PlayerPrefs.GetInt("UnlockedDifficulty", 0);
	}
}
