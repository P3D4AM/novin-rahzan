using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Ninject.Components;

namespace Ninject.Modules;

public class AssemblyNameRetriever : NinjectComponent, IAssemblyNameRetriever, INinjectComponent, IDisposable
{
	private class AssemblyChecker : MarshalByRefObject
	{
		public IEnumerable<AssemblyName> GetAssemblyNames(IEnumerable<string> filenames, Predicate<Assembly> filter)
		{
			List<AssemblyName> list = new List<AssemblyName>();
			foreach (string filename in filenames)
			{
				Assembly assembly;
				if (File.Exists(filename))
				{
					try
					{
						assembly = Assembly.LoadFrom(filename);
					}
					catch (BadImageFormatException)
					{
						continue;
					}
				}
				else
				{
					try
					{
						assembly = Assembly.Load(filename);
					}
					catch (FileLoadException)
					{
						continue;
					}
					catch (FileNotFoundException)
					{
						continue;
					}
				}
				if (filter(assembly))
				{
					list.Add(assembly.GetName(copiedName: false));
				}
			}
			return list;
		}
	}

	public IEnumerable<AssemblyName> GetAssemblyNames(IEnumerable<string> filenames, Predicate<Assembly> filter)
	{
		Type typeFromHandle = typeof(AssemblyChecker);
		AppDomain appDomain = CreateTemporaryAppDomain();
		try
		{
			return ((AssemblyChecker)appDomain.CreateInstanceAndUnwrap(typeFromHandle.Assembly.FullName, typeFromHandle.FullName ?? string.Empty)).GetAssemblyNames(filenames.ToArray(), filter);
		}
		finally
		{
			AppDomain.Unload(appDomain);
		}
	}

	private static AppDomain CreateTemporaryAppDomain()
	{
		return AppDomain.CreateDomain("NinjectModuleLoader", AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation);
	}
}
