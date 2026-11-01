using System;
using System.Data.Common;
using System.Data.SqlClient;
using PacketParser;
using PacketParser.Entities;
using TimeAttendanceDAL.Logger;

namespace TimeAttendanceDAL;

public class ClsConnection
{
	public static string connectionString;

	public static string ContextGuid;

	public static bool NeedBazsazi { get; set; }

	public static string GetServerName(string contextGuid)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return new SqlConnectionStringBuilder(Extensions.GetSqlConnectionString(contextGuid)).DataSource;
	}

	public static string GetDbName(string connectionString)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return new SqlConnectionStringBuilder(connectionString).InitialCatalog;
	}

	public static bool CheckConnectionString(string constr)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_0053: Expected O, but got Unknown
		try
		{
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(constr)
			{
				AsynchronousProcessing = true
			};
			SqlConnection val2 = new SqlConnection();
			((DbConnection)val2).ConnectionString = ((DbConnectionStringBuilder)(object)val).ConnectionString;
			AttLogger.LoggerUi.Information("checkConnectionStrindg() ... " + ((DbConnectionStringBuilder)(object)val).ConnectionString);
			((DbConnection)val2).Open();
			AttLogger.LoggerUi.Information("checkConnectionStrindg() is ok");
			((DbConnection)val2).Close();
			connectionString = constr;
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex.Message, "connstr=" + constr, "CheckConnectionString", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\ClsConnection.cs", 45);
			AttLogger.LoggerException.Error("checkConnectionStrindg() has error and Exception error sended for supporter");
			return false;
		}
	}
}
