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

namespace TimeAttendanceDAL.Dal.FLRPerson;

[Serializable]
[DoNotPruneType]
public class Per_Eisar : IHasContext
{
	public string ContextGuid { get; set; }

	public short ID { get; private set; }

	public string Name { get; set; } = "";


	public string Desc { get; set; } = "";


	public bool ST { get; set; } = true;


	public bool Check { get; set; }

	public Per_Eisar(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Per_Eisar> GetAsync(string contextGuid, short ID, CancellationToken token)
	{
		try
		{
			Per_Eisar ret = new Per_Eisar(contextGuid);
			await ret.LoadDataAsync(ID, token);
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

	public Per_Eisar(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private async Task<bool> LoadDataAsync(short pID, CancellationToken token)
	{
		_ = 2;
		try
		{
			bool result = false;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Per_Eisar_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pID);
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

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				ID = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				ST = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Desc"].ToString() != DBNull.Value.ToString())
			{
				Desc = ((DbDataReader)(object)dr)["Desc"].ToString();
			}
			return true;
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

	public async Task<bool> SaveAsync(CancellationToken token)
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
			cmd.Parameters.AddWithValue("St", (object)ST.ToString());
			cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desc));
			await ((DbConnection)(object)cn).OpenAsync(token);
			if (ID <= 0)
			{
				((DbCommand)(object)cmd).CommandText = "dbo.sp_Per_Eisar_Insert";
				object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
				if (obj != null)
				{
					ID = Convert.ToInt16(obj.ToString());
				}
			}
			else
			{
				((DbCommand)(object)cmd).CommandText = "dbo.sp_Per_Eisar_Update";
				cmd.Parameters.AddWithValue("Id", (object)ID);
				await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			}
			((DbConnection)(object)cn).Close();
			return true;
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

	public async Task<bool> DeleteAsync(CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand cmd = new SqlCommand("[dbo].[sp_Per_Eisar_DeleteRow]", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("Id", (object)ID);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
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

	public static async Task<List<Per_Eisar>> SelectAllAsync(string contextGuid, string extItem = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				List<Per_Eisar> ret = new List<Per_Eisar>();
				if (extItem != "")
				{
					Per_Eisar item = new Per_Eisar(contextGuid)
					{
						Name = extItem
					};
					ret.Add(item);
				}
				SqlCommand val = new SqlCommand("[sp_Per_Eisar_SelectAll]", cn);
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
							ret.Add(new Per_Eisar(contextGuid, dr));
						}
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
}
