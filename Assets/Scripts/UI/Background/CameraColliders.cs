using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColliders : MonoBehaviour
{
	private Camera _cam;

	private void Awake()
	{
		_cam = GetComponent<Camera>();
		GenerateColliders();
	}

	private void GenerateColliders()
	{
		if (!_cam.orthographic)
		{
			Debug.LogError("Camera is not ortho. Cannot gen colliders.");
			return;
		}

		Vector2 bl = _cam.ScreenToWorldPoint(new Vector3(0, 0, _cam.nearClipPlane));
		Vector2 tl = _cam.ScreenToWorldPoint(new Vector3(0, _cam.pixelHeight, _cam.nearClipPlane));
		Vector2 br = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, 0, _cam.nearClipPlane));
		Vector2 tr = _cam.ScreenToWorldPoint(new Vector3(_cam.pixelWidth, _cam.pixelHeight, _cam.nearClipPlane));

		var edge = gameObject.AddComponent<EdgeCollider2D>();

		edge.points = new[] {bl, tl, tr, br, bl};
	}
}

