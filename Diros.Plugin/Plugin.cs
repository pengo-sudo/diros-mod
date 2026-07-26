using System;
using System.IO;
using System.Text.RegularExpressions;
using BepInEx;
using UnityEngine;
using Diros.Animators;
using Diros.Menus;
using Diros.Patches;
using Diros.Rigging;
using Diros.Tools;

namespace Diros.Plugin;

[BepInPlugin("com.diros.gorillatag.diros", "Diros", "1.0.0")]
public class Plugin : BaseUnityPlugin
{
	public static Plugin instance;

	private static bool _enabled = true;

	public AnimatorBase dirosAnimator;

	public ComputerGUI computerGUI;

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
			if (value)
			{
				((Component)this).gameObject.GetOrAddComponent<InputHandler>();
				((Component)this).gameObject.GetOrAddComponent<Rig>();
				dirosAnimator = ((Component)this).gameObject.GetOrAddComponent<DirosAnimator>();
				computerGUI = ((Component)this).gameObject.GetOrAddComponent<ComputerGUI>();
				((Behaviour)dirosAnimator).enabled = true;
				return;
			}
			if ((Object)(object)InputHandler.instance != (Object)null)
			{
				((Component)(object)InputHandler.instance).Obliterate();
			}
			if ((Object)(object)Rig.Instance != (Object)null)
			{
				((Component)(object)Rig.Instance).Obliterate();
			}
			if ((Object)(object)dirosAnimator != (Object)null)
			{
				((Component)(object)dirosAnimator).Obliterate();
			}
			if ((Object)(object)computerGUI != (Object)null)
			{
				((Component)(object)computerGUI).Obliterate();
			}
		}
	}

	private void Awake()
	{
		instance = this;
		Logging.Init();
		try
		{
			string path = Paths.ConfigPath + "/BepInEx.cfg";
			string input = File.ReadAllText(path);
			input = Regex.Replace(input, "HideManagerGameObject = .+", "HideManagerGameObject = true");
			File.WriteAllText(path, input);
		}
		catch (Exception e)
		{
			Logging.Exception(e);
		}
	}

	private void Start()
	{
		Events.GameInitialized += OnGameInitialized;
	}

	private void OnEnable()
	{
		HarmonyPatches.ApplyHarmonyPatches();
	}

	private void OnDisable()
	{
		HarmonyPatches.RemoveHarmonyPatches();
	}

	private void OnGameInitialized(object sender, EventArgs e)
	{
		Enabled = true;
		Debug.Log("Diros initialized");
	}
}
