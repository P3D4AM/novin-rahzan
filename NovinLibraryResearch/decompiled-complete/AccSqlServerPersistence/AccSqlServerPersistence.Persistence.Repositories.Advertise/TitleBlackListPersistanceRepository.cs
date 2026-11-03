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
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Advertise;

internal class TitleBlackListPersistanceRepository : PersistenceRepository<TitleBlackListBusiness, TitleBlackList>, ITitleBlackListRepository, IRepository<TitleBlackListBusiness>, INoGuidRepository<TitleBlackListBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<TitleBlackListBusiness, TitleBlackList>, IItemAsyncMapper<TitleBlackList, TitleBlackListBusiness>
	{
		public Task<TitleBlackList> MapAsync(string contextGuid, TitleBlackListBusiness source, TitleBlackList destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs"))
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
						destination = new TitleBlackList();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Words = source.Words;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs", 99);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<TitleBlackListBusiness> MapAsync(string contextGuid, TitleBlackList source, TitleBlackListBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs"))
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
						destination = new TitleBlackListBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Words = source.Words;
				}
				catch (Exception ex)
				{
					WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs", 123);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<TitleBlackList>> MapListAsync(string contextGuid, IEnumerable<TitleBlackListBusiness> source, List<TitleBlackList> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<TitleBlackListBusiness>> MapListAsync(string contextGuid, IEnumerable<TitleBlackList> source, List<TitleBlackListBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<TitleBlackList, TitleBlackListBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<TitleBlackListBusiness, TitleBlackList> MapToDatabase => Mapper;

	public override IItemMapper<TitleBlackList, TitleBlackListBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public TitleBlackListPersistanceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<TitleBlackListBusiness>> GetAllAsync(string search, CancellationToken token, bool? status, string tranName)
	{
		Adv_ModelContext c = (Adv_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs", 25));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs"))
		{
			List<TitleBlackListBusiness> result;
			try
			{
				_ = 1;
				try
				{
					List<string> list = search?.FixString()?.SplitString();
					if (token.IsCancellationRequested)
					{
						result = null;
					}
					else
					{
						IQueryable<TitleBlackList> source = (IQueryable<TitleBlackList>)c.TitleBlackLists;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								source = source.Where((TitleBlackList p) => DbFunctionsExtensions.Like(EF.Functions, p.Words, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							source = source.Where((TitleBlackList p) => p.Status == ((bool?)status).Value);
						}
						result = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, source.ToList())) : null);
					}
				}
				catch (OperationCanceledException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs", 44);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccAdvertiseSqlServerPersistence\\Advertise\\TitleBlackListPersistanceRepository.cs", 67);
			throw;
		}
	}
}
