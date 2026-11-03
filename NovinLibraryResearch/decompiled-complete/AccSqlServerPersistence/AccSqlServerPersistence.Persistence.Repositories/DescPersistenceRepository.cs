using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class DescPersistenceRepository : PersistenceSyncRepository<DescBusiness, Desc>, IDescRepository, ISyncRepository<DescBusiness>, IEditableDelivery, IRepository<DescBusiness>, INoGuidRepository<DescBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DescBusiness, Desc>, IItemAsyncMapper<Desc, DescBusiness>
	{
		public Task<Desc> MapAsync(string contextGuid, DescBusiness source, Desc destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs"))
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
						destination = new Desc();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Section = source.Section;
					destination.Description = source.Description;
					destination.SourceType = source.SourceType;
					destination.Source = source.Source;
					destination.DestinationType = source.DestinationType;
					destination.Destination = source.Destination;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.Status = source.Status;
					destination.Modified = source.Modified;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 135);
					throw;
				}
			}
		}

		public Task<DescBusiness> MapAsync(string contextGuid, Desc source, DescBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs"))
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
						destination = new DescBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Section = source.Section;
					destination.Description = source.Description;
					destination.SourceType = source.SourceType;
					destination.Source = source.Source;
					destination.DestinationType = source.DestinationType;
					destination.Destination = source.Destination;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.Status = source.Status;
					return Task.FromResult(destination);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 172);
					throw;
				}
			}
		}

		public Task<List<Desc>> MapListAsync(string contextGuid, IEnumerable<DescBusiness> source, List<Desc> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DescBusiness>> MapListAsync(string contextGuid, IEnumerable<Desc> source, List<DescBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<Desc, DescBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DescBusiness, Desc> MapToDatabase => Mapper;

	public override IItemMapper<Desc, DescBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal DescPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<DescBusiness>> GetAllAsync(DescSection section, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 32));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs"))
		{
			IEnumerable<DescBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<Desc> queryable = ((IQueryable<Desc>)c.Desc)?.Where((Desc x) => x.Section == section);
					if (status.HasValue)
					{
						queryable = queryable?.Where((Desc q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<Desc, DescBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<Desc>(queryable, token) : null), null, tranName, token);
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 45);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<DescBusiness>> GetAllAsync(string search, DescSection section, CancellationToken token, bool? status, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 56));
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs"))
		{
			IEnumerable<DescBusiness> result;
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
						IQueryable<Desc> queryable = (IQueryable<Desc>)c.Desc;
						foreach (string word in list)
						{
							if (!string.IsNullOrEmpty(word))
							{
								queryable = queryable.Where((Desc p) => DbFunctionsExtensions.Like(EF.Functions, p.Description, $"%{word}%"));
							}
						}
						if (status.HasValue)
						{
							queryable = queryable.Where((Desc p) => (bool?)p.Status == status);
						}
						if (section != 0)
						{
							queryable = queryable.Where((Desc p) => p.Section == section);
						}
						List<DescBusiness> list2 = ((!token.IsCancellationRequested) ? (await MapFromDatabase.MapListAsync(ContextGuid, queryable?.ToList(), null, tranName, token)) : null);
						result = list2;
					}
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 77);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override Task<ReturnedSaveFuncInfo> RemoveAsync(Guid guid, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> DeleteRangeAsync(IEnumerable<Guid> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\DescCachePersistenceRepository.cs", 97);
		}
	}
}
