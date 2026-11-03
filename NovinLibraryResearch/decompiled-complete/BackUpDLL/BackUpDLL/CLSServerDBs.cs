using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace BackUpDLL;

public class CLSServerDBs
{
	private string _name = "";

	private string _dbid = "";

	private string _sid = "";

	private string _mode = "";

	private string _status = "";

	private string _status2 = "";

	private string _crDate = "";

	private string _reserved = "";

	private string _category = "";

	private string _cmptLevel = "";

	private string _fileName = "";

	private string _version = "";

	public string Name => _name;

	public string DBID => _dbid;

	public string SID => _sid;

	public string Mode => _mode;

	public string Status => _status;

	public string Status2 => _status2;

	public string CRDate => _crDate;

	public string Reserved => _reserved;

	public string Category => _category;

	public string cmptLevel => _cmptLevel;

	public string FileName => _fileName;

	public string Version => _version;

	public static Task<CLSServerDBs> GetAsync(string ConnectionString)
	{
		return LoadDataAsync(ConnectionString);
	}

	private static async Task<CLSServerDBs> LoadDataAsync(string SqlConnectionString)
	{
		_ = 3;
		try
		{
			CLSServerDBs res = new CLSServerDBs();
			SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(SqlConnectionString);
			string text = SqlConnectionString.Replace(";Initial Catalog=" + val.InitialCatalog + ";", ";Initial Catalog=;");
			SqlConnection cn = new SqlConnection(text);
			try
			{
				SqlCommand cmd = new SqlCommand("Select * from sysdatabases where name='" + val.InitialCatalog + "'", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync())
						{
							res = await LoadDataAsync(dr);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return res;
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
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 64);
			throw;
		}
	}

	private static Task<CLSServerDBs> LoadDataAsync(SqlDataReader reader)
	{
		try
		{
			return Task.FromResult(new CLSServerDBs
			{
				_name = GetValue(reader, "name"),
				_dbid = GetValue(reader, "dbid"),
				_sid = GetValue(reader, "sid"),
				_mode = GetValue(reader, "mode"),
				_status = GetValue(reader, "status"),
				_status2 = GetValue(reader, "status2"),
				_crDate = GetValue(reader, "crdate"),
				_reserved = GetValue(reader, "reserved"),
				_category = GetValue(reader, "category"),
				_cmptLevel = GetValue(reader, "cmptlevel"),
				_fileName = GetValue(reader, "filename"),
				_version = GetValue(reader, "version")
			});
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 92);
			throw;
		}
	}

	private static string GetValue(SqlDataReader dr, string name)
	{
		try
		{
			int ordinal = ((DbDataReader)(object)dr).GetOrdinal(name);
			if (ordinal < 0)
			{
				return "";
			}
			object value = ((DbDataReader)(object)dr).GetValue(ordinal);
			return (value == null) ? "" : value.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 108);
			return "";
		}
	}

	public static async Task<ReturnedSaveFuncInfoWithValue<List<CLSServerDBs>>> GetAllAsync(string serverCn, CancellationToken token)
	{
		ReturnedSaveFuncInfoWithValue<List<CLSServerDBs>> ret = new ReturnedSaveFuncInfoWithValue<List<CLSServerDBs>>
		{
			value = new List<CLSServerDBs>()
		};
		try
		{
			SqlConnection cn = new SqlConnection();
			try
			{
				SqlConnectionStringBuilder val = new SqlConnectionStringBuilder(serverCn)
				{
					InitialCatalog = "master"
				};
				((DbConnection)(object)cn).ConnectionString = ((DbConnectionStringBuilder)(object)val).ConnectionString;
				SqlCommand cmd = new SqlCommand("Select * from sysdatabases", cn);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (token.IsCancellationRequested)
					{
						ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 123);
					}
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (token.IsCancellationRequested)
						{
							ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 125);
						}
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							if (token.IsCancellationRequested)
							{
								ret.AddErrorTokenIsCancelled("token is Cancelled!", "CancellationToken", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 128);
							}
							List<CLSServerDBs> value = ret.value;
							value.Add(await LoadDataAsync(dr));
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
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 134);
		}
		catch (SqlException val2)
		{
			SqlException val3 = val2;
			if (((Exception)(object)val3).Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog((Exception)(object)val3, "لاگ جدید", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 138);
			ret.AddReturnedValue((Exception)(object)val3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 139);
		}
		catch (SqlException val4)
		{
			SqlException val5 = val4;
			if (((Exception)(object)val5).Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog((Exception)(object)val5, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 144);
			ret.AddReturnedValue((Exception)(object)val5, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 145);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 147);
			ret.AddReturnedValue(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\BackupDLL\\CLSServerDBs.cs", 147);
		}
		return ret;
	}
}
