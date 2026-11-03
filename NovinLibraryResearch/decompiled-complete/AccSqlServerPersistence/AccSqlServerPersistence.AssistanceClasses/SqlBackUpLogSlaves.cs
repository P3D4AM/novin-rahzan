using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class SqlBackUpLogSlaves
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public EnBackUpStatus Status { get; set; }

	public string StatusName => Status.GetDisplay().GetTranslate();

	public string Description { get; set; }

	public string StatusDescription { get; set; }

	public DateTime DateM { get; set; }

	public SqlBackUpLogSlaves(SqlDataReader dr)
	{
		LoadData(dr);
	}

	public SqlBackUpLogSlaves()
	{
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Guid"] != DBNull.Value)
			{
				Guid = (Guid)((DbDataReader)(object)dr)["Guid"];
			}
			if (((DbDataReader)(object)dr)["Status"] != DBNull.Value)
			{
				Status = (EnBackUpStatus)((DbDataReader)(object)dr)["Status"];
			}
			if (((DbDataReader)(object)dr)["DateM"] != DBNull.Value)
			{
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"];
			}
			if (((DbDataReader)(object)dr)["Description"] != DBNull.Value)
			{
				Description = ((DbDataReader)(object)dr)["Description"].ToString();
			}
			if (((DbDataReader)(object)dr)["MasterGuid"] != DBNull.Value)
			{
				MasterGuid = (Guid)((DbDataReader)(object)dr)["MasterGuid"];
			}
			if (((DbDataReader)(object)dr)["ShortDescription"] != DBNull.Value)
			{
				StatusDescription = ((DbDataReader)(object)dr)["ShortDescription"].ToString();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogSlaves.cs", 38);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string connectionString)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				if (Guid == Guid.Empty)
				{
					Guid = Guid.NewGuid();
				}
				SqlCommand val = new SqlCommand("sp_SqlBackUp_Log_InsertSlaves", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@guid", (object)Guid);
					cmd.Parameters.AddWithValue("@masterGuid", (object)MasterGuid);
					cmd.Parameters.AddWithValue("@status", (object)(short)Status);
					cmd.Parameters.AddWithValue("@shortDesc", (object)(StatusDescription ?? ""));
					cmd.Parameters.AddWithValue("@desc", (object)(Description ?? ""));
					cmd.Parameters.AddWithValue("@date", (object)DateM);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
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
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogSlaves.cs", 61);
			res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogSlaves.cs", 62);
		}
		return res;
	}

	public static async Task<List<SqlBackUpLogSlaves>> GetAllAsync(Guid masterGuid, string connectionString)
	{
		List<SqlBackUpLogSlaves> ret = new List<SqlBackUpLogSlaves>();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				SqlCommand val = new SqlCommand("[sp_SqlBackUp_Log_SelectAllSlaves]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@guid", (object)masterGuid);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							ret.Add(new SqlBackUpLogSlaves(dr));
						}
						((DbDataReader)(object)dr).Close();
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
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlBackUpLogSlaves.cs", 81);
		}
		return ret;
	}
}
