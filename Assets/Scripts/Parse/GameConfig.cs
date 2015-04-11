using UnityEngine;
using Parse;
using System;

public class GameConfig : MonoBehaviour
{
	private void Start()
	{
		//UpdateConfig(ConfigUpdated);
		UpdateConfig(delegate()
		{
			Debug.Log("Cost: " + ParseConfig.CurrentConfig.Get<int>("tierOneCoins"));
		});
	}

	private void ConfigUpdated()
	{
		Debug.Log("Cost: " + ParseConfig.CurrentConfig.Get<int>("tierOneCoins"));
	}

	public void UpdateConfig(Action callback)
	{
		ParseConfig.GetAsync().ContinueWith(t =>
		{
			callback();
		});
	}
}