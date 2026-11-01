using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Database;

public class BaseYearSalary
{
	public static async Task<ReturnedSaveFuncInfo> InsertAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string Script = File.ReadAllText(Path.Combine(Application.StartupPath, "BaseYearSalary.txt"));
			if (Script != null && Script.Length > 0)
			{
				SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
				await ((DbConnection)(object)cn).OpenAsync(token);
				ret = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "BaseYearSalary.txt", false, (ENSource)5, Script, Extensions.GetSqlConnectionString(contextGuid), 30, default(CancellationToken));
				((DbConnection)(object)cn).Close();
			}
		}
		catch (Exception ex)
		{
			ret.AddError(ex.Message, "", "InsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\BaseYearSalary.cs", 31, (StringBuilder)null);
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "InsertAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\BaseYearSalary.cs", 32);
		}
		return ret;
	}
}
