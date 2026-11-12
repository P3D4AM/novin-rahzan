using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.Activation;

public class ClsDepartmentSetting
{
	private static PacketParser.Entities.Department.Settings LoadData(SqlDataReader dr)
	{
		try
		{
			return new PacketParser.Entities.Department.Settings
			{
				Name = ((DbDataReader)(object)dr)["Name"].ToString(),
				Val = ((DbDataReader)(object)dr)["Val"].ToString(),
				UserID = (short)((DbDataReader)(object)dr)["UID"]
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 33);
			return new PacketParser.Entities.Department.Settings();
		}
	}

	[Obsolete("", true)]
	public static async Task<bool> GetBoolValueAsync(string propertyName, bool def, SqlConnection cn)
	{
		using (new JaegerMonitor("GetBoolValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			try
			{
				string ret = await GetValueAsync(propertyName, cn);
				if (string.IsNullOrEmpty(ret))
				{
					return def;
				}
				if (ret.Trim() == "")
				{
					return def;
				}
				bool.TryParse(ret, out var b);
				return b;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "GetBoolValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 51);
				return false;
			}
		}
	}

	[Obsolete("", true)]
	public static async Task<PacketParser.Entities.Department.Settings> LoadDataAsync(string name, short userid)
	{
		using (new JaegerMonitor("LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			PacketParser.Entities.Department.Settings settings = new PacketParser.Entities.Department.Settings();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand val = new SqlCommand("[sp_Settings_SelectRow]", cn);
					((DbCommand)val).CommandType = CommandType.StoredProcedure;
					SqlCommand cmd = val;
					try
					{
						cmd.Parameters.AddWithValue("@Name", (object)name);
						cmd.Parameters.AddWithValue("@UserID", (object)userid);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(30000).Token);
						SqlDataReader dr = cmd.ExecuteReader();
						try
						{
							if (await ((DbDataReader)(object)dr).ReadAsync())
							{
								settings = LoadData(dr);
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
							return settings;
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "ConnectionString.LocalConnection:" + ConnectionString.LocalConnection, "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 79);
				return settings;
			}
		}
	}

	[Obsolete("", true)]
	public static async Task<List<PacketParser.Entities.Department.Settings>> ListAllAsync()
	{
		using (new JaegerMonitor("ListAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			List<PacketParser.Entities.Department.Settings> list = new List<PacketParser.Entities.Department.Settings>();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand val = new SqlCommand("SELECT *  FROM  [Settings]", cn);
					((DbCommand)val).CommandType = CommandType.Text;
					SqlCommand cmd = val;
					try
					{
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								list.Add(LoadData(dr));
							}
							((DbDataReader)(object)dr).Close();
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "ListAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 101);
			}
			return list;
		}
	}

	[Obsolete("", true)]
	private static async Task SaveAsync(PacketParser.Entities.Department.Settings settings, SqlConnection cn)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			try
			{
				SqlCommand val = new SqlCommand("[sp_Settings_Save]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					((DbParameterCollection)(object)cmd.Parameters).Clear();
					cmd.Parameters.AddWithValue("@Name", (object)settings.Name);
					cmd.Parameters.AddWithValue("@Val", (object)settings.Val);
					cmd.Parameters.AddWithValue("@UserID", (object)settings.UserID);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync();
				}
				finally
				{
					((IDisposable)cmd)?.Dispose();
				}
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 159);
				throw;
			}
		}
	}

	public static async Task<string> GetValueAsync(string PropertyName, SqlConnection cn)
	{
		using (new JaegerMonitor("GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			PacketParser.Entities.Department.Settings settings = null;
			try
			{
				SqlCommand val = new SqlCommand("[sp_Settings_SelectRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@Name", (object)PropertyName);
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlDataReader dr = cmd.ExecuteReader();
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync())
						{
							settings = LoadData(dr);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						return settings?.Val ?? "";
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "GetValueAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 198);
				return "";
			}
		}
	}

	[Obsolete("just for old soap apis")]
	public static string GetValue(string PropertyName, SqlConnection cn)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_002d: Expected O, but got Unknown
		using (new JaegerMonitor("GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs"))
		{
			PacketParser.Entities.Department.Settings settings = null;
			try
			{
				SqlCommand val = new SqlCommand("[sp_Settings_SelectRow]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand val2 = val;
				try
				{
					val2.Parameters.AddWithValue("@Name", (object)PropertyName);
					((DbConnection)(object)cn).Open();
					SqlDataReader val3 = val2.ExecuteReader();
					try
					{
						if (((DbDataReader)(object)val3).Read())
						{
							settings = LoadData(val3);
						}
						((DbDataReader)(object)val3).Close();
						((DbConnection)(object)cn).Close();
						return settings?.Val ?? "";
					}
					finally
					{
						((IDisposable)val3)?.Dispose();
					}
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetValue", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\ClsDepartmentSetting.cs", 224);
				return "";
			}
		}
	}
}
