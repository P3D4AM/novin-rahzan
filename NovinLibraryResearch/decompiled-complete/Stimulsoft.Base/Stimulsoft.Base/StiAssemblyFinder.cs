using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base;

public sealed class StiAssemblyFinder
{
	public static Assembly GetAssembly(string assemblyName, bool useAssemblyFromDataAdaptersRepository = true)
	{
		string currentDirectory = Directory.GetCurrentDirectory();
		try
		{
			string location = typeof(StiAssemblyFinder).Assembly.Location;
			if (!string.IsNullOrWhiteSpace(location) && StiBaseOptions.AllowSetCurrentDirectory)
			{
				try
				{
					location = Path.GetDirectoryName(location);
					Directory.SetCurrentDirectory(location);
				}
				catch
				{
				}
			}
			string text = assemblyName;
			if (text.EndsWithInvariantIgnoreCase(".dll") || text.EndsWithInvariantIgnoreCase(".exe"))
			{
				text = Path.GetFileNameWithoutExtension(assemblyName);
			}
			Assembly assembly = GetAssemblyFromCurrentDomain(text);
			if (assembly != null)
			{
				return assembly;
			}
			if (useAssemblyFromDataAdaptersRepository)
			{
				assembly = GetAssemblyFromDataAdaptersRepository(assemblyName);
			}
			if (assembly != null)
			{
				return assembly;
			}
			assembly = GetAssemblyFromAssemblyLoad(text);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = GetAssemblyFromAssemblyLoadWithSpecifiedVersion(text);
			if (assembly != null)
			{
				return assembly;
			}
			assembly = GetAssemblyFromAssemblyLoadFrom(assemblyName);
			if (assembly != null)
			{
				return assembly;
			}
			if (assemblyName != text)
			{
				assembly = GetAssemblyFromAssemblyLoad(assemblyName);
				if (assembly != null)
				{
					return assembly;
				}
			}
			return GetAssemblyFromAssemblyLoadWithPartialName(text);
		}
		finally
		{
			if (StiBaseOptions.AllowSetCurrentDirectory)
			{
				Directory.SetCurrentDirectory(currentDirectory);
			}
		}
	}

	private static Assembly GetAssemblyFromCurrentDomain(string name)
	{
		if (!string.IsNullOrEmpty(name))
		{
			return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault((Assembly a) => a.GetName().Name == name);
		}
		return null;
	}

	private static Assembly GetAssemblyFromDataAdaptersRepository(string name)
	{
		try
		{
			string dataAdapterPath = StiDataAdaptersRepositary.GetDataAdapterPath(name);
			if (dataAdapterPath != null && File.Exists(dataAdapterPath))
			{
				return Assembly.LoadFrom(dataAdapterPath);
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	internal static Assembly GetAssemblyFromDataAdaptersFolder(string assemblyName, string assemblyFolder)
	{
		try
		{
			string assembly = Path.Combine(assemblyFolder, assemblyName);
			string dataAdapterPathFromRoot = StiDataAdaptersRepositary.GetDataAdapterPathFromRoot(assembly);
			if (dataAdapterPathFromRoot != null && File.Exists(dataAdapterPathFromRoot))
			{
				return Assembly.LoadFrom(dataAdapterPathFromRoot);
			}
		}
		catch (Exception)
		{
		}
		return null;
	}

	private static Assembly GetAssemblyFromAssemblyLoad(string name)
	{
		try
		{
			return Assembly.Load(name);
		}
		catch (FileNotFoundException)
		{
		}
		return null;
	}

	private static Assembly GetAssemblyFromAssemblyLoadWithSpecifiedVersion(string name)
	{
		try
		{
			if (name.ToLowerInvariant().StartsWith("stimulsoft", StringComparison.InvariantCulture))
			{
				string assemblyQualifiedName = typeof(StiAssemblyFinder).AssemblyQualifiedName;
				int num = assemblyQualifiedName.IndexOf(",", StringComparison.InvariantCulture);
				assemblyQualifiedName = assemblyQualifiedName.Substring(num + 1);
				assemblyQualifiedName = assemblyQualifiedName.Replace("Stimulsoft.Base", name);
				return Assembly.Load(assemblyQualifiedName);
			}
		}
		catch (FileNotFoundException)
		{
		}
		return null;
	}

	private static Assembly GetAssemblyFromAssemblyLoadFrom(string name)
	{
		try
		{
			if (File.Exists(name))
			{
				return Assembly.LoadFrom(name);
			}
		}
		catch (FileNotFoundException)
		{
		}
		return null;
	}

	private static Assembly GetAssemblyFromAssemblyLoadWithPartialName(string name)
	{
		return Assembly.LoadWithPartialName(name);
	}
}
