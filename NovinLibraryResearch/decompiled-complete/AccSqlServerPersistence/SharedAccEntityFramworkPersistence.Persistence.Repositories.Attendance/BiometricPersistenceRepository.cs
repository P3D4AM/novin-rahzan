using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal class BiometricPersistenceRepository : PersistenceRepository<BiometricBusiness, Biometric>, IBiometricRepository, IRepository<BiometricBusiness>, INoGuidRepository<BiometricBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Biometric, BiometricBusiness>, IItemAsyncMapper<BiometricBusiness, Biometric>
	{
		public Task<BiometricBusiness> MapAsync(string contextGuid, Biometric source, BiometricBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BiometricBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DeviceGroup = source.DeviceGroup;
				destination.PersonGuid = source.PersonGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 145);
				throw;
			}
		}

		public Task<Biometric> MapAsync(string contextGuid, BiometricBusiness source, Biometric destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Biometric();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.DeviceGroup = source.DeviceGroup;
				destination.PersonGuid = source.PersonGuid;
				destination.ServerStatus = source.ServerStatus;
				destination.FingerPrintCount = source.FingerPrintCount;
				destination.HasPalm = source.HasPalm;
				destination.HasFace = source.HasFace;
				destination.HasCard = source.HasCard;
				destination.HasPassword = source.HasPassword;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 178);
				throw;
			}
		}

		public Task<List<BiometricBusiness>> MapListAsync(string contextGuid, IEnumerable<Biometric> source, List<BiometricBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Biometric>> MapListAsync(string contextGuid, IEnumerable<BiometricBusiness> source, List<Biometric> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private BiometricDetailPersistenceRepository _detRep;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Biometric, BiometricBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Biometric, BiometricBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BiometricBusiness, Biometric> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public BiometricPersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRep = new BiometricDetailPersistenceRepository(db);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 42);
		}
	}

	public override async Task<BiometricBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 46));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs"))
		{
			BiometricBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					Biometric biometric = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Biometric>((IQueryable<Biometric>)c.Biometrics, (Expression<Func<Biometric, bool>>)((Biometric x) => x.Guid == guid), token);
					if (biometric == null)
					{
						result = null;
					}
					else
					{
						BiometricBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, biometric, null, tranName, token);
						await _detRep.GetAllAsync(ret, token, tranName);
						result = ret;
					}
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 61);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BiometricBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs"))
		{
			_ = 4;
			try
			{
				Att_ModelContext dbcontext = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 77));
				if (item.Guid == Guid.Empty)
				{
					Guid.NewGuid();
				}
				Biometric biometric = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(biometric, biometric.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 80);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRep.RemoveAsync(item.Guid, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 82);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRep.SaveAsync(item?.DetailList.ToList(), item.Guid, 0, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 83);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 87);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 88);
			}
			return res;
		}
	}

	public async Task<BiometricBusiness> GetAsync(Guid personGuid, EnDeviceGroup deviceGroup, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 94));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs"))
		{
			BiometricBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					Biometric biometric = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<Biometric>((IQueryable<Biometric>)c.Biometrics, (Expression<Func<Biometric, bool>>)((Biometric x) => x.PersonGuid == personGuid && x.DeviceGroup == deviceGroup), token);
					result = ((biometric != null) ? (await MapFromDatabase.MapAsync(ContextGuid, biometric, null, tranName, token)) : null);
				}
				catch (ThreadAbortException)
				{
					throw;
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex3)
				{
					if (ex3.Message.Contains("Operation cancelled by user."))
					{
						throw new OperationCanceledException();
					}
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs", 108);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BiometricPersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
