using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;

namespace TimeAttendanceDAL.View.Taradod;

public class HardWareCodeNotAssigned
{
	public int PersonNO { get; set; }

	public HardWareCodeNotAssigned(SqlDataReader dr)
	{
		if (((DbDataReader)(object)dr)["PersonNO"].ToString() != DBNull.Value.ToString())
		{
			PersonNO = (int)((DbDataReader)(object)dr)["PersonNO"];
		}
	}

	public static async Task<List<HardWareCodeNotAssigned>> GetAllAsync(string contextGuid, List<int> logOnPersonelIds, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<HardWareCodeNotAssigned> ret = new List<HardWareCodeNotAssigned>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_HardwareCodeCus_NotDefine", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@LogonPersonelIDs", (object)string.Join(",", logOnPersonelIds));
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new HardWareCodeNotAssigned(dr));
						}
						((DbConnection)(object)cn).Close();
						((DbDataReader)(object)dr).Close();
						return ret;
					}
					finally
					{
						((IDisposable)dr)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)cn)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
