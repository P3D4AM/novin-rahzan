using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Filters;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

public class MetaDataPersistenceRepository : PersistenceNoGuidRepository<MetaDataBusiness, MetaData>, IMetaDataRepository, INoGuidRepository<MetaDataBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<MetaDataBusiness, MetaData>, IItemAsyncMapper<MetaData, MetaDataBusiness>
	{
		public Task<MetaData> MapAsync(string contextGuid, MetaDataBusiness source, MetaData destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new MetaData();
				}
				destination.DataType = source.DataType;
				destination.EntityGuid = source.EntityGuid;
				destination.EntityType = source.EntityType;
				destination.Key = source.Key;
				destination.Value = source.Value;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 193);
				throw;
			}
		}

		public Task<MetaDataBusiness> MapAsync(string contextGuid, MetaData source, MetaDataBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new MetaDataBusiness(contextGuid);
				}
				destination.DataType = source.DataType;
				destination.EntityGuid = source.EntityGuid;
				destination.EntityType = source.EntityType;
				destination.Key = source.Key;
				destination.Value = source.Value;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 218);
				throw;
			}
		}

		public Task<List<MetaData>> MapListAsync(string contextGuid, IEnumerable<MetaDataBusiness> source, List<MetaData> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<MetaDataBusiness>> MapListAsync(string contextGuid, IEnumerable<MetaData> source, List<MetaDataBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<MetaData, MetaDataBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<MetaDataBusiness, MetaData> MapToDatabase => Mapper;

	internal MetaDataPersistenceRepository(ContextHolder context)
		: base(context)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 36);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(MetaDataBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 45));
				MetaData metaData = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(metaData, metaData.EntityGuid, metaData.EntityType, metaData.Key), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 47);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 51);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 52);
			}
			return ret;
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<MetaDataBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<MetaDataBusiness>> GetAllAsync(MetaDataFilter filter, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs"))
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 63));
			IEnumerable<MetaDataBusiness> ret = null;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					IQueryable<MetaData> queryable = (IQueryable<MetaData>)c.MetaData;
					if (filter != null)
					{
						if (filter.EntityGuid.HasValue && filter.EntityGuid != Guid.Empty)
						{
							queryable = queryable.Where((MetaData p) => p.EntityGuid == filter.EntityGuid);
						}
						if (filter.EntityType.HasValue)
						{
							queryable = queryable.Where((MetaData p) => (int?)p.EntityType == (int?)filter.EntityType);
						}
						if (!string.IsNullOrEmpty(filter.Key))
						{
							queryable = queryable.Where((MetaData p) => p.Key == filter.Key);
						}
						if (!string.IsNullOrEmpty(filter.Value))
						{
							queryable = queryable.Where((MetaData p) => p.Value == filter.Value);
						}
						if (filter.DataType.HasValue)
						{
							queryable = queryable.Where((MetaData p) => (int?)p.DataType == (int?)filter.DataType);
						}
					}
					List<MetaData> source = await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<MetaData>(queryable, token) : null);
					ret = await MapFromDatabase.MapListAsync(base.ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 88);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs");
				}
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid entityGuid, EntityPart part, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Global_ModelContext global_ModelContext = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 104));
				IQueryable<MetaData> queryable = ((IQueryable<MetaData>)global_ModelContext.MetaData).Where((MetaData x) => x.EntityGuid == entityGuid && x.EntityType == part);
				if (queryable == null || !queryable.Any())
				{
					return ret;
				}
				global_ModelContext.MetaData.RemoveRange((IEnumerable<MetaData>)queryable);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 111);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 112);
			}
			return ret;
		}
	}

	public async Task<List<Guid>> GetGuidListForFixAsync(CancellationToken token, string tranName)
	{
		Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetGuidListForFixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 118));
		List<Guid> result;
		try
		{
			_ = 1;
			try
			{
				List<MetaData> list = await EntityFrameworkQueryableExtensions.ToListAsync<MetaData>(((IQueryable<MetaData>)c.MetaData).Where((MetaData q) => q.EntityType == EntityPart.Customer && (q.Key == "1" || q.Key == "2" || q.Key == "3" || q.Key == "4") && !string.IsNullOrEmpty(q.Value)), token);
				result = ((list != null && list.Any()) ? (from q in list
					group q by q.EntityGuid into q
					select q.Key).ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetGuidListForFixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 129);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetGuidListForFixAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync<T>(Guid entityGuid, EntityPart part, List<T> data, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Global_ModelContext c = (Global_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 143));
			foreach (T datum in data)
			{
				MetaDataBusiness meta = datum as MetaDataBusiness;
				if (meta == null)
				{
					continue;
				}
				MetaData metaData = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<MetaData>((IQueryable<MetaData>)c.MetaData, (Expression<Func<MetaData, bool>>)((MetaData q) => q.EntityGuid == entityGuid && q.EntityType == part && q.Key == meta.Key), default(CancellationToken));
				if (string.IsNullOrEmpty(meta.Value))
				{
					if (metaData != null)
					{
						((DbContext)c).Remove<MetaData>(metaData);
					}
				}
				else if (metaData == null || !(metaData.Value == meta.Value))
				{
					if (metaData == null)
					{
						metaData = await MapToDatabase.MapAsync(base.ContextGuid, meta, null, tranName, token);
					}
					metaData.Value = meta.Value;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(metaData, metaData.EntityGuid, metaData.EntityType, metaData.Key), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 158);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 164);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Global\\MetaDataPersistenceRepository.cs", 165);
		}
		return res;
	}
}
