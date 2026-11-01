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
public class Bank : IHasContext
{
	public string ContextGuid { get; set; }

	public short ID { get; private set; }

	public string Name { get; set; } = "";


	public string Desc { get; set; } = "";


	public string Code { get; set; } = "";


	public string FullName { get; set; } = "";


	public bool ST { get; set; } = true;


	public string Shobe { get; set; } = "";


	public Bank(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Bank> GetAsync(string contextGuid, short id, CancellationToken token)
	{
		_ = 2;
		try
		{
			Bank result = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Bank_SelectRow", cn);
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
							result = LoadData(contextGuid, dr);
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

	private static Bank LoadData(string contextGuid, SqlDataReader dr)
	{
		try
		{
			Bank bank = new Bank(contextGuid);
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				bank.ID = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				bank.Code = ((DbDataReader)(object)dr)["Code"].ToString();
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				bank.Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["fullName"].ToString() != DBNull.Value.ToString())
			{
				bank.FullName = ((DbDataReader)(object)dr)["fullName"].ToString();
			}
			if (((DbDataReader)(object)dr)["Desc"].ToString() != DBNull.Value.ToString())
			{
				bank.Desc = ((DbDataReader)(object)dr)["Desc"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				bank.ST = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Shobe"].ToString() != DBNull.Value.ToString())
			{
				bank.Shobe = ((DbDataReader)(object)dr)["Shobe"].ToString();
			}
			return bank;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Code", (object)Text.FixString(Code));
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
					cmd.Parameters.AddWithValue("Shobe", (object)Text.FixString(Shobe));
					cmd.Parameters.AddWithValue("fullName", (object)Text.FixString(FullName));
					cmd.Parameters.AddWithValue("St", (object)ST);
					cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desc));
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (ID <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Bank_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							ID = Convert.ToInt16(obj.ToString());
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Bank_Update";
						cmd.Parameters.AddWithValue("Id", (object)ID);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
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
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Bank_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)ID);
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
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<Bank>> SelectAllAsync(string contextGuid, string extItem = "", CancellationToken token = default(CancellationToken))
	{
		List<Bank> ret = new List<Bank>();
		if (extItem != "")
		{
			Bank item = new Bank(contextGuid)
			{
				Name = extItem
			};
			ret.Add(item);
		}
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Bank_SelectAll]", cn);
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
							ret.Add(LoadData(contextGuid, dr));
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
}
