using System;
using System.Reflection;
using Stimulsoft.Base.Blocks;

namespace Stimulsoft.Base.Blockly;

public class StiBlocksCreator
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
				Assembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Blockly, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a");
			}
			catch
			{
			}
			isInited = true;
		}
	}

	internal static IStiBlocksParser GetBlockParse()
	{
		try
		{
			LoadAssembly();
			if (!IsAssemblyLoaded)
			{
				return null;
			}
			Type type = StiTypeFinder.GetType("Stimulsoft.Blockly.StiBlocksParser, Stimulsoft.Blockly");
			if (type != null)
			{
				return StiActivator.CreateObject(type, new object[0]) as IStiBlocksParser;
			}
		}
		catch
		{
			return null;
		}
		return null;
	}
}
