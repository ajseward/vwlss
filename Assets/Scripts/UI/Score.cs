using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{

	private TMP_Text _tmp;

	private void Awake()
	{
		_tmp = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		_tmp.SetText("0");
		GameSession.Instance.ScoreChanged.AddListener(UpdateScore);
	}

	private void UpdateScore(int value)
	{
		_tmp.SetText(value.ToString());
	}
}
