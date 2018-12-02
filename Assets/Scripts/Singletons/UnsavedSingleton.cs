using UnityEngine;

public class UnsavedSingleton<T> : MonoBehaviour
	where T : UnsavedSingleton<T>
{
	public static T Instance
	{
		get
		{
			if (_instance == null)
			{
				var obj = GameObject.Find("UnsavedGameSingleton");
				if (obj == null)
					obj = new GameObject("UnsavedGameSingleton");

				var script = obj.GetComponent<T>();
				_instance = script == null ? obj.AddComponent<T>() : script;
			}

			return _instance;
		}
	}

	private void Awake()
	{
		_instance = (T) this;
	}

	private static T _instance;
}