using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;

namespace TimeAttendancePrint;

public class STIPrint
{
	private static string _sqlQuery;

	public static string SqlQuery
	{
		get
		{
			return _sqlQuery;
		}
		set
		{
			_sqlQuery = Text.FixString(value);
		}
	}

	public static async Task<DataSet> SetSettingsAsync(string contextGuid, DataSet DS, CancellationToken token)
	{
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand val = new SqlCommand("sp_SettingAttendance_SelectInfSaleKari", cn);
			SqlDataAdapter da = new SqlDataAdapter(val);
			((DbCommand)(object)val).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			DS.Tables.Add("sp_SettingAttendance_SelectInfSaleKari");
			((DbDataAdapter)(object)da).Fill(DS.Tables["sp_SettingAttendance_SelectInfSaleKari"]);
			((DbConnection)(object)cn).Close();
			return DS;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
