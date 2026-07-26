using HarmonyLib;
using Diros.Patches;

namespace Diros.Plugin;

public static class HarmonyPatches
{
	private static Harmony harmonyInstance;

	public static void ApplyHarmonyPatches()
	{
		if (harmonyInstance != null)
		{
			return;
		}
		harmonyInstance = new Harmony("com.diros.gorillatag.diros");
		harmonyInstance.PatchAll();
	}

	public static void RemoveHarmonyPatches()
	{
		if (harmonyInstance != null)
		{
			harmonyInstance.UnpatchSelf();
			harmonyInstance = null;
		}
	}
}
