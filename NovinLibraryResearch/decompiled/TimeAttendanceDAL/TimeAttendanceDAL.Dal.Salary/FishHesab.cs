using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class FishHesab : IHasContext
{
	public string ContextGuid { get; set; }

	public int Id { get; private set; }

	public string Name { get; set; } = "";


	public int HtreeId { get; set; }

	public bool St { get; set; } = true;


	public decimal Account { get; set; }

	public string Description { get; set; } = "";


	public short Type { get; set; }

	public long CodeMoein { get; set; }

	public long CodeTafsil { get; set; }

	public bool Taxable { get; set; }

	public bool Insurance { get; set; }

	public bool PrsnTafsilUsed { get; set; }

	public static int LastValBaseHesabOnHkgogh => 4;

	public FishHesab(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<FishHesab> GetAsync(string contextGuid, string name, CancellationToken token)
	{
		_ = 2;
		try
		{
			FishHesab ret = null;
			ret.ContextGuid = contextGuid;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ret.ContextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_FishHesab_SelectRowByName", cn);
				try
				{
					cmd.Parameters.AddWithValue("Name", (object)name);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(dr, getAllfield: true, contextGuid);
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<FishHesab> GetAsync(string contextGuid, int id, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			FishHesab ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_FishHesab_SelectRow", cn);
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
							ret = LoadData(dr, getAllfield: true, contextGuid);
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
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\FishHesab.cs", 93);
			throw;
		}
	}

	private static FishHesab LoadData(SqlDataReader dr, bool getAllfield, string contextGuid)
	{
		try
		{
			FishHesab fishHesab = new FishHesab(contextGuid);
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["HTreeID"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.HtreeId = (int)((DbDataReader)(object)dr)["HTreeID"];
			}
			if (((DbDataReader)(object)dr)["Account"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.Account = (decimal)((DbDataReader)(object)dr)["Account"];
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.St = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["Type"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.Type = (short)((DbDataReader)(object)dr)["Type"];
			}
			if (((DbDataReader)(object)dr)["CodeMoein"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.CodeMoein = long.Parse(((DbDataReader)(object)dr)["CodeMoein"].ToString());
			}
			if (((DbDataReader)(object)dr)["CodeTafsil"].ToString() != DBNull.Value.ToString())
			{
				fishHesab.CodeTafsil = long.Parse(((DbDataReader)(object)dr)["CodeTafsil"].ToString());
			}
			if (getAllfield)
			{
				if (((DbDataReader)(object)dr)["Taxable"].ToString() != DBNull.Value.ToString())
				{
					fishHesab.Taxable = (bool)((DbDataReader)(object)dr)["Taxable"];
				}
				if (((DbDataReader)(object)dr)["Insurance"].ToString() != DBNull.Value.ToString())
				{
					fishHesab.Insurance = (bool)((DbDataReader)(object)dr)["Insurance"];
				}
				if (((DbDataReader)(object)dr)["PrsnTafsilUsed"].ToString() != DBNull.Value.ToString())
				{
					fishHesab.PrsnTafsilUsed = (bool)((DbDataReader)(object)dr)["PrsnTafsilUsed"];
				}
			}
			return fishHesab;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\FishHesab.cs", 122);
			throw;
		}
	}

	public async Task<bool> SaveOldAsync(CancellationToken token)
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
					cmd.Parameters.AddWithValue("Name", (object)(Text.FixString(Name) ?? ""));
					cmd.Parameters.AddWithValue("HTreeID", (object)HtreeId);
					cmd.Parameters.AddWithValue("Description", (object)(Text.FixString(Description) ?? ""));
					cmd.Parameters.AddWithValue("St", (object)St);
					cmd.Parameters.AddWithValue("Account", (object)Account);
					cmd.Parameters.AddWithValue("Type", (object)Type);
					cmd.Parameters.AddWithValue("CodeMoein", (object)CodeMoein);
					cmd.Parameters.AddWithValue("CodeTafsil", (object)CodeTafsil);
					cmd.Parameters.AddWithValue("Taxable", (object)Taxable);
					cmd.Parameters.AddWithValue("Insurance", (object)Insurance);
					cmd.Parameters.AddWithValue("PrsnTafsilUsed", (object)PrsnTafsilUsed);
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_FishHesab_Insert]";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Convert.ToInt32(obj.ToString());
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_FishHesab_Update]";
						cmd.Parameters.AddWithValue("Id", (object)Id);
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

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_FishHesab_Save", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)Id);
					cmd.Parameters.AddWithValue("Name", (object)(Text.FixString(Name) ?? ""));
					cmd.Parameters.AddWithValue("HTreeID", (object)HtreeId);
					cmd.Parameters.AddWithValue("Description", (object)(Text.FixString(Description) ?? ""));
					cmd.Parameters.AddWithValue("St", (object)St);
					cmd.Parameters.AddWithValue("Account", (object)Account);
					cmd.Parameters.AddWithValue("Type", (object)Type);
					cmd.Parameters.AddWithValue("CodeMoein", (object)CodeMoein);
					cmd.Parameters.AddWithValue("CodeTafsil", (object)CodeTafsil);
					cmd.Parameters.AddWithValue("Taxable", (object)Taxable);
					cmd.Parameters.AddWithValue("Insurance", (object)Insurance);
					cmd.Parameters.AddWithValue("PrsnTafsilUsed", (object)PrsnTafsilUsed);
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					if (obj != null && obj.ToString() != "")
					{
						Id = Convert.ToInt32(obj.ToString());
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
				SqlCommand cmd = new SqlCommand("[dbo].[sp_FishHesab_DeleteRow]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Id", (object)Id);
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

	public static async Task<List<FishHesab>> SelectAllAsync(string contextGuid, bool getAllField, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<FishHesab> ret = new List<FishHesab>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_FishHesab_SelectAll]", cn);
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
							ret.Add(LoadData(dr, getAllField, contextGuid));
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<FishHesab>> SelectRowsByTypeAsync(string contextGuid, short type, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<FishHesab> ret = new List<FishHesab>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_FishHesab_SelectRowsByType]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("Type", (object)type);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(dr, getAllfield: true, contextGuid));
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
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<long> MaxCodeMoeinAsync(string contextGuid, int ptype, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max(CodeMoein) from [FishHesab] where [St]='true' and [Type]=" + ptype + "),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return (long.Parse(obj.ToString()) == 0L) ? 1 : long.Parse(obj.ToString());
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

	public static async Task<long> NextCodeTafsilAsync(string contextGuid, int ptype, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select (isnull((select max(CodeTafsil) from [FishHesab] where [St]='true' and [Type]=" + ptype + "),0))", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
					((DbConnection)(object)cn).Close();
					return (obj == null) ? 1 : (long.Parse(obj.ToString()) + 1);
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

	public static async Task<int> GetCountByCodeMAsync(string contextGuid, int id, long codeM, long codeT, short type, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("", cn);
				try
				{
					cmd.Parameters.AddWithValue("@Id", (object)id);
					cmd.Parameters.AddWithValue("@CodeMoein", (object)codeM);
					cmd.Parameters.AddWithValue("@CodeTafsil", (object)codeT);
					cmd.Parameters.AddWithValue("@Type", (object)type);
					((DbCommand)(object)cmd).CommandText = "Select Count(*) from FishHesab Where CodeMoein=@CodeMoein and CodeTafsil=@CodeTafsil and Id<>@Id and St='True' and Type=@Type";
					((DbCommand)(object)cmd).CommandText = "Select isnull((" + ((DbCommand)(object)cmd).CommandText + "),0)";
					await ((DbConnection)(object)cn).OpenAsync(token);
					string? s = (await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString();
					((DbConnection)(object)cn).Close();
					return int.Parse(s);
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

	public static async Task<string> JoinAndGetNamesAsync(string contextGuid, List<int> ids, CancellationToken token)
	{
		try
		{
			string xstr = "";
			List<FishHesab> source = await SelectAllAsync(contextGuid, getAllField: false, token);
			foreach (int id in ids)
			{
				xstr = xstr + source.FirstOrDefault((FishHesab q) => q.Id == id)?.Name + ";";
			}
			return xstr;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
