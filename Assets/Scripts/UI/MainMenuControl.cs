using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuControl : MonoBehaviour
{

	public Button EasyButton;
	public Button MediumButton;
	public Button HardButton;
	public Button InsaneButton;

	private TriangleSpawner _spawner;

	public void StartDifficulty(int difficulty)
	{
		GameSettings.Instance.Difficulty = (GameSettings.EDifficulty)difficulty;
		SceneManager.LoadScene("GameScene");
	}

	private void Awake()
	{
		_spawner = GetComponentInParent<TriangleSpawner>();
	}

	private void Start()
	{
		var unlocked = GameSettings.Instance.UnlockedDifficulty;
		EasyButton.interactable = unlocked >= GameSettings.EDifficulty.Easy;
		MediumButton.interactable = unlocked >= GameSettings.EDifficulty.Medium;
		HardButton.interactable= unlocked >= GameSettings.EDifficulty.Hard;
		InsaneButton.interactable = unlocked >= GameSettings.EDifficulty.Insane;

		StartCoroutine(RepeatSpawnTriangles());
	}

	public IEnumerator RepeatSpawnTriangles()
	{
		while (true)
		{
			StartCoroutine(_spawner.SpawnTriangles(transform.parent as RectTransform, false));
			yield return new WaitForSeconds(1.5f);
		}
	}
}
