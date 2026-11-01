using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using Serilog;
using TimeAttendanceDAL.Dal.Other;

namespace TimeAttendanceDAL.Database;

public class InsertMashaghel
{
	public static async Task<ReturnedSaveFuncInfo> InsertAsync(string contextGuid, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			if (!(await SettingAttendance.GetBoolValueAsync(contextGuid, "insertedData", def: false, 0, token)))
			{
				string Script = File.ReadAllText(Path.Combine(Application.StartupPath, "jobs.txt"));
				await ((DbConnection)(object)cn).OpenAsync(token);
				ret = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "jobs.txt", true, (ENSource)5, Script, Extensions.GetSqlConnectionString(contextGuid), 30, default(CancellationToken));
				((DbConnection)(object)cn).Close();
				await SettingAttendance.SetValueAsync(contextGuid, "insertedData", "true", 0, token);
			}
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
