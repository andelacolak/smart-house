﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SmartHouse.Lib
{
	public class PandoraCommand
	{
		public PandoraCommand()
		{
		}

		public Result Play()
		{
			return CommandExecuter(PandoraCommandEnum.Play);
		}

		public Result Stop()
		{
			return CommandExecuter(PandoraCommandEnum.Stop);
		}

		public Result Next()
		{
			return CommandExecuter(PandoraCommandEnum.Next);
		}

		public Result ThumbUp()
		{
			return CommandExecuter(PandoraCommandEnum.ThumbUp);
		}

		public Result ThumbDown()
		{
			return CommandExecuter(PandoraCommandEnum.ThumbDown);
		}

		public Result VolumeUp()
		{
			return CommandExecuter(PandoraCommandEnum.VolumeUp);
		}

		public Result VolumeDown()
		{
			return CommandExecuter(PandoraCommandEnum.VolumeDown);
		}

		public PandoraResult GetCurrentSongInfo()
		{
			var homeDir = System.Environment.GetEnvironmentVariable("HOME");
			var path = Path.Combine(homeDir, @".config/pianobar/nowplaying");

			var lines = File.ReadLines(path).ToList();

			return new PandoraResult()
			{
				Artist = lines[0].Trim(),
				Song = lines[1].Trim(),
				Radio = lines[2].Trim(),
				Loved = lines[3] == "1",
				AlbumUri = lines[4].Trim(),
				Album = lines[5].Trim()
			};
		}

		public IEnumerable<string> GetStationList()
		{
			var homeDir = System.Environment.GetEnvironmentVariable("HOME");
			var path = Path.Combine(homeDir, @".config/pianobar/stationlist");

			var lines = File.ReadLines(path).ToList().Select(x => x.Replace(")", ""));
			return lines;
		}

		private Result CommandExecuter(PandoraCommandEnum command)
		{
			var message = String.Empty;
			switch (command)
			{
				case PandoraCommandEnum.Play:
					BashHelper.ExecBashCommand("echo 'p' >> /home/pi/.config/pianobar/ctl");
					message = "Play/Pause";
					break;
				case PandoraCommandEnum.Stop:
					BashHelper.ExecBashCommand("./pandora.sh stop");
					message = "Stop";
					break;				
				case PandoraCommandEnum.Next:
					BashHelper.ExecBashCommand("echo 'n' >> /home/pi/.config/pianobar/ctl");
					message = "Next";
					break;
				case PandoraCommandEnum.ThumbUp:
					BashHelper.ExecBashCommand("echo '+' >> /home/pi/.config/pianobar/ctl");
					message = "Thumb Up";
					break;
				case PandoraCommandEnum.ThumbDown:
					BashHelper.ExecBashCommand("echo '-' >> /home/pi/.config/pianobar/ctl");
					message = "Thumb Down";
					break;
				case PandoraCommandEnum.VolumeUp:
					BashHelper.ExecBashCommand("echo '))' >> /home/pi/.config/pianobar/ctl");
					message = "Volume Up";
					break;	
				case PandoraCommandEnum.VolumeDown:
					BashHelper.ExecBashCommand("echo '((' >> /home/pi/.config/pianobar/ctl");
					message = "Volume down";
					break;
				default:
					throw new Exception($"Command {command} is not defined");
			}

			return new Result()
			{
				ErrorCode = 0,
				Message = message,
				Ok = true
			};
		}
	}
}
