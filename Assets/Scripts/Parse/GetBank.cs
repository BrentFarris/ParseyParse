using UnityEngine;
using System.Linq;
using System.Collections.Generic;

using Parse;

public class GetBank : MonoBehaviour
{
	private List<ParseObject> achievements = new List<ParseObject>();

	private void Start()
	{
		ParseUser.LogInAsync("brent", "password").ContinueWith(t =>
		{
			if (t.Exception != null)
			{
				Debug.LogException(t.Exception);
				return;
			}

			ParseQuery<ParseObject> query = ParseObject.GetQuery("Achievements");
			query.FindAsync().ContinueWith(a =>
			{
				achievements = a.Result.ToList();

				EarnAchievement("Clicks Bro");
			});
		});
	}

	private void EarnAchievement(string achievementName)
	{
		if (ParseUser.CurrentUser == null)
		{
			Debug.LogError("You are offline");
			return;
		}

		Debug.Log("Achievements");
		foreach (ParseObject a in achievements)
		{
			Debug.Log(a.Get<string>("name"));
			if (a.Get<string>("name") == achievementName)
			{
				ParseQuery<ParseObject> query = ParseObject.GetQuery("UserAchievements");
				query.WhereEqualTo("user", ParseUser.CurrentUser.ObjectId);
				query.WhereEqualTo("achievement", a.ObjectId);
				query.FirstAsync().ContinueWith(b =>
				{
					if (b.Exception != null)
					{
						ParseObject userAchievement = new ParseObject("UserAchievements");
						userAchievement["user"] = ParseUser.CurrentUser;
						userAchievement["achievement"] = a;
						userAchievement["done"] = true;
						userAchievement.SaveAsync();
					}
					else
					{
						// The user has it
						b.Result["done"] = true;
						b.Result.SaveAsync();
					}
				});

				break;
			}
		}
	}

	/*
	ParseQuery<ParseObject> query = ParseObject.GetQuery("Bank");
	query.WhereEqualTo("user", ParseUser.CurrentUser.ObjectId);
	query.FirstAsync().ContinueWith(b =>
	{
		if (b.Exception != null)
		{
			Debug.LogException(b.Exception);
			return;
		}

		ParseObject bank = b.Result;

		int coins = bank.Get<int>("coins");
		Debug.Log("You have: " + coins);

		bank["coins"] = coins + 50;
		bank.SaveAsync();
	});
	*/

	/* Bank Get Stupid Example
	ParseQuery<ParseObject> query = ParseObject.GetQuery("Bank");
	query.GetAsync("8sQvK6WR88").ContinueWith(t =>
	{
		ParseObject bank = t.Result;

		int coins = bank.Get<int>("coins");
		Debug.Log("You have: " + coins);
	}); 
	*/
}