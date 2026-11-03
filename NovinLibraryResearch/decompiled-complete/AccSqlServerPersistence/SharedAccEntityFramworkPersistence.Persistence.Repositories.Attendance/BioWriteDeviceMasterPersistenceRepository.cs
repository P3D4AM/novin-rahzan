using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal abstract class BioWriteDeviceMasterPersistenceRepository : PersistenceNoGuidRepository<BioWriteDeviceBusiness, BioWriteDevice>, IBioWriteDeviceRepository, INoGuidRepository<BioWriteDeviceBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BioWriteDevice, BioWriteDeviceBusiness>, IItemAsyncMapper<BioWriteDeviceBusiness, BioWriteDevice>
	{
		public Task<BioWriteDeviceBusiness> MapAsync(string contextGuid, BioWriteDevice source, BioWriteDeviceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new ArgumentNullException("Please Pass a New Object");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				destination.ContextGuid = contextGuid;
				destination.BioGuid = source.BioGuid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.HarwareName = source.HarwareName;
				destination.InsertedDateM = source.InsertedDateM;
				destination.TryCount = source.TryCount;
				destination.NextTry = source.NextTry;
				destination.UserGuid = source.UserGuid;
				destination.MessageType = source.MessageType;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 139);
				throw;
			}
		}

		public Task<BioWriteDevice> MapAsync(string contextGuid, BioWriteDeviceBusiness source, BioWriteDevice destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			try
			{
				if (source == null)
				{
					destination = null;
					return Task.FromResult(destination);
				}
				if (destination == null)
				{
					destination = new BioWriteDevice();
				}
				destination.BioGuid = source.BioGuid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.HarwareName = source.HarwareName;
				destination.InsertedDateM = source.InsertedDateM;
				destination.TryCount = source.TryCount;
				destination.NextTry = source.NextTry;
				destination.UserGuid = source.UserGuid;
				destination.MessageType = source.MessageType;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 166);
				throw;
			}
		}

		public Task<List<BioWriteDeviceBusiness>> MapListAsync(string contextGuid, IEnumerable<BioWriteDevice> source, List<BioWriteDeviceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BioWriteDevice>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceBusiness> source, List<BioWriteDevice> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BioWriteDevice, BioWriteDeviceBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BioWriteDeviceBusiness, BioWriteDevice> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public BioWriteDeviceMasterPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 41);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BioWriteDeviceBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 50));
				BioWriteDevice bioWriteDevice = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(bioWriteDevice, bioWriteDevice.BioGuid, bioWriteDevice.DeviceGuid, bioWriteDevice.HardwareCode), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 52);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 56);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 57);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<BioWriteDeviceBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				if (!(items?.Any() ?? false))
				{
					return ret;
				}
				foreach (BioWriteDeviceBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 70);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 74);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 75);
			}
			return ret;
		}
	}

	public virtual Task<BioWriteDeviceBusiness> FetchNextForSendAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<BioWriteDeviceCompositeKey> item, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Att_ModelContext context = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 87));
				IQueryable<BioWriteDevice> queryable = ((IQueryable<BioWriteDevice>)context.BioWriteDevices).Where((BioWriteDevice x) => x.BioGuid == item.Key.BioGuid && x.DeviceGuid == item.Key.DeviceGuid && x.MessageType == item.Key.MessageType);
				BioWriteDevice data = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<BioWriteDevice>(queryable, token) : null);
				if (data == null)
				{
					return res;
				}
				IItemAsyncMapper<BioWriteDevice, BioWriteDeviceBusiness> mapFromDatabase = MapFromDatabase;
				string contextGuid = base.ContextGuid;
				BioWriteDevice source = data;
				BioWriteDeviceBusiness data2 = await mapFromDatabase.MapAsync(contextGuid, source, await BioWriteDeviceBusiness.GenerateNewAsync(base.ContextGuid, data.InsertedDateM, data.MessageType), tranName, token);
				if (item.SaveResultStatus)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BioWriteDeviceLogBusiness.GenerateAndSaveLogAsync(base.ContextGuid, data2, ReturnedState.Success, "", token, tranName), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 93);
					((DbContext)context).Remove<BioWriteDevice>(data);
				}
				else
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await BioWriteDeviceLogBusiness.GenerateAndSaveLogAsync(base.ContextGuid, data2, ReturnedState.Error, item.Message, token, tranName), ReturnedState.None, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 98);
					data.Message = item.Message;
					((DbContext)context).Update<BioWriteDevice>(data);
				}
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 103);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 106);
				res.AddReturnedValue(ex2, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceMasterPersistenceRepository.cs", 107);
			}
			return res;
		}
	}

	public virtual Task<IEnumerable<BioWriteDeviceModel>> GetAllAync(string contextGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
