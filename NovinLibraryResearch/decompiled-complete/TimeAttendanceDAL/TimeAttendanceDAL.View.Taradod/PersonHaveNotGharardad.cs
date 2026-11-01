using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using Serilog;

namespace TimeAttendanceDAL.View.Taradod;

public class PersonHaveNotGharardad
{
	public int PersonId { get; set; }

	public string FullName { get; set; }

	public string CodeTafsil { get; set; }

	public string CodeMoein { get; set; }

	public PersonHaveNotGharardad(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				PersonId = (int)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["FullName"].ToString() != DBNull.Value.ToString())
			{
				FullName = ((DbDataReader)(object)dr)["FullName"].ToString();
			}
			if (((DbDataReader)(object)dr)["CodeTafsil"].ToString() != DBNull.Value.ToString())
			{
				CodeTafsil = ((DbDataReader)(object)dr)["CodeTafsil"].ToString();
			}
			if (((DbDataReader)(object)dr)["CodeMoein"].ToString() != DBNull.Value.ToString())
			{
				CodeMoein = ((DbDataReader)(object)dr)["CodeMoein"].ToString();
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<PersonHaveNotGharardad>> GetAllAsync(string contextGuid, List<int> logIds, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<PersonHaveNotGharardad> ret = new List<PersonHaveNotGharardad>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_Person_HaveNotGharardad", cn);
				try
				{
					cmd.Parameters.AddWithValue("@LogonPersonelIDs", (object)string.Join(",", logIds));
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync();
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(new PersonHaveNotGharardad(dr));
						}
						((DbConnection)(object)cn).Close();
						((DbDataReader)(object)dr).Close();
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
}
