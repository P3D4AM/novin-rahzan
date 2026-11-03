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
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class HyperLinkPersistenceRepository : PersistenceRepository<HyperLinkBusiness, HyperLink>, IHyperLinkRepository, IRepository<HyperLinkBusiness>, INoGuidRepository<HyperLinkBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<HyperLinkBusiness, HyperLink>, IItemAsyncMapper<HyperLink, HyperLinkBusiness>
	{
		public Task<HyperLink> MapAsync(string contextGuid, HyperLinkBusiness source, HyperLink destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs"))
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
						destination = new HyperLink();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Category = source.Category;
					destination.Enable = source.Enable;
					destination.Icon = source.Icon;
					destination.Order = source.Order;
					destination.ParentGuid = source.ParentGuid;
					destination.Style = source.Style;
					destination.Target = source.Target;
					destination.Title = source.Title;
					destination.Url = source.Url;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs", 98);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<HyperLinkBusiness> MapAsync(string contextGuid, HyperLink source, HyperLinkBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs"))
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
						destination = new HyperLinkBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Category = source.Category;
					destination.Enable = source.Enable;
					destination.Icon = source.Icon;
					destination.Order = source.Order;
					destination.ParentGuid = source.ParentGuid;
					destination.Style = source.Style;
					destination.Target = source.Target;
					destination.Title = source.Title;
					destination.Url = source.Url;
					destination.Status = source.Status;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs", 133);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<HyperLink>> MapListAsync(string contextGuid, IEnumerable<HyperLinkBusiness> source, List<HyperLink> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<HyperLinkBusiness>> MapListAsync(string contextGuid, IEnumerable<HyperLink> source, List<HyperLinkBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<HyperLink, HyperLinkBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<HyperLinkBusiness, HyperLink> MapToDatabase => Mapper;

	public override IItemMapper<HyperLink, HyperLinkBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal HyperLinkPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<IEnumerable<HyperLinkBusiness>> GetByCategoryAsync(string category, int top, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs"))
		{
			IEnumerable<HyperLinkBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<HyperLink> queryable = ((IQueryable<HyperLink>)c.HyperLink)?.Where((HyperLink w) => w.Category == category && w.Enable);
					if (status.HasValue)
					{
						queryable = queryable?.Where((HyperLink q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<HyperLink, HyperLinkBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<HyperLink>(queryable.Take(top), token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs", 37);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\HyperLinkPersistenceRepository.cs", 57);
		}
	}
}
