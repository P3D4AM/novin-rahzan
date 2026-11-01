using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace TimeAttendanceDAL.Dal.Reports;

[Serializable]
public class RPTGenDet : IHasContext
{
	public string ContextGuid { get; set; }

	public int RepID { get; set; }

	public string ParameterName { get; set; } = "";


	public short Order { get; set; }

	public bool Visible { get; set; } = true;


	public RPTGenDet()
	{
	}

	public RPTGenDet(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	internal static async Task<RPTGenDet> LoadDataAsync(int pID, string PName, string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			RPTGenDet ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("sp_ReportGeneratorDHozor_SelectRow", cn);
			cmd.Parameters.AddWithValue("RepID", (object)pID);
			cmd.Parameters.AddWithValue("ParameterName", (object)PName);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			if (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret = LoadData(contextGuid, dr);
			}
			((DbConnection)(object)cn).Close();
			return ret;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGenDet.cs", 53);
			throw;
		}
	}

	private static RPTGenDet LoadData(string contextGuid, SqlDataReader dr)
	{
		RPTGenDet rPTGenDet = new RPTGenDet();
		try
		{
			rPTGenDet.ContextGuid = contextGuid;
			if (((DbDataReader)(object)dr)["RepID"].ToString() != DBNull.Value.ToString())
			{
				rPTGenDet.RepID = (int)((DbDataReader)(object)dr)["RepID"];
			}
			if (((DbDataReader)(object)dr)["ParameterName"].ToString() != DBNull.Value.ToString())
			{
				rPTGenDet.ParameterName = ((DbDataReader)(object)dr)["ParameterName"].ToString();
			}
			if (((DbDataReader)(object)dr)["Order"].ToString() != DBNull.Value.ToString())
			{
				rPTGenDet.Order = (short)((DbDataReader)(object)dr)["Order"];
			}
			if (((DbDataReader)(object)dr)["Visible"].ToString() != DBNull.Value.ToString())
			{
				rPTGenDet.Visible = (bool)((DbDataReader)(object)dr)["Visible"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGenDet.cs", 71);
			throw;
		}
		return rPTGenDet;
	}

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			SqlCommand val = new SqlCommand("", cn);
			((DbCommand)val).CommandType = CommandType.StoredProcedure;
			SqlCommand cmd = val;
			cmd.Parameters.AddWithValue("RepID", (object)RepID);
			cmd.Parameters.AddWithValue("ParameterName", (object)Text.FixString(ParameterName));
			cmd.Parameters.AddWithValue("Order", (object)Order);
			cmd.Parameters.AddWithValue("Visible", (object)Visible);
			await ((DbConnection)(object)cn).OpenAsync(token);
			((DbCommand)(object)cmd).CommandText = "dbo.sp_ReportGeneratorDHozor_Insert";
			cmd.ExecuteReader();
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGenDet.cs", 100);
			throw;
		}
	}

	public static async Task<bool> DeleteAsync(string contextGuid, int pRepID, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand val = new SqlCommand("[dbo].[sp_ReportGeneratorDHozor_DeleteRow]", cn);
			((DbCommand)val).CommandType = CommandType.StoredProcedure;
			SqlCommand cmd = val;
			cmd.Parameters.AddWithValue("RepID", (object)pRepID);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGenDet.cs", 124);
			throw;
		}
	}

	public static async Task<List<RPTGenDet>> GetAllAsync(string contextGuid, int pRepID, CancellationToken token = default(CancellationToken))
	{
		List<RPTGenDet> ret = new List<RPTGenDet>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand val = new SqlCommand("[sp_ReportGeneratorDHozor_SelectAll]", cn);
			((DbCommand)val).CommandType = CommandType.StoredProcedure;
			SqlCommand cmd = val;
			cmd.Parameters.AddWithValue("RepID", (object)pRepID);
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			while (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.Add(LoadData(contextGuid, dr));
			}
			((DbConnection)(object)cn).Close();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Reports\\RPTGenDet.cs", 151);
		}
		return ret;
	}
}
