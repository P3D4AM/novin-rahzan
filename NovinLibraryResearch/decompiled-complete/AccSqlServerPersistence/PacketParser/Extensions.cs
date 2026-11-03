using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using EntityCache;
using EntityCache.Core;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser;

public static class Extensions
{
	public static SqlParameter[] CastToMicrosoft(this IEnumerable<SqlParameter> sqlParameters)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		List<SqlParameter> list = new List<SqlParameter>();
		if (sqlParameters != null && sqlParameters.Any())
		{
			foreach (SqlParameter sqlParameter in sqlParameters)
			{
				SqlParameter val = new SqlParameter();
				((DbParameter)val).ParameterName = ((DbParameter)(object)sqlParameter).ParameterName;
				val.SqlDbType = sqlParameter.SqlDbType;
				val.CompareInfo = sqlParameter.CompareInfo;
				((DbParameter)val).Direction = ((DbParameter)(object)sqlParameter).Direction;
				((DbParameter)val).IsNullable = ((DbParameter)(object)sqlParameter).IsNullable;
				val.LocaleId = sqlParameter.LocaleId;
				val.Offset = sqlParameter.Offset;
				val.Precision = sqlParameter.Precision;
				val.Scale = sqlParameter.Scale;
				((DbParameter)val).Size = ((DbParameter)(object)sqlParameter).Size;
				((DbParameter)val).SourceColumn = ((DbParameter)(object)sqlParameter).SourceColumn;
				((DbParameter)val).SourceColumnNullMapping = ((DbParameter)(object)sqlParameter).SourceColumnNullMapping;
				((DbParameter)val).SourceVersion = ((DbParameter)(object)sqlParameter).SourceVersion;
				val.SqlValue = sqlParameter.SqlValue;
				val.TypeName = sqlParameter.TypeName;
				val.UdtTypeName = sqlParameter.UdtTypeName;
				((DbParameter)val).Value = ((DbParameter)(object)sqlParameter).Value;
				val.XmlSchemaCollectionDatabase = sqlParameter.XmlSchemaCollectionDatabase;
				val.XmlSchemaCollectionName = sqlParameter.XmlSchemaCollectionName;
				val.XmlSchemaCollectionOwningSchema = sqlParameter.XmlSchemaCollectionOwningSchema;
				list.Add(val);
			}
		}
		return list.ToArray();
	}

	public static string GetSqlConnectionString(this string contextGuid)
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new NullReferenceException("contextGuid");
		}
		if (contextGuid.GetCahce() == null)
		{
			throw new ArgumentException("cache for contextGuid(" + contextGuid + ") does not exists . ClsCache.GetCacheHistory:" + ClsCache.GetCacheHistory(contextGuid));
		}
		int num = 0;
		try
		{
			IEnumerable<IDatabaseManager> databaseManager = contextGuid.GetCahce().DatabaseManager;
			num = 1;
			if (databaseManager == null || !databaseManager.Any())
			{
				return null;
			}
			num = 2;
			return (databaseManager?.FirstOrDefault() as ContextHolder)?.ConnectionString;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"{num}", "GetSqlConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 105);
		}
		return null;
	}

	public static ClsCache GetCahce(this string contextGuid)
	{
		return ClsCache.GetCache(contextGuid);
	}

	public static async Task<BaseModelContext> GetDatabaseContextAsync(this string contextGuid, string tranName)
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new NullReferenceException("contextGuid");
		}
		if (string.IsNullOrEmpty(tranName))
		{
			throw new NullReferenceException("tranName");
		}
		try
		{
			IEnumerable<IDatabaseManager> databaseManager = contextGuid.GetCahce().DatabaseManager;
			if (databaseManager == null || !databaseManager.Any())
			{
				return null;
			}
			return await ((ContextHolder)databaseManager.FirstOrDefault()).GetContextAsync(tranName, 60000, "GetDatabaseContextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 120);
		}
		catch (ThreadAbortException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetDatabaseContextAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 123);
			return null;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> BeginTransactionAsync(this string contextGuid, string tranName, int timeout = 60000, EnCacheConfig config = EnCacheConfig.Accounting, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new NullReferenceException("contextGuid");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (string.IsNullOrEmpty(tranName))
			{
				tranName = Guid.NewGuid().ToString();
			}
			switch (config)
			{
			case EnCacheConfig.Accounting:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AccDatabaseManager.BeginTransactionAsync(tranName, timeout, memberName, sourceFilePath, sourceLineNumber), ReturnedState.None, "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 142);
				break;
			}
			case EnCacheConfig.Global:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).GlobalDatabaseManager.BeginTransactionAsync(tranName, timeout, memberName, sourceFilePath, sourceLineNumber), ReturnedState.None, "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 145);
				break;
			}
			case EnCacheConfig.Advertise:
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await ClsCache.GetCache(contextGuid).AdvDatabaseManager.BeginTransactionAsync(tranName, timeout, memberName, sourceFilePath, sourceLineNumber), ReturnedState.None, "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 148);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 154);
			res.AddReturnedValue(ex, "", "BeginTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 155);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> TransactionDestinyAsync(this string contextGuid, string tranName, bool hasError)
	{
		if (string.IsNullOrEmpty(contextGuid))
		{
			throw new NullReferenceException("contextGuid");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(contextGuid, tranName, hasError, disposeContext: true, "TransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 166), ReturnedState.None, "TransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 166);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 170);
			res.AddReturnedValue(ex, "", "TransactionDestinyAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Extensions.cs", 171);
		}
		return res;
	}

	public static T MinOrDefault<T>(this IEnumerable<T> source, T defaultValue)
	{
		if (source == null || !source.Any())
		{
			return defaultValue;
		}
		return source.Min();
	}

	public static T MaxOrDefault<T>(this IEnumerable<T> source, T defaultValue)
	{
		if (source == null || !source.Any())
		{
			return defaultValue;
		}
		return source.Max();
	}

	public static ReferenceCollectionBuilder OnDeleteNoAction(this ReferenceCollectionBuilder builder)
	{
		return builder.OnDelete((DeleteBehavior)5);
	}

	public static ReferenceCollectionBuilder NotRequiredInSqlite(this ReferenceCollectionBuilder builder)
	{
		return builder;
	}

	public static ReferenceReferenceBuilder OnDeleteNoAction(this ReferenceReferenceBuilder builder)
	{
		return builder.OnDelete((DeleteBehavior)5);
	}

	public static IndexBuilder IsCluster(this IndexBuilder builder, bool isCluster = true)
	{
		return SqlServerIndexBuilderExtensions.IsClustered(builder, isCluster);
	}

	public static KeyBuilder IsCluster(this KeyBuilder builder, bool isCluster = true)
	{
		return SqlServerKeyBuilderExtensions.IsClustered(builder, isCluster);
	}
}
