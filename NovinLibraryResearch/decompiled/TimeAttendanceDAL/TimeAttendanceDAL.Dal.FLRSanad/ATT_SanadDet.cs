using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Utility;

namespace TimeAttendanceDAL.Dal.FLRSanad;

[Serializable]
[DoNotPruneType]
public class ATT_SanadDet
{
	public int ID { get; private set; }

	public int SanadId { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public long CodeMoein { get; set; }

	public long CodeTafsil { get; set; }

	public string Description { get; set; } = "";


	public string TitleTafsil { get; set; } = "";


	public bool MergeWithOther { get; set; }

	public ATT_SanadDet(int sanadId, decimal debit, decimal credit, long codeMoein, long codeTafsil, string description, string titleTafsil, bool mergeWithOther)
	{
		SanadId = sanadId;
		Debit = debit.RoundPrice();
		Credit = credit.RoundPrice();
		CodeMoein = codeMoein;
		CodeTafsil = codeTafsil;
		Description = description;
		TitleTafsil = titleTafsil;
		MergeWithOther = mergeWithOther;
	}

	public ATT_SanadDet(SqlDataReader dr)
	{
		LoadData(dr);
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				ID = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["SanadID"].ToString() != DBNull.Value.ToString())
			{
				SanadId = (int)((DbDataReader)(object)dr)["SanadID"];
			}
			if (((DbDataReader)(object)dr)["Debit"].ToString() != DBNull.Value.ToString())
			{
				Debit = (decimal)((DbDataReader)(object)dr)["Debit"];
			}
			if (((DbDataReader)(object)dr)["Credit"].ToString() != DBNull.Value.ToString())
			{
				Credit = (decimal)((DbDataReader)(object)dr)["Credit"];
			}
			if (((DbDataReader)(object)dr)["CodeMoein"].ToString() != DBNull.Value.ToString())
			{
				CodeMoein = long.Parse(((DbDataReader)(object)dr)["CodeMoein"].ToString());
			}
			if (((DbDataReader)(object)dr)["CodeTafsil"].ToString() != DBNull.Value.ToString())
			{
				CodeTafsil = long.Parse(((DbDataReader)(object)dr)["CodeTafsil"].ToString());
			}
			if (((DbDataReader)(object)dr)["Description"].ToString() != DBNull.Value.ToString())
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["TitleTafsil"].ToString() != DBNull.Value.ToString())
			{
				TitleTafsil = ((DbDataReader)(object)dr)["TitleTafsil"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_SanadDet.cs", 69);
		}
	}

	public async Task<bool> SaveAsync(SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandText = "[dbo].[sp_ATT_SanadDet_Insert]";
			((DbCommand)(object)obj).CommandType = CommandType.StoredProcedure;
			obj.Parameters.AddWithValue("SanadID", (object)SanadId);
			obj.Parameters.AddWithValue("Debit", (object)Debit);
			obj.Parameters.AddWithValue("Credit", (object)Credit);
			obj.Parameters.AddWithValue("CodeMoein", (object)CodeMoein);
			obj.Parameters.AddWithValue("CodeTafsil", (object)CodeTafsil);
			obj.Parameters.AddWithValue("Description", (object)Text.FixString(Description));
			obj.Parameters.AddWithValue("TitleTafsil", (object)Text.FixString(TitleTafsil));
			await ((DbCommand)(object)obj).ExecuteNonQueryAsync(token);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_SanadDet.cs", 96);
			return false;
		}
	}

	public static async Task<bool> DeleteBySanadIdAsync(int sanadId, SqlTransaction tr, CancellationToken token)
	{
		try
		{
			SqlCommand obj = tr.Connection.CreateCommand();
			obj.Transaction = tr;
			((DbCommand)(object)obj).CommandText = "[dbo].[sp_ATT_SanadDet_DeleteRowBYSanadID]";
			((DbCommand)(object)obj).CommandType = CommandType.StoredProcedure;
			obj.Parameters.AddWithValue("SanadID", (object)sanadId);
			await ((DbCommand)(object)obj).ExecuteNonQueryAsync(token);
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteBySanadIdAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_SanadDet.cs", 115);
			return false;
		}
	}

	public static async Task<List<ATT_SanadDet>> GetAllAsync(string contextGuid, int sanadId, CancellationToken token)
	{
		List<ATT_SanadDet> ret = new List<ATT_SanadDet>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("sp_ATT_SanadDet_SelectRowBySanadID", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("SanadID", (object)sanadId);
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			while (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.Add(new ATT_SanadDet(dr));
			}
			((DbConnection)(object)cn).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\FLRSanad\\ATT_SanadDet.cs", 139);
		}
		return ret;
	}
}
