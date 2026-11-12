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

public class ClsActivationTBL
{
	public static Task<bool> SaveAsync(string sender, ActivationTBL activationTbl)
	{
		return SaveAsync(sender, activationTbl, ConnectionString.LocalConnection);
	}

	public static async Task SaveAsync(string sender, List<ActivationTBL> tbls)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					foreach (ActivationTBL activationTbl in tbls)
					{
						Task.Run(() => SaveAsync(sender, activationTbl));
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
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 33);
			}
		}
	}

	private static async Task<bool> SaveAsync(string sender, ActivationTBL activationTbl, string ConnectionString)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString);
				try
				{
					await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
					Save(activationTbl, cn);
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
				WebErrorLog.Instance.StartLog(ex, "save on connection:" + ConnectionString + "\r\nSender:" + sender + "\r\ndata:" + Json.ToStringJson(activationTbl, indented: true), "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 49);
				return false;
			}
		}
	}

	private static void Save(ActivationTBL activationTbl, SqlConnection cn)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		SqlCommand val = new SqlCommand("[sp_ActivationTbl_Insert]", cn);
		try
		{
			((DbCommand)(object)val).CommandType = CommandType.StoredProcedure;
			val.Parameters.AddWithValue("@InputMsg", (object)(activationTbl.InputMsg ?? ""));
			val.Parameters.AddWithValue("@OutputMsg", (object)(activationTbl.OutputMsg ?? ""));
			val.Parameters.AddWithValue("@Number", (object)(activationTbl.Number ?? ""));
			val.Parameters.AddWithValue("@DateM", (object)activationTbl.DateM);
			val.Parameters.AddWithValue("@DateSH", (object)activationTbl.DateSH);
			val.Parameters.AddWithValue("ExpiredDateSH", (object)activationTbl.ExpiredDateSH);
			val.Parameters.AddWithValue("@LKSerial", (object)activationTbl.LKSerial);
			val.Parameters.AddWithValue("@SoftSerial", (object)activationTbl.SoftSerial);
			val.Parameters.AddWithValue("@ExpiredDateM", (object)activationTbl.ExpiredDateM);
			val.Parameters.AddWithValue("@RefrencedID", (object)activationTbl.RefrencedID);
			val.Parameters.AddWithValue("@ST", (object)activationTbl.ST);
			((DbCommand)(object)val).ExecuteNonQuery();
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	public static async Task<List<ActivationTBL>> SelectAllAsync()
	{
		using (new JaegerMonitor("SelectAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_ActivationTbl_SelectAll]", cn);
					try
					{
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							List<ActivationTBL> list = new List<ActivationTBL>();
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
				WebErrorLog.Instance.StartLog(ex, "", "SelectAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 94);
				return null;
			}
		}
	}

	public static async Task<ActivationTBL> LoadDataAsync(int Id)
	{
		using (new JaegerMonitor("LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			ActivationTBL activationTbl = new ActivationTBL();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("[sp_ActivationTbl_SelectRow]", cn);
					try
					{
						cmd.Parameters.AddWithValue("@ID", (object)Id);
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
							if (await ((DbDataReader)(object)dr).ReadAsync())
							{
								activationTbl = LoadData(dr);
							}
							((DbDataReader)(object)dr).Close();
							((DbConnection)(object)cn).Close();
							return activationTbl;
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
				WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 119);
				return new ActivationTBL();
			}
		}
	}

	private static ActivationTBL LoadData(SqlDataReader dr)
	{
		try
		{
			return new ActivationTBL
			{
				ID = (int)((DbDataReader)(object)dr)["ID"],
				InputMsg = ((DbDataReader)(object)dr)["InputMsg"].ToString(),
				OutputMsg = ((DbDataReader)(object)dr)["OutputMsg"].ToString(),
				Number = ((DbDataReader)(object)dr)["Number"].ToString(),
				DateM = (DateTime)((DbDataReader)(object)dr)["DateM"],
				DateSH = ((DbDataReader)(object)dr)["DateSh"].ToString(),
				ExpiredDateM = (DateTime)((DbDataReader)(object)dr)["ExpiredDateM"],
				ExpiredDateSH = ((DbDataReader)(object)dr)["ExpiredDateSH"].ToString(),
				LKSerial = ((DbDataReader)(object)dr)["LKSerial"].ToString().ParseToLong(0L),
				SoftSerial = ((DbDataReader)(object)dr)["SoftSerial"].ToString(),
				RefrencedID = (long)((DbDataReader)(object)dr)["RefrencedID"],
				ST = (short)((DbDataReader)(object)dr)["st"]
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 146);
			return new ActivationTBL();
		}
	}

	public static async Task<List<ActivationTBL>> GetAll_NotSentAsync()
	{
		using (new JaegerMonitor("GetAll_NotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			List<ActivationTBL> list = new List<ActivationTBL>();
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("sp_ActivationTbl_SelectAll_Unsents", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						SqlDataReader dr = await cmd.ExecuteReaderAsync();
						try
						{
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
				WebErrorLog.Instance.StartLog(ex, "", "GetAll_NotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 170);
				return list;
			}
		}
	}

	public static async Task<int> CountRecordAsync(string Number, string DateSh)
	{
		using (new JaegerMonitor("CountRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("select Count(ID) from ActivationTbl where Number=@Number and DateSH=@DateSH", cn);
					try
					{
						cmd.Parameters.AddWithValue("@Number", (object)Number);
						cmd.Parameters.AddWithValue("@DateSH", (object)DateSh);
						((DbCommand)(object)cmd).CommandType = CommandType.Text;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						int Result = int.Parse(((DbCommand)(object)cmd).ExecuteScalar().ToString());
						((DbConnection)(object)cn).Close();
						return Result;
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
				WebErrorLog.Instance.StartLog(ex, "", "CountRecordAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 192);
				return 0;
			}
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SaveDeliveryAsync(List<int> ids)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs"))
		{
			try
			{
				SqlConnection cn = new SqlConnection(ConnectionString.LocalConnection);
				try
				{
					SqlCommand cmd = new SqlCommand("sp_ActivationTbl_Delived", cn);
					try
					{
						((DbCommand)(object)cmd).CommandType = CommandType.StoredProcedure;
						await ((DbConnection)(object)cn).OpenAsync(new CancellationTokenSource(60000).Token);
						foreach (int id in ids)
						{
							((DbParameterCollection)(object)cmd.Parameters).Clear();
							cmd.Parameters.AddWithValue("ID", (object)id);
							((DbCommand)(object)cmd).ExecuteNonQuery();
						}
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
			catch (Exception ex2)
			{
				Exception ex = ex2;
				WebErrorLog.Instance.StartLog(ex, "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 217);
				ret.AddReturnedValue(ex, "", "SaveDeliveryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSActivationTBL.cs", 218);
			}
			return ret;
		}
	}
}
