using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Device;

[Serializable]
[DoNotPruneType]
public class HardwareCodeCUSView : IHasContext
{
	public static List<HardwareCodeCUSView> HardwareCodesViews;

	public int PersonId { get; set; }

	public short DeviceId { get; set; }

	public string HardwareCode { get; set; }

	public string FullName { get; set; }

	public string DeviceName { get; set; }

	public string ContextGuid { get; private set; }

	public Guid DeviceGuid { get; set; }

	public Guid PersonGuid { get; set; }

	public HardwareCodeCUSView(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static HardwareCodeCUSView LoadData(string contextGuid, IDataRecord dr)
	{
		try
		{
			HardwareCodeCUSView hardwareCodeCUSView = new HardwareCodeCUSView(contextGuid);
			if (dr["PersonID"] != DBNull.Value)
			{
				hardwareCodeCUSView.PersonId = (int)dr["PersonID"];
			}
			if (dr["DeviceID"] != DBNull.Value)
			{
				hardwareCodeCUSView.DeviceId = (short)dr["DeviceID"];
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				hardwareCodeCUSView.HardwareCode = dr["HardwareCode"].ToString();
			}
			if (dr["FullName"] != DBNull.Value)
			{
				hardwareCodeCUSView.FullName = dr["FullName"].ToString();
			}
			if (dr["DeviceName"] != DBNull.Value)
			{
				hardwareCodeCUSView.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["DeviceGuid"] != DBNull.Value)
			{
				hardwareCodeCUSView.DeviceGuid = (Guid)dr["DeviceGuid"];
			}
			if (dr["PersonGuid"] != DBNull.Value)
			{
				hardwareCodeCUSView.PersonGuid = (Guid)dr["PersonGuid"];
			}
			return hardwareCodeCUSView;
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

	public static async Task<List<HardwareCodeCUSView>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<HardwareCodeCUSView> ret = new List<HardwareCodeCUSView>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardwareCodeCusView_SelectAll]", cn);
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
							ret.Add(LoadData(contextGuid, (IDataRecord)dr));
							if (token.IsCancellationRequested)
							{
								return null;
							}
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
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

	public static async Task<List<HardwareCodeCUSView>> GetAsync(string contextGuid, int personId, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (token.IsCancellationRequested)
			{
				return new List<HardwareCodeCUSView>();
			}
			List<HardwareCodeCUSView> ret = new List<HardwareCodeCUSView>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardwareCodeCusView_Select]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("PersonId", (object)personId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(contextGuid, (IDataRecord)dr));
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
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

	public static List<HardwareCodeCUSView> Get(string contextGuid, int personId)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_0025: Expected O, but got Unknown
		try
		{
			List<HardwareCodeCUSView> list = new List<HardwareCodeCUSView>();
			SqlConnection val = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val2 = new SqlCommand("[sp_HardwareCodeCusView_Select]", val);
				((DbCommand)val2).CommandType = CommandType.StoredProcedure;
				SqlCommand val3 = val2;
				try
				{
					val3.Parameters.AddWithValue("PersonId", (object)personId);
					((DbConnection)(object)val).Open();
					SqlDataReader val4 = val3.ExecuteReader();
					try
					{
						while (((DbDataReader)(object)val4).Read())
						{
							list.Add(LoadData(contextGuid, (IDataRecord)val4));
						}
						((DbDataReader)(object)val4).Close();
						((DbConnection)(object)val).Close();
						return list;
					}
					finally
					{
						((IDisposable)val4)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val3)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
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
}
