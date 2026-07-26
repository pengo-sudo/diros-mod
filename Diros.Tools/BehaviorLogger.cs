using System;
using System.IO;
using UnityEngine;

namespace Diros.Tools;

public class BehaviorLogger : MonoBehaviour
{
	private string logPath = @"C:\Discord_Scripts\log_folder\diros_behavior.log";
	private DateTime sessionStart = DateTime.Now;

	private void Start()
	{
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(logPath));
			File.AppendAllText(logPath, $"\n=== Session Started: {sessionStart} ===\n");
		}
		catch (Exception e)
		{
			Debug.LogError((object)($"Failed to create log directory: {e.Message}"));
		}
	}

	public void LogEvent(string eventName)
	{
		try
		{
			string timestamp = DateTime.Now.ToString("HH:mm:ss");
			string logEntry = $"[{timestamp}] {eventName}\n";
			File.AppendAllText(logPath, logEntry);
		}
		catch (Exception e)
		{
			Debug.LogError((object)($"Failed to log event: {e.Message}"));
		}
	}

	private void OnDestroy()
	{
		try
		{
			File.AppendAllText(logPath, $"=== Session Ended: {DateTime.Now} ===\n");
		}
		catch (Exception e)
		{
			Debug.LogError((object)($"Failed to log session end: {e.Message}"));
		}
	}
}
