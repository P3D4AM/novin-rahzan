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

internal class ContentBlackListPersistanceRepository : PersistenceRepository<ContentBlackListBusiness, ContentBlackList>, IContentBlackListRepository, IRepository<ContentBlackListBusiness>, INoGuidRepository<ContentBlackListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ContentBlackListBusiness, ContentBlackList>, IItemAsyncMapper<ContentBlackList, ContentBlackListBusiness>
	{
		public Task<ContentBlackList> MapAsync(string contextGuid, ContentBlackListBusiness source, ContentBlackList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ContentBlackList();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Words = source.Words;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs", 111);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ContentBlackListBusiness> MapAsync(string contextGuid, ContentBlackList source, ContentBlackListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ContentBlackListBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Words = source.Words;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs", 134);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ContentBlackList>> MapListAsync(string contextGuid, IEnumerable<ContentBlackListBusiness> source, List<ContentBlackList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ContentBlackListBusiness>> MapListAsync(string contextGuid, IEnumerable<ContentBlackList> source, List<ContentBlackListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ContentBlackList, ContentBlackListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ContentBlackListBusiness, ContentBlackList> MapToDatabase => Mapper;

	public override IItemMapper<ContentBlackList, ContentBlackListBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal ContentBlackListPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<ContentBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs", 26));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs"))
		{
			List<ContentBlackListBusiness> result;
			try
			{
				_ = 1;
				try
				{
					result = await MapFromDatabase.MapListAsync(ContextGuid, ((IEnumerable<ContentBlackList>)c.ContentBlackLists)?.ToList());
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs", 56);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\ContentBlackListPersistanceRepository.cs", 80);
			throw;
		}
	}
}
