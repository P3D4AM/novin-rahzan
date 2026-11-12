using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;

namespace Stimulsoft.Base;

public class StiDataAssemblyHelper
{
	private string assemblyName;

	private string assemblyFolder;

	private Assembly assembly;

	public bool IsAllowed => Assembly != null;

	private Assembly Assembly
	{
		get
		{
			try
			{
				bool flag = !string.IsNullOrEmpty(assemblyFolder);
				if (assembly == null && flag)
				{
					assembly = StiAssemblyFinder.GetAssemblyFromDataAdaptersFolder(assemblyName, assemblyFolder);
				}
				if (assembly == null)
				{
					assembly = StiAssemblyFinder.GetAssembly(assemblyName, !flag);
				}
				if (assembly == null)
				{
					throw new Exception("\"" + assemblyName + "\" assembly is not found!");
				}
				return assembly;
			}
			catch
			{
				return null;
			}
		}
	}

	internal DbConnection CreateConnection(string typeName, string connectionString)
	{
		return CreateObject(typeName, connectionString) as DbConnection;
	}

	internal DbDataAdapter CreateAdapter(string typeName, string query, DbConnection connection)
	{
		return CreateObject(typeName, query, connection) as DbDataAdapter;
	}

	internal DbCommand CreateCommand(string typeName, string query, DbConnection connection)
	{
		return CreateObject(typeName, query, connection) as DbCommand;
	}

	internal DbParameter CreateParameterWithValueAndSize(string typeName, string parameterName, object value, int size)
	{
		return CreateObject(typeName, parameterName, value, size) as DbParameter;
	}

	internal DbParameter CreateParameterWithValue(string typeName, string parameterName, object value)
	{
		return CreateObject(typeName, parameterName, value) as DbParameter;
	}

	internal DbParameter CreateParameterWithType(string typeName, string parameterName, int type, Type dbType)
	{
		return CreateObject(typeName, parameterName, StiConvert.ChangeType(type, dbType)) as DbParameter;
	}

	internal DbParameter CreateParameterWithTypeAndSize(string typeName, string parameterName, int type, int size, Type dbType)
	{
		return CreateObject(typeName, parameterName, StiConvert.ChangeType(type, dbType), size) as DbParameter;
	}

	internal DbCommandBuilder CreateCommandBuilder(string typeName)
	{
		return CreateObject(typeName) as DbCommandBuilder;
	}

	internal DbConnectionStringBuilder CreateConnectionStringBuilder(string typeName, string connectionString = null)
	{
		return CreateObject(typeName, connectionString) as DbConnectionStringBuilder;
	}

	internal DbDataSourceEnumerator CreateDataSourceEnumerator(string typeName)
	{
		return CreateObject(typeName) as DbDataSourceEnumerator;
	}

	internal void DeriveParameters(string typeName, string methodName, DbCommand command)
	{
		try
		{
			Type type = GetType(typeName);
			type.GetMethod(methodName)?.Invoke(null, new object[1] { command });
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
	}

	internal object CreateObject(string name, params object[] args)
	{
		try
		{
			Type type = GetType(name);
			Type[] types = (from a in args?.ToList()
				select (a == null) ? typeof(object) : a.GetType()).ToArray();
			ConstructorInfo constructor = type.GetConstructor(types);
			return constructor.Invoke(args);
		}
		catch (Exception ex)
		{
			if (ex.InnerException != null)
			{
				throw ex.InnerException;
			}
			throw;
		}
	}

	internal Type GetType(string typeName)
	{
		Assembly assembly = Assembly;
		if (assembly == null)
		{
			throw new Exception("\"" + assemblyName + "\" assembly is not found!");
		}
		Type type = assembly.GetType(typeName);
		if (type == null)
		{
			throw new Exception("\"" + typeName + "\" type is not found!");
		}
		return type;
	}

	internal StiDataAssemblyHelper(string assemblyName, string assemblyFolder = null)
	{
		this.assemblyName = assemblyName;
		this.assemblyFolder = assemblyFolder;
	}
}
