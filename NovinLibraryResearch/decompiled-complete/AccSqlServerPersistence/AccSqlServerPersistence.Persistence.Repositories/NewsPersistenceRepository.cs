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

internal class NewsPersistenceRepository : PersistenceRepository<NewsBusiness, News>, INewsRepository, IRepository<NewsBusiness>, INoGuidRepository<NewsBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<NewsBusiness, News>, IItemAsyncMapper<News, NewsBusiness>
	{
		public Task<News> MapAsync(string contextGuid, NewsBusiness source, News destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
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
						destination = new News();
					}
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.Category = source.Category;
					destination.DateM = source.DateM;
					destination.FullContent = source.FullContent;
					destination.KeyWords = source.KeyWords;
					destination.MetaDescription = source.MetaDescription;
					destination.PicUrl = source.PicUrl;
					destination.QuickText = source.QuickText;
					destination.Title = source.Title;
					destination.Type = source.Type;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 186);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<NewsBusiness> MapAsync(string contextGuid, News source, NewsBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new NewsBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Category = source.Category;
				destination.DateM = source.DateM;
				destination.FullContent = source.FullContent;
				destination.KeyWords = source.KeyWords;
				destination.MetaDescription = source.MetaDescription;
				destination.PicUrl = source.PicUrl;
				destination.QuickText = source.QuickText;
				destination.Title = source.Title;
				destination.Type = source.Type;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 220);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<News>> MapListAsync(string contextGuid, IEnumerable<NewsBusiness> source, List<News> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<NewsBusiness>> MapListAsync(string contextGuid, IEnumerable<News> source, List<NewsBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<News, NewsBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<NewsBusiness, News> MapToDatabase => Mapper;

	public override IItemMapper<News, NewsBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	internal NewsPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<List<NewsBusiness>> GetTopAsync(NewsType type, short top, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetTopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 24));
		using (new JaegerMonitor("GetTopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
		{
			List<NewsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<News> news = c.News;
					object obj;
					if (news == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<News> queryable = ((IQueryable<News>)news).Where((News w) => (int)w.Type == (int)type);
						obj = ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<News>(queryable, token) : null);
					}
					List<News> source = await (Task<List<News>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetTopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 35);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetTopAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<NewsBusiness>> GetAllByTypeCategoryAsync(NewsType type, string category, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByTypeCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 45));
		using (new JaegerMonitor("GetAllByTypeCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
		{
			IEnumerable<NewsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<News> queryable = ((IQueryable<News>)c.News)?.Where((News w) => (int)w.Type == (int)type && w.Category == category);
					if (status.HasValue)
					{
						queryable = queryable?.Where((News q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<News, NewsBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<News>((IQueryable<News>)queryable.OrderByDescending((News p) => p.DateM), token) : null), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByTypeCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 58);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByTypeCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<NewsBusiness>> GetByCategoryAsync(string category, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 68));
		using (new JaegerMonitor("GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
		{
			IEnumerable<NewsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<News> queryable = ((IQueryable<News>)c.News)?.Where((News w) => w.Category == category);
					if (status.HasValue)
					{
						queryable = queryable.Where((News q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<News, NewsBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<News>((IQueryable<News>)queryable.OrderByDescending((News p) => p.DateM), token) : null), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 81);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetByCategoryAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	internal async Task<List<NewsBusiness>> GetRelatedNewsAsync(object id, short top, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetRelatedNewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 91));
		using (new JaegerMonitor("GetRelatedNewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
		{
			List<NewsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					DbSet<News> news = c.News;
					object obj;
					if (news == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<News> queryable = ((IQueryable<News>)news).OrderByDescending((News x) => x.DateM).Take(5);
						obj = ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<News>(queryable, token) : null);
					}
					List<News> source = await (Task<List<News>>)obj;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetRelatedNewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 102);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetRelatedNewsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<NewsBusiness>> GetAllByTypeAsync(NewsType type, bool? status, CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 112));
		using (new JaegerMonitor("GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs"))
		{
			IEnumerable<NewsBusiness> result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<News> queryable = ((IQueryable<News>)c.News)?.Where((News x) => (int)x.Type == (int)type);
					if (status.HasValue)
					{
						queryable = queryable?.Where((News q) => (bool?)q.Status == status);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IItemAsyncMapper<News, NewsBusiness> mapFromDatabase = MapFromDatabase;
						string contextGuid = ContextGuid;
						result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<News>(queryable, token), null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 125);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllByTypeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs");
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\NewsPersistenceRepository.cs", 145);
		}
	}
}
