using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class TaxTable : IHasContext
{
	public string ContextGuid { get; set; }

	public int Id { get; set; }

	public DateTime StartDateM { get; set; }

	public DateTime EndDateM { get; set; }

	[JsonIgnore]
	public string StartDateCalture => Calendar.MiladiToCalture(StartDateM);

	[JsonIgnore]
	public string EndDateCalture => Calendar.MiladiToCalture(EndDateM);

	public short Year { get; set; }

	public decimal FromPrice { get; set; }

	public decimal ToPrice { get; set; }

	public decimal TaxPercentage { get; set; }

	public decimal InitialValue { get; set; }

	public string Description { get; set; } = "";


	public TaxTable(string contextGuid)
	{
		ContextGuid = contextGuid;
		try
		{
			StartDateM = DateTime.Now;
			EndDateM = Utilities.GotoEndYear(DateTime.Now);
			Year = short.Parse(Calendar.MiladiToCalture(DateTime.Now)?.Substring(0, 4) ?? throw new InvalidOperationException("TaxTable_ctor: year is not valid."));
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\TaxTable.cs", 57);
		}
	}

	public static async Task<TaxTable> GetAsync(string contextGuid, int id, CancellationToken token)
	{
		TaxTable taxtable = new TaxTable
		{
			ContextGuid = contextGuid
		};
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(taxtable.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_TaxTable_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)id);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							taxtable.LoadData(dr);
						}
						((DbConnection)(object)cn).Close();
						return taxtable;
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

	public static async Task<TaxTable> GetAsync(string contextGuid, short year, decimal salary, CancellationToken token)
	{
		TaxTable taxTable = new TaxTable
		{
			ContextGuid = contextGuid
		};
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(taxTable.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_TaxTable_SelectOnSalary", cn);
				try
				{
					cmd.Parameters.AddWithValue("year", (object)year);
					salary = salary.RoundPrice();
					cmd.Parameters.AddWithValue("salary", (object)salary);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							taxTable.LoadData(dr);
						}
						((DbConnection)(object)cn).Close();
						return taxTable;
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

	public TaxTable(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public TaxTable()
	{
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["StartDateM"].ToString() != DBNull.Value.ToString())
			{
				StartDateM = (DateTime)((DbDataReader)(object)dr)["StartDateM"];
			}
			if (((DbDataReader)(object)dr)["EndDateM"].ToString() != DBNull.Value.ToString())
			{
				EndDateM = (DateTime)((DbDataReader)(object)dr)["EndDateM"];
			}
			if (((DbDataReader)(object)dr)["Year"].ToString() != DBNull.Value.ToString())
			{
				Year = (short)((DbDataReader)(object)dr)["Year"];
			}
			if (((DbDataReader)(object)dr)["FromPrice"].ToString() != DBNull.Value.ToString())
			{
				FromPrice = (decimal)((DbDataReader)(object)dr)["FromPrice"];
			}
			if (((DbDataReader)(object)dr)["ToPrice"].ToString() != DBNull.Value.ToString())
			{
				ToPrice = (decimal)((DbDataReader)(object)dr)["ToPrice"];
			}
			if (((DbDataReader)(object)dr)["TaxPercentage"].ToString() != DBNull.Value.ToString())
			{
				TaxPercentage = (decimal)((DbDataReader)(object)dr)["TaxPercentage"];
			}
			if (((DbDataReader)(object)dr)["InitialValue"].ToString() != DBNull.Value.ToString())
			{
				InitialValue = (decimal)((DbDataReader)(object)dr)["InitialValue"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\TaxTable.cs", 143);
		}
	}

	public async Task<bool> SaveAsync(SqlConnection pcn = null, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = (SqlConnection)(((object)pcn) ?? ((object)new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid))));
			SqlCommand val = new SqlCommand("", cn);
			((DbCommand)val).CommandType = CommandType.StoredProcedure;
			SqlCommand cmd = val;
			try
			{
				cmd.Parameters.AddWithValue("StartDateM", (object)StartDateM);
				cmd.Parameters.AddWithValue("EndDateM", (object)EndDateM);
				cmd.Parameters.AddWithValue("Year", (object)Year);
				cmd.Parameters.AddWithValue("FromPrice", (object)FromPrice);
				cmd.Parameters.AddWithValue("ToPrice", (object)ToPrice);
				cmd.Parameters.AddWithValue("TaxPercentage", (object)TaxPercentage);
				cmd.Parameters.AddWithValue("InitialValue", (object)InitialValue);
				cmd.Parameters.AddWithValue("Description", (object)Description);
				if (pcn == null)
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
				}
				if (Id <= 0)
				{
					((DbCommand)(object)cmd).CommandText = "dbo.sp_TaxTable_Insert";
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						Id = Convert.ToInt16(obj.ToString());
					}
					_ = " [ افزودن جدول مالیاتی] : " + Description + " [سال عملکرد] : " + Year + " [تاریخ اجراء از] : " + Calendar.MiladiToCalture(StartDateM) + " [ToDate] : " + Calendar.MiladiToCalture(EndDateM) + " [بازه مبلغ از] : " + FromPrice.ToString("N0") + " [ToPrice] : " + ToPrice.ToString("N0") + " [و در صد] " + TaxPercentage + " [و مبلغ پیش فرض] " + InitialValue.ToString("N0");
				}
				else
				{
					((DbCommand)(object)cmd).CommandText = "dbo.sp_TaxTable_Update";
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					_ = "  [ویرایش جدول مالیاتی] : " + Description + " [سال عملکرد] : " + Year + " [تاریخ اجراء از] : " + Calendar.MiladiToCalture(StartDateM) + " [ToDate] : " + Calendar.MiladiToCalture(EndDateM) + " [بازه مبلغ از] : " + FromPrice.ToString("N0") + " [ToPrice] : " + ToPrice.ToString("N0") + " [و در صد] " + TaxPercentage + " [و مبلغ پیش فرض] " + InitialValue.ToString("N0");
				}
				if (pcn == null)
				{
					((DbConnection)(object)cn).Close();
				}
				return true;
			}
			finally
			{
				((IDisposable)cmd)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> DeleteAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_TaxTable_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)Id);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					_ = "  [حذف جدول مالیاتی] : " + Description + " [سال عملکرد] : " + Year + " [تاریخ اجراء از]: " + Calendar.MiladiToCalture(StartDateM) + " [ToDate] : " + Calendar.MiladiToCalture(EndDateM) + " [بازه مبلغ از] : " + FromPrice.ToString("N0") + " [ToPrice]: " + ToPrice.ToString("N0") + " [و در صد] " + TaxPercentage + " [و مبلغ پیش فرض] " + InitialValue.ToString("N0");
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
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<TaxTable>> GetAllAsync(string contextGuid, short year, CancellationToken token)
	{
		List<TaxTable> ret = new List<TaxTable>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_TaxTable_SelectAll]", cn);
				try
				{
					cmd.Parameters.AddWithValue("year", (object)year);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new TaxTable(contextGuid, dr));
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

	public static async Task<short> RecordCountAsync(string contextGuid, short year, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_TaxTable_IsExistTable]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("year", (object)year);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null)
					{
						return Convert.ToInt16(obj.ToString());
					}
					((DbConnection)(object)cn).Close();
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
		return 0;
	}
}
