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

public class CLSLKTamdid
{
	public static async Task<bool> SaveAsync(long LKS, decimal Price, DateTime DateExpire, string TranInfo)
	{
		int line = 0;
		try
		{
			return await SaveAsync(LKS, Price, DateExpire, TranInfo, ConnectionString.LocalConnection);
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			WebErrorLog.Instance.StartLog(ex, $"xxxxxxxxxxxx1/Line:{line}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs", 29);
			return false;
		}
	}

	private static async Task<bool> SaveAsync(long LKS, decimal Price, DateTime DateExpire, string TranInfo, string connectionString)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				SqlConnection cn = new SqlConnection(connectionString);
				try
				{
					line = 2;
					SqlCommand cmd = new SqlCommand("[sp_LKTamdid_Insert]", cn);
					try
					{
						line = 3;
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						cmd.Parameters.AddWithValue("@LKSerial", (object)LKS);
						line = 5;
						cmd.Parameters.AddWithValue("@Price", (object)Price);
						cmd.Parameters.AddWithValue("@TODate", (object)DateExpire);
						cmd.Parameters.AddWithValue("@TranInfo", (object)TranInfo);
						line = 10;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						((DbCommand)(object)cmd).ExecuteNonQuery();
						line = 15;
						((DbConnection)(object)cn).Close();
						line = 16;
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
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}//LKS:{LKS}//Price:{Price}//DateExpire:{DateExpire}//TranInfo:{TranInfo}//connectionString:{connectionString}", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs", 66);
				return false;
			}
		}
	}

	public static async Task<List<LKTamdid>> GetAllAsync()
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_LKtamdid_NotSaved]", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = cmd.ExecuteReader();
						try
						{
							List<LKTamdid> list = new List<LKTamdid>();
							while (await ((DbDataReader)(object)dr).ReadAsync())
							{
								list.Add(LoadData(dr));
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
							return list;
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
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs", 91);
				return null;
			}
		}
	}

	private static LKTamdid LoadData(SqlDataReader dr)
	{
		try
		{
			return new LKTamdid
			{
				ID = (int)((DbDataReader)(object)dr)["ID"],
				LKSerial = ((DbDataReader)(object)dr)["LKSerial"].ToString().ParseToLong(0L),
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"],
				Price = (decimal)((DbDataReader)(object)dr)["Price"],
				TODate = (DateTime)((DbDataReader)(object)dr)["ToDate"],
				TranInfo = ((DbDataReader)(object)dr)["TranInfo"].ToString(),
				SaveOnServer = (bool)((DbDataReader)(object)dr)["SaveOnServer"],
				GUID = (Guid)((DbDataReader)(object)dr)["GUID"],
				LastTransfer = (DateTime)((DbDataReader)(object)dr)["lastTransfer"]
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSLKTamdid.cs", 116);
			return new LKTamdid();
		}
	}

	public static async Task<bool> ChangeSTAsync(List<int> ListID)
	{
		SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
		try
		{
			SqlCommand cmd = new SqlCommand("[sp_LKTamdid_UpDate]", cn);
			try
			{
				((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
				await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
				foreach (int item in ListID)
				{
					((DbParameterCollection)(object)cmd.Parameters).Clear();
					cmd.Parameters.AddWithValue("@ID", (object)item);
					((DbCommand)(object)cmd).ExecuteNonQuery();
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
}
