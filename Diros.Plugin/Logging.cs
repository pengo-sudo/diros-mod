using System;
using BepInEx.Logging;

namespace Diros.Plugin;

public static class Logging
{
	private static ManualLogSource logSource;

	public static void Init()
	{
		logSource = Logger.CreateLogSource("Diros");
	}

	public static void Debug(string message)
	{
		if (logSource != null)
		{
			logSource.LogDebug((object)message);
		}
	}

	public static void Info(string message)
	{
		if (logSource != null)
		{
			logSource.LogInfo((object)message);
		}
	}

	public static void Warning(string message)
	{
		if (logSource != null)
		{
			logSource.LogWarning((object)message);
		}
	}

	public static void Error(string message)
	{
		if (logSource != null)
		{
			logSource.LogError((object)message);
		}
	}

	public static void Exception(Exception e)
	{
		if (logSource != null)
		{
			logSource.LogError((object)e);
		}
	}
}
