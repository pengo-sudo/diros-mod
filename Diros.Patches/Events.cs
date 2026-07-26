using System;

namespace Diros.Patches;

public static class Events
{
	public static event EventHandler<EventArgs> GameInitialized;

	public static void OnGameInitialized()
	{
		GameInitialized?.Invoke(null, EventArgs.Empty);
	}
}
