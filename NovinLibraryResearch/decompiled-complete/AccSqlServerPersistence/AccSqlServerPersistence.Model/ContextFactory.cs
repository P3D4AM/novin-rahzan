using System;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Model;

public static class ContextFactory
{
	public static Acc_ModelContext GetAccInstance(string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return new Acc_ModelContext(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber);
	}

	public static Global_ModelContext GetGlobalInstance(string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return new Global_ModelContext(connectionString, tranName, debugerMessage, timeout, memberName, sourceFilePath, sourceLineNumber);
	}

	public static Adv_ModelContext GetAdvInstance(string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return new Adv_ModelContext(connectionString, tranName, debugerMessage, timeout, memberName, sourceFilePath, sourceLineNumber);
	}

	public static Dpt_ModelContext GetDepartmentInstance(string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return new Dpt_ModelContext(connectionString, tranName, debugerMessage, timeout, memberName, sourceFilePath, sourceLineNumber);
	}

	public static Att_ModelContext GetAttInstance(string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return new Att_ModelContext(connectionString, tranName, debugerMessage, timeout, memberName, sourceFilePath, sourceLineNumber);
	}

	public static BaseModelContext GetInstance(EnCacheConfig config, string connectionString, string tranName, int timeout, string debugerMessage, string memberName, string sourceFilePath, int sourceLineNumber)
	{
		return config switch
		{
			EnCacheConfig.Accounting => GetAccInstance(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber), 
			EnCacheConfig.Advertise => GetAdvInstance(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber), 
			EnCacheConfig.Global => GetGlobalInstance(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber), 
			EnCacheConfig.Attendance => GetAttInstance(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber), 
			EnCacheConfig.Department => GetDepartmentInstance(connectionString, tranName, timeout, debugerMessage, memberName, sourceFilePath, sourceLineNumber), 
			_ => throw new NotImplementedException(), 
		};
	}
}
