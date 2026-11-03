using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;

namespace AccSqlServerPersistence.AssistanceClasses;

public class SqlServerTableInformation : ISqlServerTableInformation
{
	public long TableId { get; private set; }

	public string TableName { get; private set; }

	public string SchemaName { get; private set; }

	public DateTime CreateDate { get; private set; }

	public string ContextGuid { get; set; }

	public SqlServerTableInformation(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public SqlServerTableInformation(SqlDataReader dr)
	{
		LoadData(dr);
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			TableId = ((DbDataReader)(object)dr)["TableId"].ToString().ParseToLong(0L);
			CreateDate = ((DbDataReader)(object)dr)["TableCreation"].ToString().ParseToDate();
			TableName = ((DbDataReader)(object)dr)["TableName"].ToString();
			SchemaName = ((DbDataReader)(object)dr)["SchemaName"].ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerTableInformation.cs", 37);
		}
	}

	public Task<List<ISqlServerTableInformation>> GetAllAsync(params string[] tableNames)
	{
		return GetAllAsync(ContextGuid.GetSqlConnectionString(), tableNames);
	}

	public static async Task<List<ISqlServerTableInformation>> GetAllAsync(string connectionString, params string[] tableNames)
	{
		_ = 2;
		try
		{
			List<ISqlServerTableInformation> ret = new List<ISqlServerTableInformation>();
			List<string> list = new List<string>();
			if (tableNames?.Any() ?? false)
			{
				foreach (string text in tableNames)
				{
					if (text.Contains("[") || text.Contains("]"))
					{
						list.Add(text.Replace("[", "").Replace("]", ""));
					}
				}
			}
			if (list.Count <= 0)
			{
				list.AddRange(tableNames);
			}
			SqlConnection cn = new SqlConnection(connectionString);
			try
			{
				SqlCommand cmd = new SqlCommand(" select\tsys.objects.object_id as TableId, \t\tsys.schemas.name as SchemaName, \t\tsys.objects.name as TableName, \t\tsys.objects.create_date as TableCreation  from\tsys.objects inner join sys.schemas on sys.schemas.schema_id=sys.objects.schema_id  where\tsys.objects.type='u' ", cn);
				try
				{
					try
					{
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					}
					catch
					{
						return ret;
					}
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync())
						{
							ret.Add(new SqlServerTableInformation(dr));
						}
						if (list != null && list.Any((string tableName) => !string.IsNullOrEmpty(tableName)))
						{
							list = (from p in list.Distinct()
								select p.ToLower()).ToList();
							List<ISqlServerTableInformation> list2 = ret?.Where((ISqlServerTableInformation p) => list.Contains(p.TableName.ToLower()))?.ToList();
							ret = list2;
						}
						((DbDataReader)(object)dr).Close();
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
			WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\AssistanceClasses\\SqlServerTableInformation.cs", 86);
			throw;
		}
	}
}
