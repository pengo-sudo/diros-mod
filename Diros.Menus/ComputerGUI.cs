using UnityEngine;

namespace Diros.Menus;

public class ComputerGUI : MonoBehaviour
{
	public static ComputerGUI Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}
}
