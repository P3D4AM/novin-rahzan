using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Advertise;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Advertise;
using EntityCache.Core;
using EntityCache.Core.Repository.Advertise;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class AdvGroupPersistenceRepository : PersistenceRepository<AdvGroupBusiness, AdvGroup>, IAdvGroupRepository, IRepository<AdvGroupBusiness>, INoGuidRepository<AdvGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<AdvGroupBusiness, AdvGroup>, IItemAsyncMapper<AdvGroup, AdvGroupBusiness>
	{
		public Task<AdvGroup> MapAsync(string contextGuid, AdvGroupBusiness source, AdvGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvGroup();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.GroupName = source.GroupName;
				destination.Status = source.Status;
				destination.ParentGuid = source.ParentGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 156);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<AdvGroupBusiness> MapAsync(string contextGuid, AdvGroup source, AdvGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new AdvGroupBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.GroupName = source.GroupName;
				destination.Status = source.Status;
				destination.ParentGuid = source.ParentGuid;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 181);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<AdvGroup>> MapListAsync(string contextGuid, IEnumerable<AdvGroupBusiness> source, List<AdvGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<AdvGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<AdvGroup> source, List<AdvGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<AdvGroup, AdvGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<AdvGroupBusiness, AdvGroup> MapToDatabase => Mapper;

	public override IItemMapper<AdvGroup, AdvGroupBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public AdvGroupPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<bool> CheckNameAsync(string name, Guid guid, string tranName)
	{
		Adv_ModelContext adv_ModelContext = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 22));
		using (new JaegerMonitor("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs"))
		{
			bool result;
			try
			{
				AdvGroup advGroup = ((IQueryable<AdvGroup>)adv_ModelContext.AdvGroup).FirstOrDefault((AdvGroup p) => p.GroupName == name && p.Guid != guid);
				result = advGroup == null;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 32);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await adv_ModelContext.DisposeAsync("CheckNameAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AdvGroupBusiness> GetAsyncByParent(Guid parentGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsyncByParent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 45));
		using (new JaegerMonitor("GetAsyncByParent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs"))
		{
			AdvGroupBusiness result;
			try
			{
				_ = 1;
				try
				{
					AdvGroup source = ((IQueryable<AdvGroup>)c.AdvGroup).FirstOrDefault((AdvGroup p) => p.Guid == parentGuid);
					result = await MapFromDatabase.MapAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsyncByParent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 55);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsyncByParent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<List<AdvGroupBusiness>> GetAllAsync(Guid parentGuid, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 67));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs"))
		{
			List<AdvGroupBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<AdvGroup> source = ((IQueryable<AdvGroup>)c.AdvGroup).Where((AdvGroup p) => p.ParentGuid == parentGuid)?.ToList();
					result = await MapFromDatabase.MapListAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 77);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<AdvGroupBusiness> GetAsync(string name, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 89));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs"))
		{
			AdvGroupBusiness result;
			try
			{
				_ = 1;
				try
				{
					AdvGroup source = ((IQueryable<AdvGroup>)c.AdvGroup).FirstOrDefault((AdvGroup p) => p.GroupName == name.Trim());
					result = await MapFromDatabase.MapAsync(ContextGuid, source);
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 99);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs");
				}
			}
			return result;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\AdvGroupPersistenceRepository.cs", 123);
			throw;
		}
	}
}
