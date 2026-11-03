using System;
using System.Reflection;
using Stimulsoft.Base;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report;

public static class StiDatabaseAssembly
{
	private static Assembly databaseAssembly;

	public static Assembly GetAssembly()
	{
		if (databaseAssembly != null)
		{
			return databaseAssembly;
		}
		try
		{
			databaseAssembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Database, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
		}
		catch
		{
		}
		try
		{
			if (databaseAssembly == null)
			{
				databaseAssembly = StiAssemblyFinder.GetAssembly("Stimulsoft.Database.Wpf, Version=2022.1.1.0, Culture=neutral, PublicKeyToken=ebe6666cba19647a", true);
			}
		}
		catch
		{
		}
		return databaseAssembly;
	}

	public static Type GetConnectionStringHelper(string typeName)
	{
		if (typeName == null)
		{
			return null;
		}
		Assembly assembly = GetAssembly();
		if (assembly == null)
		{
			return null;
		}
		return StiTypeFinder.GetType("Stimulsoft.Database.Connection." + typeName);
	}

	public static Type GetWpfConnectionStringHelper(string typeName)
	{
		if (typeName == null)
		{
			return null;
		}
		Assembly assembly = GetAssembly();
		if (assembly == null)
		{
			return null;
		}
		return StiTypeFinder.GetType("Stimulsoft.Database.Wpf.Connection." + typeName);
	}

	public static string EditConnectionString(StiDatabase database, string connectionString, string typeName)
	{
		Type connectionStringHelper = GetConnectionStringHelper(typeName);
		if (connectionStringHelper == null)
		{
			return connectionString;
		}
		MethodInfo method = connectionStringHelper.GetMethod("EditConnectionString", BindingFlags.Static | BindingFlags.Public);
		return method.Invoke(null, new object[1] { connectionString }) as string;
	}

	public static bool CanEditConnectionString(string typeName)
	{
		if (!(GetConnectionStringHelper(typeName) != null))
		{
			return GetWpfConnectionStringHelper(typeName) != null;
		}
		return true;
	}
}
