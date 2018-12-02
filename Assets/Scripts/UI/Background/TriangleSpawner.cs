using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class TriangleSpawner : MonoBehaviour
{
	public GameObject TrianglePrefab;
	
	public int SpawnPerFrame = 5;
	public int MinSpawn = 50;
	public int MaxSpawn = 100;

	public IEnumerator SpawnTriangles(RectTransform wordRect, bool halfVertical = true)
	{
		var corners = new Vector3[4];
		
		wordRect.GetWorldCorners(corners);

		if (Camera.main == null)
		{
			Debug.LogError("Camera.main returned null.");
			yield break;
		}
		
		var z = Camera.main.nearClipPlane;

		var bl = corners[0];
		var tl = corners[1];
		var tr = corners[2];

		bl = Camera.main.ScreenToWorldPoint(bl);
		tl = Camera.main.ScreenToWorldPoint(tl);
		tr = Camera.main.ScreenToWorldPoint(tr);

		bl.z = z;
		tl.z = z;
		tr.z = z;
		
		var spawnAmount = Random.Range(MinSpawn, MaxSpawn);
		for (var x = 1; x <= spawnAmount; ++x)
		{
			if (x % SpawnPerFrame == 0)
				yield return null;

			var bly = halfVertical ? bl.y * 0.5f : bl.y;
			var pos = new Vector3(
				Random.Range(bl.x, tr.x),
				Random.Range(bly, tl.y),
				z);

			Instantiate(TrianglePrefab, pos, Quaternion.identity);
		}
	}
}
