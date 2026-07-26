using System;
using UnityEngine;
using Diros.Rigging;

namespace Diros.Plugin;

public class InputHandler : MonoBehaviour
{
	public static InputHandler instance;

	private void Awake()
	{
		instance = this;
	}

	private void Update()
	{
		try
		{
			if (HeadDriver.instance != null && Plugin.instance.Enabled)
			{
				if (Input.GetKeyDown(KeyCode.P))
				{
					Plugin.instance.Enabled = !Plugin.instance.Enabled;
				}
			}
		}
		catch
		{
		}
	}
}
