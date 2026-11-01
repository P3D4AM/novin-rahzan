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
public class Per_Khedmat : IHasContext
{
	public string ContextGuid { get; set; }

	public short ID { get; private set; }

	public string Name { get; set; } = "";


	public string Desc { get; set; } = "";


	public bool ST { get; set; } = true;


	public bool Check { get; set; }

	public Per_Khedmat(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<Per_Khedmat> GetAsync(string contextGuid, short ID, CancellationToken token)
	{
		try
		{
			Per_Khedmat ret = new Per_Khedmat(contextGuid)
			{
				ContextGuid = contextGuid
			};
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

	public Per_Khedmat(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private async Task<bool> LoadDataAsync(short pID, CancellationToken token)
	{
		bool result = false;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Per_Khedmat_SelectRow", cn);
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
			return result;
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
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
					cmd.Parameters.AddWithValue("St", (object)ST.ToString());
					cmd.Parameters.AddWithValue("Desc", (object)Text.FixString(Desc));
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (ID <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Per_Khedmat_Insert";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							ID = Convert.ToInt16(obj.ToString());
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "dbo.sp_Per_Khedmat_Update";
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
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_Per_Khedmat_DeleteRow]", cn);
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

	public static async Task<List<Per_Khedmat>> SelectAllAsync(string contextGuid, string extItem = "", CancellationToken token = default(CancellationToken))
	{
		List<Per_Khedmat> ret = new List<Per_Khedmat>();
		if (extItem != "")
		{
			Per_Khedmat per_Khedmat = new Per_Khedmat(contextGuid);
			per_Khedmat.Name = extItem;
			ret.Add(per_Khedmat);
		}
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_Per_Khedmat_SelectAll]", cn);
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
							ret.Add(new Per_Khedmat(contextGuid, dr));
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
