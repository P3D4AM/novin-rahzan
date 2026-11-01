using System;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using EntityCache;
using FormCustom.Cache;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Database;

public class CLSDatabase : IHasContext
{
	public string ContextGuid { get; set; }

	public CLSDatabase(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<ReturnedSaveFuncInfo> CreateDbIfNotExist(string contextGuid)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (Extensions.GetSqlConnectionString(contextGuid) != "")
		{
			return ret;
		}
		string Database = "Attendance";
		int i = 0;
		try
		{
			for (; i < 100; i++)
			{
				string connectionString = (await BaseDataBaseAction.Instance.CreateDataBaseAsync(BackupSettingsRepo.GetInstance(contextGuid), Database + i)).ConnectionString;
				if (!string.IsNullOrEmpty(connectionString) && ClsConnection.CheckConnectionString(connectionString))
				{
					ClsCache cache = ClsCache.GetCache(contextGuid);
					if (cache != null)
					{
						cache.Dispose();
					}
					CacheContext context = new CacheContext($"CreateDataBaseGuid{Guid.NewGuid()}");
					await EntityCacheConfig.InitAsync(context.ContextGuid, connectionString, SystemInfoClass.SystemInfo(), VersionManagment.Version, (EnConnectionType)1);
					ClsConnection.ContextGuid = context.ContextGuid;
					return ret;
				}
			}
		}
		catch (Exception ex)
		{
			ret.AddReturnedValue((ReturnedState)2, "ایجاد بانک اطلاعاتی با خطا مواجه شد", "", "CreateDbIfNotExist", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\CLSDatabase.cs", 51);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "CreateDbIfNotExist", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\CLSDatabase.cs", 52);
		}
		ret.AddReturnedValue((ReturnedState)2, "بانک اطلاعاتی ایجاد نشد", "", "CreateDbIfNotExist", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\CLSDatabase.cs", 54);
		return ret;
	}
}
