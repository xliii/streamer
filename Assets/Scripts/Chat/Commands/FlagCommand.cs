﻿using System;

public class FlagCommand : ChatCommand {

	public override string command()
	{
		return "!flag";
	}

	public override void process(User user, string[] args, Action<string> callback)
	{
		Flag flag;
		if (args.Length == 0)
		{
			flag = FlagRepository.GetByUsername(user.username);
			if (flag == null)
			{
				callback("You have no flag");
			}
			else
			{
				callback("You have a flag at " + flag.place);
			}
			return;
		}

		if (args[0] == "delete" || args[0] == "remove")
		{
			bool deleted = FlagRepository.DeleteByUsername(user.username);
			callback(deleted ? "Flag deleted" : "You have no flag");
			return;
		}

		if (args[0] == "clear")
		{
			int deleted = FlagRepository.Clear();
			callback(deleted == 0 ? "There were no flags" : deleted == 1 ? "1 flag deleted" : deleted + " flags deleted");
			return;
		}

		if (args[0] == "count")
		{
			var count = FlagRepository.GetAll().Count;
			callback(count == 0 ? "No flags :(" : (count == 1) ? "1 flag total" : count + " flags total");
			return;
		}

		string place = string.Join(" ", args);
		flag = FlagRepository.GetByUsername(user.username);
		bool wasNoFlag = flag == null;
		if (wasNoFlag)
		{
			flag = new Flag(user.username);
		}

		TwitchAPI.Geolocate(place, (formatted, lat, lng) =>
		{
			flag.place = formatted;
			flag.latitude = lat;
			flag.longitude = lng;
			FlagRepository.Save(flag);
			callback(wasNoFlag ? "Flag created at " + flag.place : "Flag set to " + flag.place);
		});
	}
}
