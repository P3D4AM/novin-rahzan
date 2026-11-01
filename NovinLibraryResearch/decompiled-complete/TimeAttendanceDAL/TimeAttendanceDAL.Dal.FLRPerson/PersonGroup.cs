using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Attendance.InterFaces;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.FLRPerson;

[Serializable]
[DoNotPruneType]
public class PersonGroup : IPersonGroup, IHasContext
{
	public string ContextGuid { get; set; }

	public short Id { get; private set; }

	public int ParentId { get; set; }

	public short SubGroups { get; set; }

	public short Level { get; set; }

	public string Code { get; set; } = "";


	public string Name { get; set; } = "";


	public bool St { get; set; } = true;


	public bool Check { get; set; }

	public PersonGroup(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	private PersonGroup(string contextGuid, SqlDataReader dr)
	{
		ContextGuid = contextGuid;
		LoadData(dr);
	}

	public static async Task<PersonGroup> GetAsync(string contextGuid, short pId, CancellationToken token)
	{
		_ = 2;
		try
		{
			PersonGroup ret = new PersonGroup(contextGuid);
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("sp_PersonGroup_SelectRow", cn);
				try
				{
					cmd.Parameters.AddWithValue("Id", (object)pId);
					((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
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

	private bool LoadData(SqlDataReader dr)
	{
		try
		{
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				Id = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["ParentID"].ToString() != DBNull.Value.ToString())
			{
				ParentId = (int)((DbDataReader)(object)dr)["ParentID"];
			}
			if (((DbDataReader)(object)dr)["SubGroups"].ToString() != DBNull.Value.ToString())
			{
				SubGroups = (short)((DbDataReader)(object)dr)["SubGroups"];
			}
			if (((DbDataReader)(object)dr)["Level"].ToString() != DBNull.Value.ToString())
			{
				Level = (short)((DbDataReader)(object)dr)["Level"];
			}
			if (((DbDataReader)(object)dr)["Code"].ToString() != DBNull.Value.ToString())
			{
				Code = ((DbDataReader)(object)dr)["Code"].ToString();
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				St = (bool)((DbDataReader)(object)dr)["St"];
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

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("ParentID", (object)ParentId);
					cmd.Parameters.AddWithValue("SubGroups", (object)SubGroups);
					cmd.Parameters.AddWithValue("Level", (object)Level);
					cmd.Parameters.AddWithValue("Code", (object)Text.FixString(Code));
					cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
					cmd.Parameters.AddWithValue("St", (object)St.ToString());
					await ((DbConnection)(object)cn).OpenAsync(token);
					if (Id <= 0)
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_PersonGroup_Insert]";
						object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
						if (obj != null)
						{
							Id = Convert.ToInt16(obj.ToString());
						}
					}
					else
					{
						((DbCommand)(object)cmd).CommandText = "[dbo].[sp_PersonGroup_Update]";
						cmd.Parameters.AddWithValue("Id", (object)Id);
						await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					}
					((DbConnection)(object)cn).Close();
					return true;
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

	public static async Task<List<PersonGroup>> SelectAllAsync(string contextGuid, bool containsAllGroups, CancellationToken token)
	{
		List<PersonGroup> ret = new List<PersonGroup>();
		try
		{
			if (containsAllGroups)
			{
				PersonGroup item = new PersonGroup(contextGuid)
				{
					Name = "[کلیه گروه ها]",
					Id = -1
				};
				ret.Add(item);
			}
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_PersonGroup_SelectAll]", cn);
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
							PersonGroup personGroup = new PersonGroup(contextGuid, dr);
							if (containsAllGroups)
							{
								personGroup.Name = " " + personGroup.Name;
							}
							ret.Add(personGroup);
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
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			return null;
		}
		return ret;
	}

	public static async Task<int> CountGroupIDAsync(string contextGuid, short ID, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand cmd = new SqlCommand("select isnull((Select Count(*) from Person where [GroupID]=@Id and [St]='True'),0)", cn);
				try
				{
					((DbCommand)(object)cmd).CommandTimeout *= 100;
					((DbParameter)(object)cmd.Parameters.Add("@Id", SqlDbType.Int)).Value = ID;
					await ((DbConnection)(object)cn).OpenAsync(token);
					string? s = (await ((DbCommand)(object)cmd).ExecuteScalarAsync(token)).ToString();
					((DbConnection)(object)cn).Close();
					return int.Parse(s);
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
