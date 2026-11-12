using System;
using System.Reflection;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Base.Blockly;

public class StiBlocklyEditorCreator
{
	private static object lockObject = new object();

	private static bool isInited;

	private static Assembly assembly;

	internal static bool IsAssemblyLoaded => Assembly != null;

	internal static Assembly Assembly
	{
		get
		{
			LoadAssembly();
			return assembly;
		}
		set
		{
			assembly = value;
		}
	}

	internal static void LoadAssembly()
	{
		if (isInited)
		{
			return;
		}
		lock (lockObject)
		{
			try
			{
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.BlocklyEditor.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiBlocklyEditorWindow GetBlocklyEditorWindow(string xml, IStiReport report, object eventValue, bool showCurrentValue)
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = Assembly.GetType("Stimulsoft.BlocklyEditor.Wpf.StiBlocklyEditorWindow");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[4] { xml, report, eventValue, showCurrentValue }) as IStiBlocklyEditorWindow;
			}
		}
		catch
		{
			return null;
		}
		return null;
	}
}
