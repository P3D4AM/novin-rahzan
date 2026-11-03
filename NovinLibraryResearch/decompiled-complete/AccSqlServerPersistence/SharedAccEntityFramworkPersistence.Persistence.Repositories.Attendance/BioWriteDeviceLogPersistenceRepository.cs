using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class BioWriteDeviceLogPersistenceRepository : BioWriteDeviceLogBasePersistenceRepository
{
	public BioWriteDeviceLogPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<BioWriteDeviceLogModel>> GetAllAsync(BioWriteDeviceLogFilter filter, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDeviceLogPersistenceRepository.cs", 21));
		IEnumerable<BioWriteDeviceLogModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<BioWriteDeviceLogModel> data = new List<BioWriteDeviceLogModel>();
				List<SqlParameter> list = new List<SqlParameter>();
				if ((filter?.DeviceGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@DeviceGuid", (object)filter.DeviceGuid.Value));
				}
				if ((filter?.PersonGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@PersonGuid", (object)filter.PersonGuid.Value));
				}
				if ((filter?.UserGuid ?? Guid.Empty) != Guid.Empty)
				{
					list.Add(new SqlParameter("@UserGuid", (object)filter.UserGuid.Value));
				}
				if (filter?.DateFrom.HasValue ?? false)
				{
					list.Add(new SqlParameter("@DateFrom", (object)filter.DateFrom.Value));
				}
				if (filter?.DateTo.HasValue ?? false)
				{
					list.Add(new SqlParameter("@DateTo", (object)filter.DateTo.Value));
				}
				if ((filter?.HardwareCode).GetValueOrDefault() != 0)
				{
					list.Add(new SqlParameter("@HardwareCode", (object)filter.HardwareCode.Value));
				}
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Att_BioWriteDeviceLog_SelectAll", list, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDeviceLogPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDeviceLogPersistenceRepository.cs", 44);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDeviceLogPersistenceRepository.cs");
			}
		}
		return result;
	}

	private BioWriteDeviceLogModel LoadData(DbDataReader dr)
	{
		try
		{
			BioWriteDeviceLogModel bioWriteDeviceLogModel = new BioWriteDeviceLogModel();
			if (dr["Guid"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.Guid = (Guid)dr["Guid"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.Modified = (DateTime)dr["Modified"];
			}
			if (dr["DateM"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.DateM = (DateTime)dr["DateM"];
			}
			if (dr["Description"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.Description = dr["Description"].ToString();
			}
			if (dr["DeviceGuid"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.DeviceGuid = (Guid)dr["DeviceGuid"];
			}
			if (dr["DeviceName"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["BioGuid"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.BioGuid = (Guid)dr["BioGuid"];
			}
			if (dr["PersonName"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.PersonName = dr["PersonName"].ToString();
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.HardwareCode = (int)dr["HardwareCode"];
			}
			if (dr["HardwareName"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.HardwareName = dr["HardwareName"].ToString();
			}
			if (dr["Result"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.Result = (ReturnedState)(short)dr["Result"];
			}
			if (dr["UserGuid"] != DBNull.Value)
			{
				bioWriteDeviceLogModel.UserGuid = (Guid)dr["UserGuid"];
			}
			return bioWriteDeviceLogModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioWriteDeviceLogPersistenceRepository.cs", 74);
			throw;
		}
	}
}
