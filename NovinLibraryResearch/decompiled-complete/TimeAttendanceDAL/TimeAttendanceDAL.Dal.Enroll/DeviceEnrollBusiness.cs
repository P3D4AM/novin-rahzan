using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.Enroll;

public class DeviceEnrollBusiness
{
	public string ContextGuid = "";

	public List<DeviceEnroll> DeviceEnrolls { get; set; }

	public async Task<List<DeviceEnroll>> GetEnrollsSqlAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			List<DeviceEnroll> lst = new List<DeviceEnroll>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_tblEnroll_selectAll", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							lst.Add(LoadData(dr));
						}
						((DbConnection)(object)cn).Close();
						return lst;
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
	}

	private DeviceEnroll LoadData(SqlDataReader dr)
	{
		DeviceEnroll deviceEnroll = new DeviceEnroll();
		try
		{
			if (((DbDataReader)(object)dr)["EMachineNumber"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.EMachineNumber = (int)((DbDataReader)(object)dr)["EMachineNumber"];
			}
			if (((DbDataReader)(object)dr)["EnrollNumber"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.EnrollNumber = (int)((DbDataReader)(object)dr)["EnrollNumber"];
			}
			if (((DbDataReader)(object)dr)["FingerNumber"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.FingerNumber = (int)((DbDataReader)(object)dr)["FingerNumber"];
			}
			if (((DbDataReader)(object)dr)["Privilige"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.Privilige = (int)((DbDataReader)(object)dr)["Privilige"];
			}
			if (((DbDataReader)(object)dr)["Password1"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.Password1 = (int)((DbDataReader)(object)dr)["Password1"];
			}
			if (((DbDataReader)(object)dr)["FPData"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.FPData = (byte[])((DbDataReader)(object)dr)["FPData"];
			}
			if (((DbDataReader)(object)dr)["CompanyName"].ToString() != DBNull.Value.ToString())
			{
				deviceEnroll.CompanyName = ((DbDataReader)(object)dr)["CompanyName"].ToString();
			}
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			throw;
		}
		return deviceEnroll;
	}
}
