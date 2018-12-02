using UnityEngine;

public class Singleton<T> : MonoBehaviour
	where T : Singleton<T>
{
	public static T Instance 
	{
		get
		{
			if (_instance == null)
			{
				var obj = GameObject.Find("GameSingleton");
				if (obj == null)
					obj = new GameObject("GameSingleton");

				var script = obj.GetComponent<T>();
				_instance = script == null ? obj.AddComponent<T>() : script;
				DontDestroyOnLoad(obj);
			}

			return _instance;
		}
	}

	private void Awake()
	{
		_instance = (T)this;
	}

	private static T _instance;
}
