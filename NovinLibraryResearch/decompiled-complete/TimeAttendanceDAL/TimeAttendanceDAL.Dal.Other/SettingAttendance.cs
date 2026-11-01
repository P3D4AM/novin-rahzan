using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;

namespace TimeAttendanceDAL.Dal.Other;

public class SettingAttendance : IHasContext
{
	[JsonIgnore]
	public string ContextGuid { get; set; }

	private string Val { get; set; } = "";


	public static async Task<string> GetAsync(string contextGuid, string name, short userId, CancellationToken token)
	{
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("settingKey is null or empty!!!");
		}
		try
		{
			string ret = "";
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand("sp_SettingAttendance_SelectRow", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Name", (object)name);
					cmd.Parameters.AddWithValue("UserID", (object)userId);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token) && ((DbDataReader)(object)dr)["Val"].ToString() != DBNull.Value.ToString())
						{
							ret = ((DbDataReader)(object)dr)["Val"].ToString();
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<bool> SetValueAsync(string contextGuid, string name, string value, short userId, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			value = Text.FixString(value);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_SettingAttendance_SaveValue", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					((DbParameter)(object)cmd.Parameters.Add("@Name", SqlDbType.NVarChar)).Value = name.Trim();
					((DbParameter)(object)cmd.Parameters.Add("@UserID", SqlDbType.Int)).Value = userId;
					((DbParameter)(object)cmd.Parameters.Add("@Val", SqlDbType.NVarChar)).Value = value.Trim();
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
		catch (Exception ex)
		{
			SqlConnection.ClearAllPools();
			Log.Error(ex, "");
			throw;
		}
	}

	public static Task<string> GetValueAsync(string contextGuid, string propertyName, short userId, CancellationToken token = default(CancellationToken))
	{
		return GetAsync(contextGuid, propertyName, userId, token);
	}

	public static async Task<string> GetSoftVersionFromDBAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token);
				SqlCommand cmd = new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SettingAttendance]') AND type in (N'U')) select 0 else select 1", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.Text;
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					if ((int)obj == 0)
					{
						return "";
					}
					return await GetAsync(contextGuid, "Version", 0, token);
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

	public static async Task<bool> GetBoolValueAsync(string contextGuid, string propertyName, bool def, short userId, CancellationToken token)
	{
		try
		{
			string text = await GetValueAsync(contextGuid, propertyName, userId, token);
			if (string.IsNullOrEmpty(text))
			{
				return def;
			}
			if (text.Trim() == "")
			{
				return def;
			}
			bool.TryParse(text, out var result);
			return result;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
