using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Attendance;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

namespace AccSqlServerPersistence.Persistence.Repositories.Attendance;

internal class BioWriteDevicePersistenceRepository : BioWriteDeviceMasterPersistenceRepository
{
	public BioWriteDevicePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<BioWriteDeviceBusiness> FetchNextForSendAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "FetchNextForSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 21));
		BioWriteDeviceBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 2;
			try
			{
				BioWriteDeviceBusiness data = null;
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DeviceGuid", (object)deviceGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Att_BioWriteDevice_FetchNextForSend", parameters, tranName, token, CommandType.StoredProcedure, 30, "FetchNextForSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs");
				if (await dr.ReadAsync(token))
				{
					data = await LoadDataAsync(dr);
				}
				result = data;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "FetchNextForSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 35);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("FetchNextForSendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<IEnumerable<BioWriteDeviceModel>> GetAllAync(string contextGuid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 46));
		IEnumerable<BioWriteDeviceModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<BioWriteDeviceModel> data = new List<BioWriteDeviceModel>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Att_BioWriteDevice_SelectAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (base.IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						data.Add(LoadData(dr));
						continue;
					}
					result = data;
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs");
			}
		}
		return result;
	}

	private async Task<BioWriteDeviceBusiness> LoadDataAsync(DbDataReader dr)
	{
		try
		{
			BioWriteDeviceBusiness bioWriteDeviceBusiness = await BioWriteDeviceBusiness.GenerateNewAsync(base.ContextGuid, DateTime.Now, EnDeviceMessageType.AddOrUpdate);
			if (dr["BioGuid"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.BioGuid = (Guid)dr["BioGuid"];
			}
			if (dr["DeviceGuid"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.DeviceGuid = (Guid)dr["DeviceGuid"];
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.HardwareCode = (int)dr["HardwareCode"];
			}
			if (dr["HarwareName"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.HarwareName = dr["HarwareName"].ToString();
			}
			if (dr["InsertedDateM"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.InsertedDateM = (DateTime)dr["InsertedDateM"];
			}
			if (dr["TryCount"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.TryCount = (short)dr["TryCount"];
			}
			if (dr["NextTry"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.NextTry = (DateTime)dr["NextTry"];
			}
			if (dr["UserGuid"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.UserGuid = (Guid)dr["UserGuid"];
			}
			if (dr["Message"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.Message = dr["Message"].ToString();
			}
			if (dr["MessageType"] != DBNull.Value)
			{
				bioWriteDeviceBusiness.MessageType = (EnDeviceMessageType)dr["MessageType"];
			}
			return bioWriteDeviceBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadDataAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 92);
			throw;
		}
	}

	private BioWriteDeviceModel LoadData(DbDataReader dr)
	{
		try
		{
			BioWriteDeviceModel bioWriteDeviceModel = new BioWriteDeviceModel();
			if (dr["BioGuid"] != DBNull.Value)
			{
				bioWriteDeviceModel.BioGuid = (Guid)dr["BioGuid"];
			}
			if (dr["DeviceGuid"] != DBNull.Value)
			{
				bioWriteDeviceModel.DeviceGuid = (Guid)dr["DeviceGuid"];
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				bioWriteDeviceModel.HardwareCode = (int)dr["HardwareCode"];
			}
			if (dr["HarwareName"] != DBNull.Value)
			{
				bioWriteDeviceModel.HarwareName = dr["HarwareName"].ToString();
			}
			if (dr["InsertedDateM"] != DBNull.Value)
			{
				bioWriteDeviceModel.InsertedDateM = (DateTime)dr["InsertedDateM"];
			}
			if (dr["TryCount"] != DBNull.Value)
			{
				bioWriteDeviceModel.TryCount = (short)dr["TryCount"];
			}
			if (dr["NextTry"] != DBNull.Value)
			{
				bioWriteDeviceModel.NextTry = (DateTime)dr["NextTry"];
			}
			if (dr["UserGuid"] != DBNull.Value)
			{
				bioWriteDeviceModel.UserGuid = (Guid)dr["UserGuid"];
			}
			if (dr["Message"] != DBNull.Value)
			{
				bioWriteDeviceModel.Message = dr["Message"].ToString();
			}
			if (dr["DeviceName"] != DBNull.Value)
			{
				bioWriteDeviceModel.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["PersonName"] != DBNull.Value)
			{
				bioWriteDeviceModel.PersonName = dr["PersonName"].ToString();
			}
			if (dr["DeviceGroup"] != DBNull.Value)
			{
				bioWriteDeviceModel.DeviceGroup = (EnDeviceGroup)(short)dr["DeviceGroup"];
			}
			if (dr["FingerPrintCount"] != DBNull.Value)
			{
				bioWriteDeviceModel.FingerPrintCount = (short)dr["FingerPrintCount"];
			}
			if (dr["HasPalm"] != DBNull.Value)
			{
				bioWriteDeviceModel.HasPalm = (bool)dr["HasPalm"];
			}
			if (dr["HasPassword"] != DBNull.Value)
			{
				bioWriteDeviceModel.HasPassword = (bool)dr["HasPassword"];
			}
			if (dr["HasFace"] != DBNull.Value)
			{
				bioWriteDeviceModel.HasFace = (bool)dr["HasFace"];
			}
			if (dr["HasCard"] != DBNull.Value)
			{
				bioWriteDeviceModel.HasCard = (bool)dr["HasCard"];
			}
			return bioWriteDeviceModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDevicePersistenceRepository.cs", 122);
			throw;
		}
	}
}
