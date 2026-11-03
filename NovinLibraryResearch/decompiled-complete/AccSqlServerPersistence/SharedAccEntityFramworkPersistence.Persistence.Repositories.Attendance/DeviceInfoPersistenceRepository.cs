using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Filters.Attendance;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class DeviceInfoPersistenceRepository : IDeviceInfoRepository, ISyncRepository<DeviceInfoBusiness>, IEditableDelivery, IRepository<DeviceInfoBusiness>, INoGuidRepository<DeviceInfoBusiness>, IDisposable
{
	private bool _isDisposed;

	public ContextHolder DatabaseContext { get; private set; }

	public WebContextHolder Context => DatabaseContext.Context;

	public string ContextGuid => Context?.Guid;

	public bool IsDisposed => _isDisposed;

	internal DeviceInfoPersistenceRepository(ContextHolder context)
	{
		DatabaseContext = context;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 37);
		}
	}

	public async Task<List<DeviceInfoBusiness>> GetAllAsync(DeviceInfoFilter filter, string tranName, CancellationToken token)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 41);
		List<DeviceInfoBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				List<DeviceInfoBusiness> list = new List<DeviceInfoBusiness>();
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_DeviceInfo_SelectAll", null, tranName, token, CommandType.StoredProcedure, 30, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs");
				while (true)
				{
					if (await dr.ReadAsync(token))
					{
						if (IsDisposed || token.IsCancellationRequested)
						{
							result = null;
							break;
						}
						list.Add(LoadData(ContextGuid, dr));
						continue;
					}
					dr.Close();
					if (IsDisposed || token.IsCancellationRequested || !(list?.Any() ?? false))
					{
						result = null;
						break;
					}
					if ((filter?.ModelList?.Any()).GetValueOrDefault())
					{
						list = list?.Where((DeviceInfoBusiness q) => filter.ModelList.Contains(q.Model))?.ToList();
					}
					result = list;
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<DeviceInfoBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		BaseModelContext c = await DatabaseContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 75);
		DeviceInfoBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				DeviceInfoBusiness item = null;
				List<SqlParameter> parameters = new List<SqlParameter>
				{
					new SqlParameter("@Guid", (object)guid)
				};
				using DbDataReader dr = await c.ExecuteReaderAsync("sp_DeviceInfo_SelectRowByGuid", parameters, tranName, token, CommandType.StoredProcedure, 30, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs");
				if (await dr.ReadAsync(token))
				{
					item = LoadData(ContextGuid, dr);
				}
				dr.Close();
				result = item;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 91);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs");
			}
		}
		return result;
	}

	public Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public DeviceInfoBusiness Get(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<DeviceInfoBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<IEnumerable<DeviceInfoBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<List<Guid>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<DeviceInfoBusiness> GetByNumberAsync(short number, string tranName, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<DeviceInfoBusiness> GetBySerialAsync(string serial, string tranName, CancellationToken token)
	{
		throw new NotImplementedException();
	}

	public Task<DeviceInfoBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> KeepRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveAsync(DeviceInfoBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<DeviceInfoBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	private static DeviceInfoBusiness LoadData(string contextGuid, DbDataReader dr)
	{
		try
		{
			DeviceInfoBusiness deviceInfoBusiness = new DeviceInfoBusiness(contextGuid);
			if (dr["Id"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.Id = (short)dr["Id"];
			}
			if (dr["Guid"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.Guid = (Guid)dr["Guid"];
			}
			if (dr["SerialNumberStr"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.SerialNumberStr = dr["SerialNumberStr"].ToString();
			}
			if (dr["IP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.Ip = dr["IP"].ToString().Trim();
			}
			if (dr["TCPPort"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.TcpPort = (int)dr["TCPPort"];
			}
			if (dr["TCPPort"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.PortRealtime = (int)dr["PortRealTime"];
			}
			if (dr["GetNewTaradodOnly"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.GetNewTaradodOnly = (bool)dr["GetNewTaradodOnly"];
			}
			if (dr["DeviceName"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.DeviceName = dr["DeviceName"].ToString();
			}
			if (dr["DeviceType"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.DeviceType = dr["DeviceType"].ToString();
			}
			if (dr["MACAddress"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.MacAddress = dr["MACAddress"].ToString();
			}
			if (dr["SubnetMask"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.SubnetMask = dr["SubnetMask"].ToString();
			}
			if (dr["UseDHCP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.UseDhcp = (bool)dr["UseDHCP"];
			}
			if (dr["UseDHCP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.UseRealTime = (bool)dr["UseRealTime"];
			}
			if (dr["EngineVersion"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.EngineVersion = dr["EngineVersion"].ToString();
			}
			if (dr["FWVersion"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.FwVersion = dr["FWVersion"].ToString();
			}
			if (dr["TransferMode"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.TransferMode = (TypeConnection)dr["TransferMode"];
			}
			if (dr["HostIP"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.HostIp = dr["HostIP"].ToString().Trim();
			}
			if (dr["DefultGetWay"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.DefultGetWay = dr["DefultGetWay"].ToString();
			}
			if (dr["CommunicationPass"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.CommunicationPass = dr["CommunicationPass"].ToString();
			}
			if (dr["DeviceNO"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.DeviceNo = (short)dr["DeviceNO"];
			}
			if (dr["Desc"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.Desc = dr["Desc"].ToString();
			}
			if (dr["Autorecieve"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.AutoRecieve = (bool)dr["Autorecieve"];
			}
			if (dr["MinCheck"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.MinCheck = (int)dr["MinCheck"];
			}
			deviceInfoBusiness.SyncTime = ((dr["SyncTime"].ToString() != DBNull.Value.ToString()) ? ((DateTime)dr["SyncTime"]) : DateTime.Now);
			if (dr["Model"].ToString() != DBNull.Value.ToString() && (short)dr["Model"] >= 0)
			{
				deviceInfoBusiness.Model = (DeviceModelEnumType)(short)dr["Model"];
			}
			if (dr["RdEnrollDataTime"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.RdEnrollDataTime = (DateTime)dr["RdEnrollDataTime"];
			}
			if (dr["AutoSetDateTime"].ToString() != DBNull.Value.ToString())
			{
				deviceInfoBusiness.AutoSetDateTime = (bool)dr["AutoSetDateTime"];
			}
			return deviceInfoBusiness;
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "LoadData", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\DeviceInfoPersistenceRepository.cs", 155);
			throw;
		}
	}
}
