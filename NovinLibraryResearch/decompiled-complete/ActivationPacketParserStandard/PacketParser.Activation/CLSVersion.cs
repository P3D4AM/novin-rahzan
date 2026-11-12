using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;

namespace PacketParser.Activation;

public class CLSVersion
{
	public static async Task<PacketParser.Entities.Department.Version> LoadDataAsync(string Code)
	{
		using (new JaegerMonitor("LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			PacketParser.Entities.Department.Version version = new PacketParser.Entities.Department.Version();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_Version_SelectRow]", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@Code", (object)Code);
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							if (await ((DbDataReader)(object)dr).ReadAsync())
							{
								version = LoadData(dr);
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
				return version;
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 38);
				return version;
			}
		}
	}

	public static async Task<List<PacketParser.Entities.Department.Version>> ListAllAsync()
	{
		using (new JaegerMonitor("ListAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			List<PacketParser.Entities.Department.Version> list = new List<PacketParser.Entities.Department.Version>();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					SqlCommand cmd = new SqlCommand("[sp_Version_SelectAll]", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								list.Add(LoadData(dr));
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "ListAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 65);
			}
			return list;
		}
	}

	[Obsolete("just for old soap apis")]
	public static PacketParser.Entities.Department.Version LoadData(string Code)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		using (new JaegerMonitor("LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			PacketParser.Entities.Department.Version result = new PacketParser.Entities.Department.Version();
			try
			{
				SqlConnection val = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand val2 = new SqlCommand("[sp_Version_SelectRow]", val);
					try
					{
						((DbCommand)(object)val2).CommandType = CommandType.StoredProcedure;
						val2.Parameters.AddWithValue("@Code", (object)Code);
						((DbConnection)(object)val).Open();
						SqlDataReader val3 = val2.ExecuteReader();
						try
						{
							if (((DbDataReader)(object)val3).Read())
							{
								result = LoadData(val3);
							}
							((DbDataReader)(object)val3).Close();
							((DbConnection)(object)val).Close();
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
				finally
				{
					((IDisposable)val)?.Dispose();
				}
				return result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 91);
				return result;
			}
		}
	}

	private static PacketParser.Entities.Department.Version LoadData(SqlDataReader dr)
	{
		try
		{
			return new PacketParser.Entities.Department.Version
			{
				Code = ((DbDataReader)(object)dr)["Code"].ToString(),
				Description = ((DbDataReader)(object)dr)["Description"].ToString(),
				DescriptionURL = ((DbDataReader)(object)dr)["DescriptionURL"].ToString(),
				FilePath = ((DbDataReader)(object)dr)["FilePath"].ToString(),
				PublishDateM = (DateTime)((DbDataReader)(object)dr)["PublishDateM"],
				StrVersion = ((DbDataReader)(object)dr)["Version"].ToString()
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 114);
			return new PacketParser.Entities.Department.Version();
		}
	}

	public static async Task<bool> DeleteAllAsync()
	{
		using (new JaegerMonitor("DeleteAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("delete from Version", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.Text;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						((DbCommand)(object)cmd).ExecuteNonQuery();
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "DeleteAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 135);
				return false;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveAsync(List<PacketParser.Entities.Department.Version> versions)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					foreach (PacketParser.Entities.Department.Version version in versions)
					{
						ret.AddReturnedValue(Save(version, cn), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 150);
					}
					((DbConnection)(object)cn).Close();
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 155);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 156);
			}
			return ret;
		}
	}

	public static async Task<bool> SaveAsync(PacketParser.Entities.Department.Version version)
	{
		return await SaveAsync(version, ConnectionString.LocalConnection);
	}

	private static async Task<bool> SaveAsync(PacketParser.Entities.Department.Version version, string connectionString)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(connectionString);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					Save(version, cn);
					((DbConnection)(object)cn).Close();
					return true;
				}
				finally
				{
					((IDisposable)cn)?.Dispose();
				}
			}
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 184);
				return false;
			}
		}
	}

	private static ReturnedSaveFuncInfo Save(PacketParser.Entities.Department.Version version, SqlConnection cn)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Expected O, but got Unknown
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		try
		{
			SqlCommand val = new SqlCommand("[sp_Version_Save]", cn);
			try
			{
				((DbCommand)(object)val).CommandType = CommandType.StoredProcedure;
				val.Parameters.AddWithValue("@Code", (object)version.Code);
				val.Parameters.AddWithValue("@PublishDateM", (object)version.PublishDateM);
				val.Parameters.AddWithValue("@PublishDateSH", (object)version.PublishDateSH);
				val.Parameters.AddWithValue("@Version", (object)version.StrVersion);
				val.Parameters.AddWithValue("@FilePath", (object)version.FilePath);
				val.Parameters.AddWithValue("@Description", (object)version.Description);
				val.Parameters.AddWithValue("@DescriptionURL", (object)version.DescriptionURL);
				((DbCommand)(object)val).ExecuteNonQuery();
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 207);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "Save", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSVersion.cs", 208);
		}
		return returnedSaveFuncInfo;
	}
}
