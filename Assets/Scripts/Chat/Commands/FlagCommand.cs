using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class FlagCommand : ChatCommand
{
	private Random random = new Random();

	public override string command()
	{
		return "!flag";
	}

	public override void Clauses()
	{
		Clause("", ctx =>
		{
			var flag = FlagRepository.GetByUsername(ctx.user.username);
			if (flag == null)
			{
				ctx.callback("You have no flag");
			}
			else
			{
				ctx.callback("You have a flag at " + flag.place);
			}
		});
		Clause("clear", ctx =>
		{
			int deleted = FlagRepository.Clear();
			ctx.callback(deleted == 0 ? "There were no flags" : deleted == 1 ? "1 flag deleted" : deleted + " flags deleted");
		});
		Clause("count", ctx =>
		{
			var count = FlagRepository.GetAll().Count;
			ctx.callback(count == 0 ? "No flags :(" : (count == 1) ? "1 flag total" : count + " flags total");
		});
		Clause("debug", Debug).Roles(UserRole.Streamer);
		Clause("delete", Remove);
		Clause("remove", Remove);
		Clause("color ROLE COLOR", (role, color, ctx) =>
		{
			UserRole userRole = UserRoleExtensions.Parse(role);
			if (userRole == UserRole.None)
			{
				ctx.callback("Invalid role specified");
				return;
			}
			
			PutFlag earth = FindObjectOfType<PutFlag>();
			if (earth == null)
			{
				ctx.callback("No earth active soz :(");
				return;
			}

			earth.SetFlagColor(userRole, color, true);
			ctx.callback("Color updated");
		}).Roles(UserRole.Admin);
		Clause(Param.REST, (place, ctx) =>
		{
			var flag = FlagRepository.GetByUsername(ctx.user.username);
			AddFlag(ctx.user.username, flag, place, ctx.callback);
		});
	}	

	private void Remove(Context ctx)
	{
		bool deleted = FlagRepository.DeleteByUsername(ctx.user.username);
		ctx.callback(deleted ? "Flag deleted" : "You have no flag");
	}

	private void Debug(Context ctx)
	{
		PutFlag earth = FindObjectOfType<PutFlag>();
		if (earth == null)
		{
			ctx.callback("No earth, no debug ¯\\_(ツ)_/¯ ");
			return;
		}

		int count = 1;
		if (ctx.args.Length > 1)
		{
			if (!int.TryParse(ctx.args[1], out count))
			{
				count = 1;
			}
		}

		float delay = 1;
		if (ctx.args.Length > 2)
		{
			if (!float.TryParse(ctx.args[2], out delay))
			{
				delay = 1;
			}
		}

		earth.StartCoroutine(AddDebugFlag(count, delay));
		ctx.callback(count + " random debug flag added");
	}

	private IEnumerator AddDebugFlag(int count, float delay)
	{
		while (count > 0)
		{
			count--;
			var username = "Debug" + random.Next(1000);
			var debugFlag = new Flag(username)
			{
				place = "Debug place",
				latitude = (float)random.NextDouble() * 150 - 75,
				longitude = (float)random.NextDouble() * 360 - 180
			};
			//omit top/bottom 15 degrees 
			FlagRepository.Save(debugFlag);
			yield return new WaitForSeconds(delay);
		}
	}

	private void AddFlag(string username, Flag flag, string place, Action<string> callback)
	{
		bool wasNoFlag = flag == null;
		if (wasNoFlag)
		{
			flag = new Flag(username);
		}

		TwitchAPI.Geolocate(place, (formatted, lat, lng) =>
		{
			flag.place = place; //TODO: strip formatting. Meanwhile Unicode characters break JSON serialization
			flag.latitude = lat;
			flag.longitude = lng;
			FlagRepository.Save(flag);
			callback(wasNoFlag ? "Flag created at " + flag.place : "Flag set to " + flag.place);
		});
	}
}
