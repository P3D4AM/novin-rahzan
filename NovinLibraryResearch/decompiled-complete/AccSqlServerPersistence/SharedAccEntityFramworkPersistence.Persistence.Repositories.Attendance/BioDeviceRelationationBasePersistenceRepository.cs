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
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Filters.Attendance;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Attendance;

internal abstract class BioDeviceRelationationBasePersistenceRepository : PersistenceNoGuidRepository<BioDeviceRelationBusiness, BioDeviceRelation>, IBioDeviceRelationRepository, INoGuidRepository<BioDeviceRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<BioDeviceRelation, BioDeviceRelationBusiness>, IItemAsyncMapper<BioDeviceRelationBusiness, BioDeviceRelation>
	{
		public Task<BioDeviceRelationBusiness> MapAsync(string contextGuid, BioDeviceRelation source, BioDeviceRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BioDeviceRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.BioGuid = source.BioGuid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.Privilage = source.Privilage;
				destination.FromDateM = source.FromDateM;
				destination.ToDateM = source.ToDateM;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 217);
				throw;
			}
		}

		public Task<BioDeviceRelation> MapAsync(string contextGuid, BioDeviceRelationBusiness source, BioDeviceRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new BioDeviceRelation();
				}
				destination.BioGuid = source.BioGuid;
				destination.DeviceGuid = source.DeviceGuid;
				destination.HardwareCode = source.HardwareCode;
				destination.Privilage = source.Privilage;
				destination.FromDateM = source.FromDateM;
				destination.ToDateM = source.ToDateM;
				destination.Status = source.Status;
				destination.Modified = source.Modified;
				return Task.FromResult(destination);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 243);
				throw;
			}
		}

		public Task<List<BioDeviceRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<BioDeviceRelation> source, List<BioDeviceRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<BioDeviceRelation>> MapListAsync(string contextGuid, IEnumerable<BioDeviceRelationBusiness> source, List<BioDeviceRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<BioDeviceRelation, BioDeviceRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<BioDeviceRelationBusiness, BioDeviceRelation> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public BioDeviceRelationationBasePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 40);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(BioDeviceRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 49));
				BioDeviceRelation bioDeviceRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(bioDeviceRelation, bioDeviceRelation.BioGuid, bioDeviceRelation.DeviceGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 51);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 55);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 56);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<BioDeviceRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs"))
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
				foreach (BioDeviceRelationBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 69);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 73);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 74);
			}
			return ret;
		}
	}

	public async Task<BioDeviceRelationBusiness> GetAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 80));
		if (bioGuid == Guid.Empty || deviceGuid == Guid.Empty)
		{
			throw new ArgumentException();
		}
		BioDeviceRelationBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				BioDeviceRelation bioDeviceRelation = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<BioDeviceRelation>((IQueryable<BioDeviceRelation>)c.BioDeviceRelations, (Expression<Func<BioDeviceRelation, bool>>)((BioDeviceRelation q) => q.BioGuid == bioGuid && q.DeviceGuid == deviceGuid), token);
				result = ((bioDeviceRelation != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, bioDeviceRelation, null, tranName, token)) : null);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 90);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<BioDeviceRelationBusiness> GetAsync(Guid deviceGuid, int hardwareCode, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 101));
		if (hardwareCode <= 0 || deviceGuid == Guid.Empty)
		{
			throw new ArgumentException();
		}
		BioDeviceRelationBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				BioDeviceRelation bioDeviceRelation = (await EntityFrameworkQueryableExtensions.ToListAsync<BioDeviceRelation>(((IQueryable<BioDeviceRelation>)EntityFrameworkQueryableExtensions.Include<BioDeviceRelation, Biometric>((IQueryable<BioDeviceRelation>)c.BioDeviceRelations, (Expression<Func<BioDeviceRelation, Biometric>>)((BioDeviceRelation q) => q.BioItem))).Where((BioDeviceRelation q) => q.BioItem.PersonGuid != null && q.BioItem.PersonGuid != Guid.Empty), default(CancellationToken)))?.SingleOrDefault((BioDeviceRelation q) => q.HardwareCode == hardwareCode && q.DeviceGuid == deviceGuid);
				result = ((bioDeviceRelation != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, bioDeviceRelation, null, tranName, token)) : null);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 112);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<BioDeviceRelationBusiness>> GetAllAsync(BioDeviceRelationFilter filter, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 123));
		IEnumerable<BioDeviceRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<BioDeviceRelation> queryable = (IQueryable<BioDeviceRelation>)c.BioDeviceRelations;
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if (filter == null)
					{
						goto IL_028b;
					}
					if ((filter.DeviceGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((BioDeviceRelation q) => q.DeviceGuid == filter.DeviceGuid.Value);
					}
					if (queryable != null && queryable.Any())
					{
						goto IL_028b;
					}
					result = null;
				}
				goto end_IL_00de;
				IL_028b:
				result = await MapFromDatabase.MapListAsync(base.ContextGuid, queryable, null, tranName, token);
				end_IL_00de:;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 141);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<BioDeviceRelationBusiness> GetByPersonAsync(Guid deviceGuid, Guid personGuid, CancellationToken token, string tranName)
	{
		Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByPersonAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 152));
		if (personGuid == Guid.Empty || deviceGuid == Guid.Empty)
		{
			throw new ArgumentException();
		}
		BioDeviceRelationBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				BioDeviceRelation bioDeviceRelation = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<BioDeviceRelation>((IQueryable<BioDeviceRelation>)EntityFrameworkQueryableExtensions.Include<BioDeviceRelation, Biometric>((IQueryable<BioDeviceRelation>)c.BioDeviceRelations, (Expression<Func<BioDeviceRelation, Biometric>>)((BioDeviceRelation q) => q.BioItem)), (Expression<Func<BioDeviceRelation, bool>>)((BioDeviceRelation q) => q.DeviceGuid == deviceGuid && q.BioItem.PersonGuid == personGuid), token);
				result = ((bioDeviceRelation != null) ? (await MapFromDatabase.MapAsync(base.ContextGuid, bioDeviceRelation, null, tranName, token)) : null);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetByPersonAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 162);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetByPersonAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid bioGuid, Guid deviceGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Att_ModelContext c = (Att_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 178));
				BioDeviceRelation bioDeviceRelation = await c.BioDeviceRelations.FindAsync(new object[2] { bioGuid, deviceGuid });
				if (bioDeviceRelation == null)
				{
					return res;
				}
				c.BioDeviceRelations.Remove(bioDeviceRelation);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 185);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Attendance\\Biometric\\BioDeviceRelationBasePersistenceRepository.cs", 186);
			}
			return res;
		}
	}

	public virtual Task<IEnumerable<BioDeviceRelationModel>> GetAllAsync(Guid deviceGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}
}
