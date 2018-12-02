using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class TriangleBehavior : MonoBehaviour
{
	private Rigidbody2D _rb;
	private SpriteRenderer _sprite;

	public float LifeTimeMin = 1f;
	public float LifeTimeMax = 4f;

	public float VelocityMin = 0.8f;
	public float VelocityMax = 2.8f;
	
	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_sprite = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void Start ()
	{
		transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
		var x = Random.Range(-1f, 1f);
		var y = Random.Range(-1f, 1f);
		_rb.AddRelativeForce(new Vector2(x, y) * Random.Range(VelocityMin, VelocityMax), ForceMode2D.Impulse);
		_rb.AddTorque(Random.Range(-2f, 2f));

		int idx;
		idx = Random.Range(0, GameSettings.Instance.TriangleColors.Count - 1);
		_sprite.color = GameSettings.Instance.TriangleColors[idx];
	}

	private void Update()
	{
		var color = _sprite.color;
		var lifeTime = Random.Range(LifeTimeMin, LifeTimeMax);
		color.a -= Time.deltaTime * (1 / lifeTime);
		_sprite.color = color;
		if (color.a <= 0f)
		{
			Destroy(gameObject);
		}
	}
}
