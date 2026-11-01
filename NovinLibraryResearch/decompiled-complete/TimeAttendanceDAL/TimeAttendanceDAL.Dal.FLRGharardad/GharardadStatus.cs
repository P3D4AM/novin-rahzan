using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRGharardad;

[Serializable]
[DoNotPruneType]
public class GharardadStatus : IHasContext
{
	[JsonIgnore]
	public string ContextGuid { get; set; }

	public short Id { get; private set; }

	public string Name { get; set; } = "";


	public static async Task<List<GharardadStatus>> SelectAllAsync(string contextGuid, CancellationToken token)
	{
		_ = 2;
		try
		{
			List<GharardadStatus> ret = new List<GharardadStatus>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_GharardadStatus_SelectAll]", cn);
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
							ret.Add(new GharardadStatus(contextGuid, dr));
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

	public GharardadStatus(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	private void LoadData(SqlDataReader dr)
	{
		try
		{
			if (dr != null)
			{
				if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
				{
					Id = (short)((DbDataReader)(object)dr)["Id"];
				}
				if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
				{
					Name = ((DbDataReader)(object)dr)["Name"].ToString();
				}
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
