using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class GpsPersistenceRepository : PersistenceSyncRepository<GPSBusiness, Gps>, IGpsRepository, ISyncRepository<GPSBusiness>, IEditableDelivery, IRepository<GPSBusiness>, INoGuidRepository<GPSBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<GPSBusiness, Gps>, IItemAsyncMapper<Gps, GPSBusiness>
	{
		public Task<Gps> MapAsync(string contextGuid, GPSBusiness source, Gps destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs"))
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
						destination = new Gps();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Accuracy = source.Accuracy;
					destination.Activity = source.Activity;
					destination.Altitude = source.Altitude;
					destination.Bearing = source.Bearing;
					destination.ColorHex = source.ColorHex;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Distance = source.Distance;
					destination.FromLastPointDistance = source.FromLastPointDistance;
					destination.FromStartPointDistance = source.FromStartPointDistance;
					destination.GPSTime = source.GPSTime;
					destination.Icon = source.Icon;
					destination.Latitude = source.Latitude;
					destination.Longitude = source.Longitude;
					destination.PointDurationSeconds = source.PointDurationSeconds;
					destination.PreviewsID = source.PreviewsID;
					destination.Speed = source.Speed;
					destination.Time = source.Time;
					destination.UId = source.UId;
					destination.UserAccGuid = source.UserAccGuid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 154);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<GPSBusiness> MapAsync(string contextGuid, Gps source, GPSBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs"))
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
						destination = new GPSBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Accuracy = source.Accuracy;
					destination.Activity = source.Activity;
					destination.Altitude = source.Altitude;
					destination.Bearing = source.Bearing;
					destination.ColorHex = source.ColorHex;
					destination.DateM = source.DateM;
					destination.Description = source.Description;
					destination.Distance = source.Distance;
					destination.FromLastPointDistance = source.FromLastPointDistance;
					destination.FromStartPointDistance = source.FromStartPointDistance;
					destination.GPSTime = source.GPSTime;
					destination.Icon = source.Icon;
					destination.Latitude = source.Latitude;
					destination.Longitude = source.Longitude;
					destination.PointDurationSeconds = source.PointDurationSeconds;
					destination.PreviewsID = source.PreviewsID;
					destination.Speed = source.Speed;
					destination.Time = source.Time;
					destination.UId = source.UId;
					destination.UserAccGuid = source.UserAccGuid;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 205);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<Gps>> MapListAsync(string contextGuid, IEnumerable<GPSBusiness> source, List<Gps> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<GPSBusiness>> MapListAsync(string contextGuid, IEnumerable<Gps> source, List<GPSBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<Gps, GPSBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<GPSBusiness, Gps> MapToDatabase => Mapper;

	public override IItemMapper<Gps, GPSBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public GpsPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<GPSBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs"))
		{
			List<GPSBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<Gps> source = ((IQueryable<Gps>)c.Gps)?.Take(count)?.ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (ex2.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 35);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				await ((Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 51))).ExecuteNoneQueryAsync("delete from Acc_Gps where Guid='" + guid.ToString().ToUpper() + "'", null, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 57);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 58);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAllAsync(IEnumerable<Guid> res, short MaxTryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs"))
		{
			_ = 1;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 71));
				foreach (Guid re in res)
				{
					await dbcontext.ExecuteNoneQueryAsync("delete from Acc_Gps where Guid='" + re.ToString().ToUpper() + "'", null, tranName, token, "", CommandType.StoredProcedure, 30, "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs");
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 78);
				ret.AddReturnedValue(ex, "", "RemoveAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 79);
			}
			return ret;
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\GpsPersistenceRepository.cs", 97);
		}
	}
}
