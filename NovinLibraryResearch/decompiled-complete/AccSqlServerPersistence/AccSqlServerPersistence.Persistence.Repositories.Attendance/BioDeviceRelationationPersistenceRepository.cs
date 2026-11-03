using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Model;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

namespace AccSqlServerPersistence.Persistence.Repositories.Attendance;

internal class BioDeviceRelationationPersistenceRepository : BioDeviceRelationationBasePersistenceRepository
{
	public BioDeviceRelationationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override async Task<IEnumerable<BioDeviceRelationModel>> GetAllAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioDeviceRelationationPersistenceRepository.cs", 20));
		IEnumerable<BioDeviceRelationModel> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<BioDeviceRelationModel> data = new List<BioDeviceRelationModel>();
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@DeviceGuid", (object)deviceGuid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_Att_BioDeviceRelation_SelectAll", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioDeviceRelationationPersistenceRepository.cs");
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioDeviceRelationationPersistenceRepository.cs", 37);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioDeviceRelationationPersistenceRepository.cs");
			}
		}
		return result;
	}

	private BioDeviceRelationModel LoadData(DbDataReader dr)
	{
		try
		{
			BioDeviceRelationModel bioDeviceRelationModel = new BioDeviceRelationModel();
			if (dr["DeviceGuid"] != DBNull.Value)
			{
				bioDeviceRelationModel.DeviceGuid = (Guid)dr["DeviceGuid"];
			}
			if (dr["DeviceName"] != DBNull.Value)
			{
				bioDeviceRelationModel.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["BioGuid"] != DBNull.Value)
			{
				bioDeviceRelationModel.BioGuid = (Guid)dr["BioGuid"];
			}
			if (dr["PersonName"] != DBNull.Value)
			{
				bioDeviceRelationModel.PersonName = dr["PersonName"].ToString();
			}
			if (dr["HardwareCode"] != DBNull.Value)
			{
				bioDeviceRelationModel.HardwareCode = (int)dr["HardwareCode"];
			}
			if (dr["Privilage"] != DBNull.Value)
			{
				bioDeviceRelationModel.Privilage = (EnAttendancePrivilage)(short)dr["Privilage"];
			}
			if (dr["Modified"] != DBNull.Value)
			{
				bioDeviceRelationModel.Modified = (DateTime)dr["Modified"];
			}
			if (dr["FingerPrintCount"] != DBNull.Value)
			{
				bioDeviceRelationModel.FingerPrintCount = (short)dr["FingerPrintCount"];
			}
			if (dr["HasPalm"] != DBNull.Value)
			{
				bioDeviceRelationModel.HasPalm = (bool)dr["HasPalm"];
			}
			if (dr["HasPassword"] != DBNull.Value)
			{
				bioDeviceRelationModel.HasPassword = (bool)dr["HasPassword"];
			}
			if (dr["HasFace"] != DBNull.Value)
			{
				bioDeviceRelationModel.HasFace = (bool)dr["HasFace"];
			}
			if (dr["HasCard"] != DBNull.Value)
			{
				bioDeviceRelationModel.HasCard = (bool)dr["HasCard"];
			}
			return bioDeviceRelationModel;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccSqlServerPersistence\\Persistence\\Repositories\\Attendance\\BioDeviceRelationationPersistenceRepository.cs", 67);
			throw;
		}
	}
}
