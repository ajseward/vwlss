using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class GameTimer : MonoBehaviour
{

	public Color ErrorColor;
	
	private TMP_Text _tmp;

	private bool _isAnimating;

	private void Awake()
	{
		_tmp = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		_tmp.SetText("60");
		GameSession.Instance.TimeChanged.AddListener(Countdown);
		GameSession.Instance.BadKeyPressed.AddListener(CallBadKeyAnim);
	}

	private void CallBadKeyAnim()
	{
		StartCoroutine(BadKeyAnim());
	}

	private IEnumerator BadKeyAnim()
	{
		if (_isAnimating) 
			yield break;
		
		_isAnimating = true;
		var baseColor = _tmp.color;

		var flip = false;

		for (var i = 1; i <= 4; ++i)
		{
			_tmp.color = flip ? baseColor : ErrorColor;
			_tmp.rectTransform.localScale = flip ? Vector3.one : Vector3.one * 1.4f;
			flip = !flip;
			yield return new WaitForSeconds(0.05f);
		}

		_tmp.color = baseColor;
		_isAnimating = false;
	}

	private void Countdown(int value)
	{
		value = Mathf.Max(0, value);
		_tmp.SetText(value.ToString());
	}
}
