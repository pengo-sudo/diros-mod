using UnityEngine;

namespace Diros.Tools;

public static class Extensions
{
	public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		T val = gameObject.GetComponent<T>();
		if ((Object)(object)val == (Object)null)
		{
			val = gameObject.AddComponent<T>();
		}
		return val;
	}

	public static void Obliterate(this Component component)
	{
		Object.Destroy((Object)(object)component);
	}

	public static void Obliterate(this GameObject gameObject)
	{
		Object.Destroy((Object)(object)gameObject);
	}

	public static float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
	{
		return toMin + (value - fromMin) / (fromMax - fromMin) * (toMax - toMin);
	}

	public static float Distance(this Vector3 a, Vector3 b)
	{
		return Vector3.Distance(a, b);
	}
}
