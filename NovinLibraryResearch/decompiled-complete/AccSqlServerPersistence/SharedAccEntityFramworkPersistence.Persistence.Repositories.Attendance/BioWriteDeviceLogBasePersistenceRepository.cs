using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Attendance;
using EntityCache.Core;
using EntityCache.Core.Repository.Attendance;
using PacketParser;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal abstract class BioWriteDeviceLogBasePersistenceRepository : PersistenceRepository<BioWriteDeviceLogBusiness, BioWriteDeviceLog>, IBioWriteDeviceLogRepository, IRepository<BioWriteDeviceLogBusiness>, INoGuidRepository<BioWriteDeviceLogBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BioWriteDeviceLog, BioWriteDeviceLogBusiness>, IItemAsyncMapper<BioWriteDeviceLogBusiness, BioWriteDeviceLog>
	{
		public Task<BioWriteDeviceLogBusiness> MapAsync(string contextGuid, BioWriteDeviceLog source, BioWriteDeviceLogBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BioWriteDeviceLogBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.HardwareName = source.HardwareName;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.Result = source.Result;
				destination.BioGuid = source.BioGuid;
				destination.UserGuid = source.UserGuid;
				if (!string.IsNullOrEmpty(source.BioInfo))
				{
					destination.BioInfo = source.BioInfo.FromJson<BiometricBusiness>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceLogBasePersistenceRepository.cs", 67);
				}
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceLogBasePersistenceRepository.cs", 72);
				throw;
			}
		}

		public Task<BioWriteDeviceLog> MapAsync(string contextGuid, BioWriteDeviceLogBusiness source, BioWriteDeviceLog destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BioWriteDeviceLog();
				}
				destination.Guid = source.Guid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.HardwareName = source.HardwareName;
				destination.Description = source.Description;
				destination.DateM = source.DateM;
				destination.Result = source.Result;
				destination.BioGuid = source.BioGuid;
				destination.UserGuid = source.UserGuid;
				if (source.BioInfo != null)
				{
					destination.BioInfo = Json.ToStringJson(source.BioInfo);
				}
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceLogBasePersistenceRepository.cs", 101);
				throw;
			}
		}

		public Task<List<BioWriteDeviceLogBusiness>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceLog> source, List<BioWriteDeviceLogBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BioWriteDeviceLog>> MapListAsync(string contextGuid, IEnumerable<BioWriteDeviceLogBusiness> source, List<BioWriteDeviceLog> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<BioWriteDeviceLog, BioWriteDeviceLogBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<BioWriteDeviceLog, BioWriteDeviceLogBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BioWriteDeviceLogBusiness, BioWriteDeviceLog> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public BioWriteDeviceLogBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioWriteDeviceLogBasePersistenceRepository.cs", 40);
		}
	}

	public virtual Task<IEnumerable<BioWriteDeviceLogModel>> GetAllAsync(BioWriteDeviceLogFilter filter, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
