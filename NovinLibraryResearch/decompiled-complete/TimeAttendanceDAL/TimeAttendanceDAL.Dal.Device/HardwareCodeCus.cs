using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Attendance;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.User;

namespace TimeAttendanceDAL.Dal.Device;

[Serializable]
[DoNotPruneType]
public class HardwareCodeCus : IHasContext
{
	public static List<HardwareCodeCus> HardwareCodes = null;

	public static DateTime LastUpdateTime = DateTime.Now.AddSeconds(-120.0);

	public string ContextGuid { get; private set; }

	public int PersonId { get; set; }

	public short DeviceId { get; set; }

	public string HardwareCode { get; set; }

	public string DeviceName { get; set; }

	public DateTime? LastTaradodM { get; private set; }

	public string LastTaradod { get; private set; }

	public HardwareCodeCus(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static HardwareCodeCus LoadData(string contextGuid, IDataRecord dr)
	{
		try
		{
			HardwareCodeCus hardwareCodeCus = new HardwareCodeCus(contextGuid);
			if (dr["PersonID"] != DBNull.Value)
			{
				hardwareCodeCus.PersonId = (int)dr["PersonID"];
			}
			if (dr["DeviceID"] != DBNull.Value)
			{
				hardwareCodeCus.DeviceId = (short)dr["DeviceID"];
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				hardwareCodeCus.HardwareCode = dr["HardwareCode"].ToString();
			}
			return hardwareCodeCus;
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

	public static async Task<HardwareCodeCus> GetAsync_ByPersonNoDeviceId(string contextGuid, int personNo, int deviceId, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			HardwareCodeCus ret = null;
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardwareCodeCus_SelectByPersonNo_DeviceId]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@PersonNo", (object)personNo);
					cmd.Parameters.AddWithValue("@deviceId", (object)deviceId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						if (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret = LoadData(contextGuid, (IDataRecord)dr);
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

	public static async Task<List<HardwareCodeCus>> GetAllFromDbAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			List<HardwareCodeCus> ret = new List<HardwareCodeCus>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardwareCodeCus_SelectAll]", cn);
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
							HardwareCodeCus item = LoadData(contextGuid, (IDataRecord)dr);
							ret.Add(item);
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						if (token.IsCancellationRequested)
						{
							return null;
						}
						await DeviceInfo.GetAllAsync(contextGuid, token);
						if (token.IsCancellationRequested)
						{
							return null;
						}
						if (token.IsCancellationRequested)
						{
							return null;
						}
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

	public static void Reset()
	{
		LastUpdateTime = DateTime.Now.AddSeconds(-120.0);
	}

	public static async Task<List<HardwareCodeCus>> GetAllAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (LastUpdateTime.AddSeconds(60.0) < DateTime.UtcNow)
			{
				return HardwareCodes;
			}
			List<HardwareCodeCus> ret = await LoadHardwareCodesAsync(contextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			await DeviceInfo.GetAllAsync(contextGuid, token).ConfigureAwait(continueOnCapturedContext: false);
			if (token.IsCancellationRequested)
			{
				return null;
			}
			HardwareCodes = ret;
			return HardwareCodes;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "Error during hardware codes retrieval");
			throw;
		}
	}

	private static async Task<List<HardwareCodeCus>> LoadHardwareCodesAsync(string contextGuid, CancellationToken token)
	{
		List<HardwareCodeCus> ret = new List<HardwareCodeCus>();
		SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
		try
		{
			SqlCommand val = new SqlCommand("[sp_HardwareCodeCus_SelectAll]", cn);
			((DbCommand)val).CommandType = CommandType.StoredProcedure;
			SqlCommand cmd = val;
			try
			{
				await ((DbConnection)(object)cn).OpenAsync(token).ConfigureAwait(continueOnCapturedContext: false);
				SqlDataReader dr = await cmd.ExecuteReaderAsync(token).ConfigureAwait(continueOnCapturedContext: false);
				try
				{
					while (await ((DbDataReader)(object)dr).ReadAsync(token).ConfigureAwait(continueOnCapturedContext: false))
					{
						ret.Add(LoadData(contextGuid, (IDataRecord)dr));
					}
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

	public static async Task<List<HardwareCodeCus>> GetAsync(string contextGuid, int personId, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (personId <= 0)
			{
				throw new ArgumentOutOfRangeException("personId", "شناسه شخص نباید کوچکتر یا مساوی صفر باشد");
			}
			List<HardwareCodeCus> ret = new List<HardwareCodeCus>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardwareCodeCus_GetByPersonId]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@PersonId", (object)personId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					SqlDataReader dr = await cmd.ExecuteReaderAsync(token);
					try
					{
						while (await ((DbDataReader)(object)dr).ReadAsync(token))
						{
							ret.Add(LoadData(contextGuid, (IDataRecord)dr));
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						foreach (HardwareCodeCus item in ret)
						{
							_ = item;
						}
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

	public static async Task<List<HardwareCodeCus>> GetDuplicateAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			List<HardwareCodeCus> ret = new List<HardwareCodeCus>();
			List<Tuple<int, string>> list = new List<Tuple<int, string>>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[sp_HardWareCode_SelectDuplicate]", cn);
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
							int num = 0;
							string text = "";
							if (((DbDataReader)(object)dr)["DeviceID"] != DBNull.Value)
							{
								num = (short)((DbDataReader)(object)dr)["DeviceID"];
							}
							if (((DbDataReader)(object)dr)["HardwareCode"] != DBNull.Value)
							{
								text = ((DbDataReader)(object)dr)["HardwareCode"].ToString();
							}
							if (num > 0 && text.Length > 0)
							{
								list.Add(new Tuple<int, string>(num, text));
							}
						}
						((DbDataReader)(object)dr).Close();
						((DbConnection)(object)cn).Close();
						if (list.Count > 0)
						{
							List<HardwareCodeCus> list2 = await GetAllFromDbAsync(contextGuid);
							if (list2 != null && list2.Count > 0)
							{
								foreach (Tuple<int, string> item in list)
								{
									IEnumerable<HardwareCodeCus> enumerable = list2.Where((HardwareCodeCus p) => p.DeviceId == item.Item1 && p.HardwareCode == item.Item2);
									if (enumerable.Any())
									{
										ret.AddRange(enumerable);
									}
								}
							}
						}
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

	public async Task<bool> SaveAsync(CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(ContextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_HardwareCodeCus_Insert]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("PersonID", (object)PersonId);
					cmd.Parameters.AddWithValue("DeviceID", (object)DeviceId);
					cmd.Parameters.AddWithValue("HardwareCode", (object)Text.FixString(HardwareCode));
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					Reset();
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

	public static async Task<bool> DeleteAsync(string contextGuid, int personId, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("[dbo].[sp_HardwareCodeCus_DeleteRowByPersonID]", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("PersonID", (object)personId);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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

	public static async Task<bool> DeleteAsync(string contextGuid, int personId, int deviceId, string hardwareCode, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_HardwareCodeCus_DeleteByPersonIdPersonNoHardwareCode", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("PersonID", (object)personId);
					cmd.Parameters.AddWithValue("DeviceId", (object)deviceId);
					cmd.Parameters.AddWithValue("HardwareCode", (object)hardwareCode);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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

	public static async Task<bool> DeleteAsync(string contextGuid, int deviceId, string hardwareCode, CancellationToken token)
	{
		_ = 1;
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_HardwareCodeCus_DeleteByDeviceIdHardwareCode", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("DeviceId", (object)deviceId);
					cmd.Parameters.AddWithValue("HardwareCode", (object)hardwareCode);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
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

	public static async Task<ReturnedSaveFuncInfo> SaveBioDataAync(string contextGuid)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			List<HardWareCodeCustomerModel> list = await GetAllModelAsync(contextGuid);
			if (list == null || !list.Any())
			{
				return res;
			}
			ReturnedSaveFuncInfo val = res;
			val.AddReturnedValue(await BiometricBusiness.SaveRangeAsync(contextGuid, list, UserAttendance.SelectedUser.Guid, default(CancellationToken), ""), (ReturnedState)0, "SaveBioDataAync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 356);
		}
		catch (OperationCanceledException)
		{
			res.AddErrorTokenIsCancelled("", "", "SaveBioDataAync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 358);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SaveBioDataAync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 361);
			res.AddReturnedValue(ex2, "", "SaveBioDataAync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 362);
		}
		return res;
	}

	private static async Task<List<HardWareCodeCustomerModel>> GetAllModelAsync(string contextGuid, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			List<HardWareCodeCustomerModel> list = new List<HardWareCodeCustomerModel>();
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_HardwareCodeCus_SelectAllModel", cn);
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
							if (token.IsCancellationRequested)
							{
								return null;
							}
							list.Add(LoadDataModel(dr));
						}
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
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 389);
			throw;
		}
	}

	private static HardWareCodeCustomerModel LoadDataModel(SqlDataReader dr)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			HardWareCodeCustomerModel val = new HardWareCodeCustomerModel
			{
				Guid = Guid.NewGuid()
			};
			if (((DbDataReader)(object)dr)["DeviceType"] != DBNull.Value)
			{
				val.DeviceType = (DeviceModelEnumType)Utilities.ParseToShort(((DbDataReader)(object)dr)["DeviceType"].ToString(), (short)0);
			}
			if (((DbDataReader)(object)dr)["DeviceGuid"] != DBNull.Value)
			{
				val.DeviceGuid = (Guid)((DbDataReader)(object)dr)["DeviceGuid"];
			}
			if (((DbDataReader)(object)dr)["PersonGuid"] != DBNull.Value)
			{
				val.PersonGuid = (Guid)((DbDataReader)(object)dr)["PersonGuid"];
			}
			if (((DbDataReader)(object)dr)["PersonName"] != DBNull.Value)
			{
				val.PersonName = ((DbDataReader)(object)dr)["PersonName"].ToString();
			}
			if (((DbDataReader)(object)dr)["HardwareCode"] != DBNull.Value)
			{
				val.HardWareCode = ((DbDataReader)(object)dr)["HardwareCode"].ToString();
			}
			return val;
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

	public static async Task<ReturnedSaveFuncInfo> CopyAsync(string contextGuid, Guid sourceDeviceGuid, Guid destinationDeviceGuid, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			SqlConnection cn = new SqlConnection(Extensions.GetSqlConnectionString(contextGuid));
			try
			{
				SqlCommand val = new SqlCommand("sp_HardwareCodeCus_CopyData", cn);
				((DbCommand)val).CommandType = CommandType.StoredProcedure;
				SqlCommand cmd = val;
				try
				{
					cmd.Parameters.AddWithValue("@SourceDeviceGuid", (object)sourceDeviceGuid);
					cmd.Parameters.AddWithValue("@DestinationDeviceGuid", (object)destinationDeviceGuid);
					await ((DbConnection)(object)cn).OpenAsync(token);
					await ((DbCommand)(object)cmd).ExecuteNonQueryAsync(token);
					((DbConnection)(object)cn).Close();
					Reset();
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
			res.AddErrorTokenIsCancelled("", "", "CopyAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 429);
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
			res.AddReturnedValue(ex2, "", "CopyAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Device\\HardwareCodeCUS.cs", 433);
		}
		return res;
	}
}
