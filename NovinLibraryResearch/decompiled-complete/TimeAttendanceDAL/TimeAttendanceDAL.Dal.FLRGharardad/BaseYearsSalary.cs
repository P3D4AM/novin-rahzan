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

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class BaseYearsSalary : IHasContext
{
	public string ContextGuid { get; set; }

	public short nYear { get; set; }

	public short GroupId { get; set; }

	public decimal Price { get; set; }

	public string Name { get; set; }

	public BaseYearsSalary(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<BaseYearsSalary> GetAsync(string contextGuid, short pYear, short pGroup, CancellationToken token)
	{
		try
		{
			BaseYearsSalary ret = new BaseYearsSalary(contextGuid);
			await ret.LoadDataAsync(pYear, pGroup, token);
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public BaseYearsSalary(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private async Task<bool> LoadDataAsync(short pYear, short pGroup, CancellationToken token)
	{
		_ = 2;
		try
		{
			bool result = false;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_BaseYearsSalary_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("nYear", (object)pYear);
					cmd.Parameters.AddWithValue("GroupId", (object)pGroup);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							LoadData(dr);
							result = true;
						}
						((DbConnection)(object)cn).Close();
						return result;
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

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["nYear"].ToString() != DBNull.Value.ToString())
			{
				nYear = (short)((DbDataReader)(object)dr)["nYear"];
			}
			if (((DbDataReader)(object)dr)["GroupId"].ToString() != DBNull.Value.ToString())
			{
				GroupId = (short)((DbDataReader)(object)dr)["GroupId"];
			}
			if (((DbDataReader)(object)dr)["Price"].ToString() != DBNull.Value.ToString())
			{
				Price = (decimal)((DbDataReader)(object)dr)["Price"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("nYear", (object)nYear);
			cmd.Parameters.AddWithValue("GroupId", (object)GroupId);
			cmd.Parameters.AddWithValue("Price", (object)Price);
			await ((DbConnection)(object)cn).OpenAsync(token);
			((DbCommand)(object)cmd).CommandText = "dbo.sp_BaseYearsSalary_Save";
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("[dbo].[sp_BaseYearsSalary_DeleteRow]", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("nYear", (object)nYear);
			cmd.Parameters.AddWithValue("GroupId", (object)GroupId);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<BaseYearsSalary>> GetAllAsync(string contextGuid, CancellationToken token)
	{
		List<BaseYearsSalary> ret = new List<BaseYearsSalary>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_BaseYearsSalary_SelectAll]", cn);
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
							ret.Add(new BaseYearsSalary(contextGuid, dr));
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<decimal> GetPriceSanavatAsync(string contextGuid, DateTime startDateMGharardad, DateTime currentDate, int groupid, int psanavat, CancellationToken token)
	{
		_ = 1;
		try
		{
			decimal xsum = default(decimal);
			int num = (int)(((double)psanavat + (currentDate - startDateMGharardad).TotalDays / 30.0) / 12.0);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_BaseYearsSalary_GetSanavatAmount]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("nYear", (object)num);
					cmd.Parameters.AddWithValue("GroupId", (object)groupid);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null && obj.ToString() != "")
					{
						xsum = Convert.ToDecimal(obj.ToString());
					}
					((DbConnection)(object)cn).Close();
					return xsum;
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
