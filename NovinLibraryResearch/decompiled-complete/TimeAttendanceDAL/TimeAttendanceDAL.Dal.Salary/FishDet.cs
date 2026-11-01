using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Salary;

[Serializable]
[DoNotPruneType]
public class FishDet
{
	public int Id { get; private set; }

	public int HesabId { get; set; }

	public int FishId { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public decimal TmpValue { get; set; }

	public string Description { get; set; } = "";


	public short Type { get; set; }

	public string Name { get; set; } = "";


	public bool IsDecrease { get; set; }

	public FishDet()
	{
	}

	public FishDet(SqlDataReader dr)
	{
		LoadData(dr);
	}

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["HesabID"].ToString() != DBNull.Value.ToString())
			{
				HesabId = (int)((DbDataReader)(object)dr)["HesabID"];
			}
			if (((DbDataReader)(object)dr)["FishID"].ToString() != DBNull.Value.ToString())
			{
				FishId = (int)((DbDataReader)(object)dr)["FishID"];
			}
			if (((DbDataReader)(object)dr)["Debit"].ToString() != DBNull.Value.ToString())
			{
				Debit = (decimal)((DbDataReader)(object)dr)["Debit"];
			}
			if (((DbDataReader)(object)dr)["Credit"].ToString() != DBNull.Value.ToString())
			{
				Credit = (decimal)((DbDataReader)(object)dr)["Credit"];
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["Type"].ToString() != DBNull.Value.ToString())
			{
				Type = (short)((DbDataReader)(object)dr)["Type"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["ISDecrease"].ToString() != DBNull.Value.ToString())
			{
				IsDecrease = (bool)((DbDataReader)(object)dr)["ISDecrease"];
			}
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Salary\\FishDet.cs", 96);
			return false;
		}
	}

	public async Task<bool> Save(SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandType = CommandType.StoredProcedure;
			obj.Parameters.AddWithValue("HesabID", (object)HesabId);
			obj.Parameters.AddWithValue("FishID", (object)FishId);
			obj.Parameters.AddWithValue("Debit", (object)Debit);
			obj.Parameters.AddWithValue("Credit", (object)Credit);
			obj.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
			obj.Parameters.AddWithValue("Type", (object)Type);
			obj.Parameters.AddWithValue("ISDecrease", (object)IsDecrease);
			((DbCommand)(object)obj).CommandText = "sp_FishMali_Insert";
			object obj2 = await ((DbCommand)(object)obj).ExecuteScalarAsync(token);
			if (obj2 != null)
			{
				Id = Convert.ToInt32(obj2.ToString());
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task DeleteFishIDAsync(SqlTransaction tr, int fishId, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandText = "delete from [FishMali] where FishID=" + fishId;
			await ((DbCommand)(object)obj).ExecuteScalarAsync(token);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<FishDet>> GetAllAsync(string contextGuid, int fishId, CancellationToken token)
	{
		_ = 1;
		try
		{
			List<FishDet> ret = new List<FishDet>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("[sp_FishMali_SelectRowsByFishID]", cn);
				try
				{
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@FishID", (object)fishId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = cmd.ExecuteReader();
					while (await ((DbDataReader)(object)dr).ReadAsync(token))
					{
						ret.Add(new FishDet(dr));
					}
					((DbConnection)(object)cn).Close();
					return ret;
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
