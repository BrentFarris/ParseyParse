using UnityEngine;
using Parse;
using System;
using System.Collections.Generic;

public class GameConfig : MonoBehaviour
{
	private List<Action> mainThreadCalls = new List<Action>();
	private object threadCallMutex = new object();

	private void Start()
	{
		UpdateConfig(delegate()
		{
			Application.LoadLevel(Application.loadedLevel + 1);
		});
	}

	public void UpdateConfig(Action callback)
	{
		ParseConfig.GetAsync().ContinueWith(t =>
		{
			lock (threadCallMutex)
			{
				mainThreadCalls.Add(callback);
			}
		});
	}

	private void Update()
	{
		if (mainThreadCalls.Count > 0)
		{
			lock (threadCallMutex)
			{
				foreach (Action call in mainThreadCalls)
					call();

				mainThreadCalls.Clear();
			}
		}
	}
}