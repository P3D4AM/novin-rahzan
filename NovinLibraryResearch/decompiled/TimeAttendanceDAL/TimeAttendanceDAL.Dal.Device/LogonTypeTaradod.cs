using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Device;

[Serializable]
[DoNotPruneType]
public class LogonTypeTaradod : IHasContext
{
	private string _conv;

	public string ContextGuid { get; set; }

	public DeviceModelEnumType enDeviceModel { get; set; }

	public string DeviceModelName => Utilities.GetDisplay<DeviceModelEnumType>(enDeviceModel);

	public short TraficId { get; set; }

	public TypeTaradodEnumType TypeTaradod { get; set; }

	public string TypeTaradodName => Utilities.GetDisplay<TypeTaradodEnumType>(TypeTaradod);

	public TypeTaradodEnumType ConvertedTypeTaradod { get; set; }

	public string ConvertedTypeTaradodName => Utilities.GetDisplay<TypeTaradodEnumType>(ConvertedTypeTaradod);

	public LogonTypeTaradod(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<LogonTypeTaradod> GetAsync(string contextGuid, short pTraficID, DeviceModelEnumType deviceModel, CancellationToken token = default(CancellationToken))
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		LogonTypeTaradod ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_LogonTypeTaradod_SelectRow", cn);
				try
				{
					new SqlDataAdapter(cmd);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("TraficID", (object)pTraficID);
					cmd.Parameters.AddWithValue("DeviceModel", (object)(short)deviceModel);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, contextGuid);
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
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

	public static async Task<LogonTypeTaradod> GetAsync(string contextGuid, string deviceSerial, int deviceNo, TypeTaradodEnumType typeTaradod, CancellationToken token = default(CancellationToken))
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		LogonTypeTaradod ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_LogonTypeTaradod_SelectRowByDeviceNoOrDeviceSerial", cn);
				try
				{
					new SqlDataAdapter(cmd);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("TypeTaradod", (object)typeTaradod);
					cmd.Parameters.AddWithValue("DeviceSerial", (object)deviceSerial);
					cmd.Parameters.AddWithValue("DeviceNo", (object)deviceNo);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, contextGuid);
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
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

	public static async Task<LogonTypeTaradod> GetAsync(string contextGuid, DeviceModelEnumType deviceModel, TypeTaradodEnumType typeTaradod, CancellationToken token = default(CancellationToken))
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		LogonTypeTaradod ret = null;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_LogonTypeTaradod_SelectRowOnType", cn);
				try
				{
					new SqlDataAdapter(cmd);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@DeviceModel", (object)(short)deviceModel);
					cmd.Parameters.AddWithValue("@TypeTaradod", (object)typeTaradod);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, contextGuid);
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
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

	private static LogonTypeTaradod LoadData(SqlDataReader dr, string contextGuid)
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		LogonTypeTaradod logonTypeTaradod = new LogonTypeTaradod(contextGuid);
		try
		{
			if (((DbDataReader)(object)dr)["TraficID"].ToString() != DBNull.Value.ToString())
			{
				logonTypeTaradod.TraficId = (short)((DbDataReader)(object)dr)["TraficID"];
			}
			if (((DbDataReader)(object)dr)["DeviceModel"].ToString() != DBNull.Value.ToString())
			{
				short num = (short)((DbDataReader)(object)dr)["DeviceModel"];
				logonTypeTaradod.enDeviceModel = (DeviceModelEnumType)num;
			}
			if (((DbDataReader)(object)dr)["typeTaradod"].ToString() != DBNull.Value.ToString())
			{
				logonTypeTaradod.TypeTaradod = (TypeTaradodEnumType)((DbDataReader)(object)dr)["typeTaradod"];
			}
			if (((DbDataReader)(object)dr)["ConvertedTypeTaradod"].ToString() != DBNull.Value.ToString())
			{
				logonTypeTaradod.ConvertedTypeTaradod = (TypeTaradodEnumType)((DbDataReader)(object)dr)["ConvertedTypeTaradod"];
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
		return logonTypeTaradod;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonTypeTaradod_Update", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@TraficID", (object)TraficId);
					cmd.Parameters.AddWithValue("@DeviceModel", (object)enDeviceModel);
					cmd.Parameters.AddWithValue("@ConvertedTypeTaradod", (object)(short)(int)ConvertedTypeTaradod);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					return true;
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

	public static async Task<List<LogonTypeTaradod>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		List<LogonTypeTaradod> ret = new List<LogonTypeTaradod>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_LogonTypeTaradod_SelectAll", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							token.ThrowIfCancellationRequested();
							ret.Add(LoadData(dr, contextGuid));
						}
						((DbConnection)(object)cn).Close();
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
			return ret;
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
}
