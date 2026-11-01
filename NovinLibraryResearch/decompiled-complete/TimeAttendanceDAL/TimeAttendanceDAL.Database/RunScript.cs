using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AccSqlServerPersistence;
using AccSqlServerPersistence.AssistanceClasses;
using FormCustom.Waiting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using TimeAttendanceDAL.Logger;
using TimeAttendanceDAL.Properties;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Database;

public class RunScript
{
	public static async Task<ReturnedSaveFuncInfo> Run(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		Waiter waiter = new Waiter("در حال پردازش", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
		string connectionStr = Extensions.GetSqlConnectionString(contextGuid);
		SqlConnection cn = new SqlConnection(connectionStr);
		try
		{
			ReturnedSaveFuncInfo result = new ReturnedSaveFuncInfo();
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				Waiter obj = waiter;
				if (obj != null)
				{
					obj.Dispose();
				}
				waiter = new Waiter("در حال بررسی بانک اطلاعاتی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
				SqlCommand cmd = new SqlCommand(" ALTER DATABASE [" + ClsConnection.GetDbName(Extensions.GetSqlConnectionString(contextGuid)) + "] SET AUTO_SHRINK ON WITH NO_WAIT", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					AttLogger.LoggerUi.Information("ALTER DATABASE Is Ok");
					AttLogger.LoggerUi.Information("Create_Functions is working");
					string create_Functions = Resources.Create_Functions;
					Waiter obj2 = waiter;
					if (obj2 != null)
					{
						obj2.Dispose();
					}
					waiter = new Waiter("در حال بررسی توابع بانک اطلاعاتی", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Create_Functions", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						if (result.ErrorMessage.Contains("'format' is not a recognized built-in function name"))
						{
							TimeAttendanceDAL.Utility.Utility.MessageBoxShowError("Sql server version must be higher than 2012", contextGuid);
						}
						AttLogger.LoggerErrorRSFI.Error("Create_Functions has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Create_Functions is ok");
					AttLogger.LoggerUi.Information("Create_Tables is working");
					create_Functions = Resources.Create_Tables;
					Waiter obj3 = waiter;
					if (obj3 != null)
					{
						obj3.Dispose();
					}
					waiter = new Waiter("در حال بررسی جداول", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Create_Tables", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("Create_Tables has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Create_Tables is ok");
					AttLogger.LoggerUi.Information("Create_Triggers is working");
					create_Functions = Resources.Create_Triggers;
					Waiter obj4 = waiter;
					if (obj4 != null)
					{
						obj4.Dispose();
					}
					waiter = new Waiter("در حال بررسی تریگرها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Create_Triggers", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						TimeAttendanceDAL.Utility.Utility.MessageBoxShowByNotif(showPopUp: false, (NotificationEnum)4, result.ErrorMessage, "", 3);
						AttLogger.LoggerErrorRSFI.Error("Create_Triggers has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Create_Triggers is ok");
					AttLogger.LoggerUi.Information("Drop_Fields is working");
					create_Functions = Resources.Drop_Fields;
					Waiter obj5 = waiter;
					if (obj5 != null)
					{
						obj5.Dispose();
					}
					waiter = new Waiter("در حال بررسی فیلدهای قابل حذف", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Drop_Fields", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("Drop_Fields has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Drop_Fields is ok");
					AttLogger.LoggerUi.Information("Create_Views is working");
					create_Functions = Resources.Create_Views;
					Waiter obj6 = waiter;
					if (obj6 != null)
					{
						obj6.Dispose();
					}
					waiter = new Waiter("در حال بررسی ویوها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Create_Views", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("Create_Views has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Create_Views is ok");
					AttLogger.LoggerUi.Information("Create_StoresProcedures is working");
					create_Functions = Resources.Create_StoresProcedures;
					Waiter obj7 = waiter;
					if (obj7 != null)
					{
						obj7.Dispose();
					}
					waiter = new Waiter("در حال بررسی پروسیجرها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "Create_StoresProcedures", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("Create_StoresProcedures has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("Create_StoresProcedures is ok");
					AttLogger.LoggerUi.Information("CreateTaghvim is working");
					create_Functions = Resources.CreateTaghvim;
					Waiter obj8 = waiter;
					if (obj8 != null)
					{
						obj8.Dispose();
					}
					waiter = new Waiter("در حال بررسی تقویم", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "CreateTaghvim", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("CreateTaghvim has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("CreateTaghvim is ok");
					AttLogger.LoggerUi.Information("DataInTables is working");
					create_Functions = Resources.DataInTables;
					Waiter obj9 = waiter;
					if (obj9 != null)
					{
						obj9.Dispose();
					}
					waiter = new Waiter("در حال بررسی داده های جداول", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "DataInTables", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("DataInTables has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("DataInTables is ok");
					AttLogger.LoggerUi.Information("StpCanRunAsync is working");
					create_Functions = Resources.StpCanRunAsync;
					Waiter obj10 = waiter;
					if (obj10 != null)
					{
						obj10.Dispose();
					}
					waiter = new Waiter("در حال بررسی سایر پروسیجرها", (Form)null, (Task)null, false, (EnWaiterType)0, (Control)null, "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs");
					result = await RunScript.Instance.RunAsync(BackupSettingsRepo.GetInstance(contextGuid), "StpCanRunAsync", false, (ENSource)5, create_Functions, connectionStr, 30, default(CancellationToken));
					if (result.HasError)
					{
						AttLogger.LoggerErrorRSFI.Error("StpCanRunAsync has error:" + result.ErrorMessage);
						return result;
					}
					AttLogger.LoggerUi.Information("StpCanRunAsync is ok");
					Waiter obj11 = waiter;
					if (obj11 != null)
					{
						obj11.Dispose();
					}
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				((DbConnection)(object)cn).Close();
				WebErrorLog.ErrorLogInstance.StartLog(ex, "", "Run", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Database\\RunScripts.cs", 158);
			}
			finally
			{
				Waiter obj12 = waiter;
				if (obj12 != null)
				{
					obj12.Dispose();
				}
			}
			return result;
		}
		finally
		{
			((IDisposable)cn)?.Dispose();
		}
	}
}
