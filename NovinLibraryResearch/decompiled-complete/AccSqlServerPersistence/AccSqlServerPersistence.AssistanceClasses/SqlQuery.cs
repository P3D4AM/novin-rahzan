using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DalInterfaces;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class SqlQuery
{
	private string _connectionString;

	private ENSource _source;

	public Guid Guid { get; set; }

	public string Title { get; set; }

	public string Query { get; set; }

	public bool IsChecked { get; set; }

	public SqlQuery(ISettingRepo repo, string connectionString, ENSource source)
	{
		_connectionString = connectionString;
		_source = source;
		RunScript.Instance.RunSqlScriptsAsync(repo, connectionString, _source);
	}

	public SqlQuery(SqlDataReader dr, string connectionString, ENSource source)
	{
		LoadData(dr, connectionString, source);
	}

	private void LoadData(SqlDataReader dr, string connectionString, ENSource source)
	{
		try
		{
			_connectionString = connectionString;
			_source = source;
			if (((DbDataReader)(object)dr)["Title"] != DBNull.Value)
			{
				Title = ((DbDataReader)(object)dr)["Title"].ToString();
			}
			if (((DbDataReader)(object)dr)["Query"] != DBNull.Value)
			{
				Query = ((DbDataReader)(object)dr)["Query"].ToString();
			}
			if (((DbDataReader)(object)dr)["GUID"] != DBNull.Value)
			{
				Guid = (Guid)((DbDataReader)(object)dr)["GUID"];
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 45);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string connectionString)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@Title", (object)(Title ?? ""));
					cmd.Parameters.AddWithValue("@Query", (object)(Query ?? ""));
					if (Guid == Guid.Empty)
					{
						Guid = Guid.NewGuid();
						((DbCommand)(object)cmd).CommandText = "sp_SqlExecuteQuery_Inset";
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "sp_SqlExecuteQuery_Update";
					}
					cmd.Parameters.AddWithValue("@guid", (object)Guid);
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
			ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 72);
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 73);
		}
		return ret;
	}

	public static async Task<List<SqlQuery>> GetAllAsync(string connectionString, ENSource source, string search = "")
	{
		List<SqlQuery> ret = new List<SqlQuery>();
		try
		{
			await RunScript.Instance.ExecuteSqlLogScripts(null, source, connectionString);
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				SqlCommand val = new SqlCommand("[sp_SqlExecuteQuery_SelectAll]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							ret.Add(new SqlQuery(dr, connectionString, source));
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						foreach (string item in search.SplitString())
						{
							ret = ret.Where((SqlQuery q) => q.Title.ToLower().Contains(item.ToLower())).ToList();
						}
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
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 95);
		}
		return ret;
	}

	public static async Task<SqlQuery> GetAsync(ISettingRepo repo, Guid guid, string connectionString, ENSource source)
	{
		SqlQuery ret = null;
		try
		{
			await RunScript.Instance.RunSqlScriptsAsync(repo, connectionString, source);
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				SqlCommand val = new SqlCommand("[sp_SqlExecuteQuery_SelectRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@guid", (object)guid);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync())
						{
							ret = new SqlQuery(dr, connectionString, source);
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
			WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 115);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> DeleteAsync(string connectionString)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				SqlCommand val = new SqlCommand("[sp_SqlExecuteQuery_DeleteRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@guid", (object)Guid);
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
			ret.AddReturnedValue(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 134);
			WebErrorLog.Instance.StartLog(ex, "", "DeleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlQuery.cs", 135);
		}
		return ret;
	}
}
