using System;
using System.IO;
using System.Reflection;

namespace Stimulsoft.Controls.Win;

internal sealed class StiAssemblyFinder
{
	public static Assembly GetAssembly(string assemblyName)
	{
		string text = assemblyName;
		if (text.ToLower().EndsWith(".dll"))
		{
			text = Path.GetFileNameWithoutExtension(assemblyName);
		}
		else if (text.ToLower().EndsWith(".exe"))
		{
			text = Path.GetFileNameWithoutExtension(assemblyName);
		}
		if (!string.IsNullOrEmpty(text))
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Assembly[] array = assemblies;
			foreach (Assembly assembly in array)
			{
				AssemblyName name = assembly.GetName();
				if (text == name.Name)
				{
					return assembly;
				}
			}
		}
		try
		{
			return Assembly.Load(text);
		}
		catch (FileNotFoundException)
		{
		}
		try
		{
			string assemblyQualifiedName = typeof(StiAssemblyFinder).AssemblyQualifiedName;
			if (text.ToLowerInvariant().StartsWith("stimulsoft"))
			{
				int num = assemblyQualifiedName.IndexOf(",", StringComparison.Ordinal);
				assemblyQualifiedName = assemblyQualifiedName.Substring(num + 1);
				assemblyQualifiedName = assemblyQualifiedName.Replace("Stimulsoft.Base", text);
				return Assembly.Load(assemblyQualifiedName);
			}
		}
		catch (FileNotFoundException)
		{
		}
		try
		{
			if (File.Exists(assemblyName))
			{
				return Assembly.LoadFrom(assemblyName);
			}
		}
		catch (FileNotFoundException)
		{
		}
		return null;
	}
}
