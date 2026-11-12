using System;
using ExcelDataReader.Log.Logger;

namespace ExcelDataReader.Log;

public static class Log
{
	private static readonly object LockObject = new object();

	private static Type logType = typeof(NullLogFactory);

	private static ILogFactory factoryInstance;

	public static void InitializeWith<T>() where T : ILogFactory, new()
	{
		lock (LockObject)
		{
			logType = typeof(T);
			factoryInstance = null;
		}
	}

	public static ILog GetLoggerFor(Type loggingType)
	{
		ILogFactory logFactory = factoryInstance;
		if (logFactory == null)
		{
			lock (LockObject)
			{
				if (logFactory == null)
				{
					logFactory = (factoryInstance = (ILogFactory)Activator.CreateInstance(logType));
				}
			}
		}
		return logFactory.Create(loggingType);
	}
}
