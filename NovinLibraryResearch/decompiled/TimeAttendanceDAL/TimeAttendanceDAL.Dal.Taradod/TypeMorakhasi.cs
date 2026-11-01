using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Settings;

namespace TimeAttendanceDAL.Dal.Taradod;

[Serializable]
[DoNotPruneType]
public class TypeMorakhasi : IHasContext
{
	[JsonIgnore]
	public string ContextGuid { get; set; }

	public short Id { get; set; }

	public string Name { get; set; } = "";


	public bool Bimeh { get; set; }

	public bool Ezafat { get; set; }

	public bool Kosorat { get; set; }

	public bool Hoghogh { get; set; }

	public bool Maliyat { get; set; }

	public bool ST { get; set; } = true;


	public TypeMorakhasi(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<TypeMorakhasi> GetAsync(string contextGuid, short id, CancellationToken token)
	{
		_ = 1;
		try
		{
			TypeMorakhasi ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			SqlCommand cmd = new SqlCommand("sp_TypeMorakhasi_SelectRow", cn);
			cmd.Parameters.AddWithValue("Id", (object)id);
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
			Log.Error(ex, "");
			throw;
		}
	}

	private static TypeMorakhasi LoadData(string contextGuid, SqlDataReader dr)
	{
		try
		{
			TypeMorakhasi typeMorakhasi = new TypeMorakhasi(contextGuid);
			if (((DbDataReader)(object)dr)["Id"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Id = (short)((DbDataReader)(object)dr)["Id"];
			}
			if (((DbDataReader)(object)dr)["Name"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Name = ((DbDataReader)(object)dr)["Name"].ToString();
			}
			if (((DbDataReader)(object)dr)["Kosorat"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Kosorat = (bool)((DbDataReader)(object)dr)["Kosorat"];
			}
			if (((DbDataReader)(object)dr)["Ezafat"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Ezafat = (bool)((DbDataReader)(object)dr)["Ezafat"];
			}
			if (((DbDataReader)(object)dr)["Bimeh"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Bimeh = (bool)((DbDataReader)(object)dr)["Bimeh"];
			}
			if (((DbDataReader)(object)dr)["Hoghogh"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Hoghogh = (bool)((DbDataReader)(object)dr)["Hoghogh"];
			}
			if (((DbDataReader)(object)dr)["St"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.ST = (bool)((DbDataReader)(object)dr)["St"];
			}
			if (((DbDataReader)(object)dr)["Maliyat"].ToString() != DBNull.Value.ToString())
			{
				typeMorakhasi.Maliyat = (bool)((DbDataReader)(object)dr)["Maliyat"];
			}
			return typeMorakhasi;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TypeMorakhasi.cs", 73);
			throw;
		}
	}

	public async Task<bool> InsertAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			SqlCommand cmd = new SqlCommand("sp_TypeMorakhasi_Insert", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("Id", (object)Id);
			cmd.Parameters.AddWithValue("Name", (object)Text.FixString(Name));
			cmd.Parameters.AddWithValue("Bimeh ", (object)Bimeh);
			cmd.Parameters.AddWithValue("Hoghogh", (object)Hoghogh);
			cmd.Parameters.AddWithValue("St", (object)ST);
			cmd.Parameters.AddWithValue("Ezafat", (object)Ezafat);
			cmd.Parameters.AddWithValue("Kosorat", (object)Kosorat);
			cmd.Parameters.AddWithValue("maliyat", (object)Maliyat);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public async Task<bool> UpdateAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(EntityCacheConfig.CacheInstance.ContextGuid));
			SqlCommand cmd = new SqlCommand("sp_TypeMorakhasi_Update", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("Id", (object)Id);
			cmd.Parameters.AddWithValue("Name", (object)Name);
			cmd.Parameters.AddWithValue("Bimeh ", (object)Bimeh);
			cmd.Parameters.AddWithValue("Hoghogh", (object)Hoghogh);
			cmd.Parameters.AddWithValue("St", (object)ST);
			cmd.Parameters.AddWithValue("Ezafat", (object)Ezafat);
			cmd.Parameters.AddWithValue("Kosorat", (object)Kosorat);
			cmd.Parameters.AddWithValue("maliyat", (object)Maliyat);
			await ((DbConnection)(object)cn).OpenAsync(token);
			await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
			((DbConnection)(object)cn).Close();
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<List<TypeMorakhasi>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		List<TypeMorakhasi> ret = new List<TypeMorakhasi>();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("[sp_TypeMorakhasi_SelectAll]", cn);
			((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
			await ((DbConnection)(object)cn).OpenAsync(token);
			SqlDataReader dr = cmd.ExecuteReader();
			while (await ((DbDataReader)(object)dr).ReadAsync(token))
			{
				ret.Add(LoadData(contextGuid, dr));
			}
			((DbConnection)(object)cn).Close();
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	public static async Task<int> MaxIDAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			SqlCommand cmd = new SqlCommand("SELECT  max(Id) FROM [TypeMorakhasi]", cn);
			await ((DbConnection)(object)cn).OpenAsync(token);
			object obj = await ((DbCommand)(object)cmd).ExecuteScalarAsync(token);
			((DbConnection)(object)cn).Close();
			int result = 0;
			if (obj != null)
			{
				int.TryParse(obj.ToString(), out result);
			}
			return result + 1;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "MaxIDAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Taradod\\TypeMorakhasi.cs", 175);
			return 1;
		}
	}
}
