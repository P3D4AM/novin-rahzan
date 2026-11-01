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
using TimeAttendanceDAL.Dal.Salary;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class GharardadDet : IHasContext
{
	public string ContextGuid { get; set; }

	public int GharardadId { get; set; }

	public int HesabId { get; set; }

	public string Name { get; private set; } = "";


	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public short Order { get; set; }

	public string Desc { get; set; } = "";


	public short Type { get; private set; }

	public int HTreeId { get; private set; }

	public string DescFishHesab { get; private set; } = "";


	public decimal Account { get; set; }

	public long CodeTafsil { get; private set; }

	public long CodeMoein { get; private set; }

	public bool IsDecrease { get; set; }

	public GharardadDet(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<GharardadDet> GetAsync(string contextGuid, int pGharardadID, int pHesabID, CancellationToken token)
	{
		_ = 2;
		try
		{
			GharardadDet ret = new GharardadDet(contextGuid);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_GharardadDet_SelectRow", cn);
				try
				{
					new SqlDataAdapter(cmd);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("GharardadID", (object)pGharardadID);
					cmd.Parameters.AddWithValue("HesabID", (object)pHesabID);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.LoadData(dr);
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

	public static async Task<GharardadDet> GetAsync(string contextGuid, int pHesabID, CancellationToken token)
	{
		try
		{
			GharardadDet ret = new GharardadDet(contextGuid);
			await ret.LoadHesabIDFieldsAsync(pHesabID, isFromconstructor: true, token);
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

	private async Task LoadHesabIDFieldsAsync(int pHesabID, bool isFromconstructor = false, CancellationToken token = default(CancellationToken))
	{
		try
		{
			FishHesab fishHesab = await FishHesab.GetAsync(ContextGuid, pHesabID, token);
			if (isFromconstructor)
			{
				HesabId = fishHesab.Id;
				Desc = fishHesab.Description;
				if (fishHesab.Type == 1)
				{
					Credit = fishHesab.Account;
				}
				else
				{
					Debit = fishHesab.Account;
				}
			}
			Name = fishHesab.Name;
			Type = fishHesab.Type;
			HTreeId = fishHesab.HtreeId;
			DescFishHesab = fishHesab.Description;
			Account = fishHesab.Account;
			CodeTafsil = fishHesab.CodeTafsil;
			CodeMoein = fishHesab.CodeMoein;
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

	public GharardadDet(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["GharardadID"].ToString() != DBNull.Value.ToString())
			{
				GharardadId = (int)((DbDataReader)(object)dr)["GharardadID"];
			}
			if (((DbDataReader)(object)dr)["HesabID"].ToString() != DBNull.Value.ToString())
			{
				HesabId = (int)((DbDataReader)(object)dr)["HesabID"];
			}
			if (((DbDataReader)(object)dr)["Debit"].ToString() != DBNull.Value.ToString())
			{
				Debit = (decimal)((DbDataReader)(object)dr)["Debit"];
			}
			if (((DbDataReader)(object)dr)["Credit"].ToString() != DBNull.Value.ToString())
			{
				Credit = (decimal)((DbDataReader)(object)dr)["Credit"];
			}
			if (((DbDataReader)(object)dr)["Order"].ToString() != DBNull.Value.ToString())
			{
				Order = (short)((DbDataReader)(object)dr)["Order"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Desc = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["ISDecrease"].ToString() != DBNull.Value.ToString())
			{
				IsDecrease = (bool)((DbDataReader)(object)dr)["ISDecrease"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["Type"].ToString() != DBNull.Value.ToString())
			{
				Type = (short)((DbDataReader)(object)dr)["Type"];
			}
			if (((DbDataReader)(object)dr)["HTreeID"].ToString() != DBNull.Value.ToString())
			{
				HTreeId = (int)((DbDataReader)(object)dr)["HTreeID"];
			}
			if (((DbDataReader)(object)dr)["DescFishHesab"].ToString() != DBNull.Value.ToString())
			{
				DescFishHesab = ((DbDataReader)(object)dr)["DescFishHesab"].ToString();
			}
			if (((DbDataReader)(object)dr)["Account"].ToString() != DBNull.Value.ToString())
			{
				Account = (decimal)((DbDataReader)(object)dr)["Account"];
			}
			if (((DbDataReader)(object)dr)["CodeTafsil"].ToString() != DBNull.Value.ToString())
			{
				CodeTafsil = long.Parse(((DbDataReader)(object)dr)["CodeTafsil"].ToString());
			}
			if (((DbDataReader)(object)dr)["CodeMoein"].ToString() != DBNull.Value.ToString())
			{
				CodeMoein = long.Parse(((DbDataReader)(object)dr)["CodeMoein"].ToString());
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

	public async Task<bool> SaveAsync(SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandText = "sp_GharardadDet_Insert";
			((DbCommand)(object)obj).CommandType = CommandType.StoredProcedure;
			obj.Parameters.AddWithValue("@GharardadID", (object)GharardadId);
			obj.Parameters.AddWithValue("@HesabID", (object)HesabId);
			obj.Parameters.AddWithValue("@Debit", (object)Debit);
			obj.Parameters.AddWithValue("@Credit", (object)Credit);
			obj.Parameters.AddWithValue("@Order", (object)Order);
			obj.Parameters.AddWithValue("@Description", (object)Desc);
			obj.Parameters.AddWithValue("@ISDecrease", (object)IsDecrease);
			await ((DbCommand)(object)obj).ExecuteNonQueryAsync(token);
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

	public static async Task<bool> DeleteByGharardadIDAsync(SqlTransaction tr, int pGharardadID, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandText = "sp_GharardadDet_DeleteRowsByGharardadID";
			((DbCommand)(object)obj).CommandType = CommandType.StoredProcedure;
			obj.Parameters.AddWithValue("GharardadID", (object)pGharardadID);
			await ((DbCommand)(object)obj).ExecuteNonQueryAsync(token);
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

	public static async Task<List<GharardadDet>> SelectAllByGharardadAsync(string contextGuid, int pGharardadID, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<GharardadDet> ret = new List<GharardadDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_GharardadDet_SelectRowsByGharardadID", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("GharardadID", (object)pGharardadID);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new GharardadDet(contextGuid, dr));
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
