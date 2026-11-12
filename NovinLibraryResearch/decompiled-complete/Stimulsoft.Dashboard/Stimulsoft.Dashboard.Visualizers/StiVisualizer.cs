using System;
using System.Collections;
using System.Linq;
using Stimulsoft.Base;

namespace Stimulsoft.Dashboard.Visualizers;

public abstract class StiVisualizer
{
	private static Hashtable typeToGdiVisualizer = new Hashtable();

	private static Hashtable typeToWpfVisualizer = new Hashtable();

	private static object lockObject = new object();

	public static StiVisualizer GetVisualizer(Type type, StiGuiMode guiMode)
	{
		lock (lockObject)
		{
			Hashtable hashtable = ((guiMode == StiGuiMode.Gdi) ? typeToGdiVisualizer : typeToWpfVisualizer);
			StiVisualizer stiVisualizer = hashtable[type] as StiVisualizer;
			if (stiVisualizer != null)
			{
				return stiVisualizer;
			}
			switch (guiMode)
			{
			case StiGuiMode.Wpf:
				stiVisualizer = GetWpfVisualizer(type);
				break;
			case StiGuiMode.Gdi:
				stiVisualizer = GetGdiVisualizer(type);
				break;
			}
			if (stiVisualizer == null)
			{
				throw new Exception($"StiVisualizer for '{type}' is not found!");
			}
			hashtable[type] = stiVisualizer;
			return stiVisualizer;
		}
	}

	private static StiVisualizer GetGdiVisualizer(Type type)
	{
		string text = ((type.GetCustomAttributes(typeof(StiGdiVisualizerAttribute), inherit: true).FirstOrDefault() is StiGdiVisualizerAttribute stiGdiVisualizerAttribute) ? stiGdiVisualizerAttribute.VisualizerTypeName : null);
		if (text == null)
		{
			return null;
		}
		return StiActivator.CreateObject(text, new object[0]) as StiVisualizer;
	}

	private static StiVisualizer GetWpfVisualizer(Type type)
	{
		string text = ((type.GetCustomAttributes(typeof(StiWpfVisualizerAttribute), inherit: true).FirstOrDefault() is StiWpfVisualizerAttribute stiWpfVisualizerAttribute) ? stiWpfVisualizerAttribute.VisualizerTypeName : null);
		if (text == null)
		{
			return null;
		}
		return StiActivator.CreateObject(text, new object[0]) as StiVisualizer;
	}
}
