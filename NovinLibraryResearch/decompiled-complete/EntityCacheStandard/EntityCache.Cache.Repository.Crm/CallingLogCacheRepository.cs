using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using EntityCache.Business.Crm.Report;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using EntityCache.Mapping.BusinessMappers.Crm.Calling;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Crm;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Accounting.Crm;
using PacketParser.EventArgs;
using PacketParser.Filters.Crm;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Crm;

public class CallingLogCacheRepository : CacheRepositoryNoGuiId<CallingLogBusiness, string>, ICallingLogRepository, INoGuidRepository<CallingLogBusiness>, IDisposable, IItemAsyncMapper<MapperItemContainer<string>, MapperItemContainer<string>>
{
	private TimingCache<Guid, MapperItemContainer<string>> coGuid2UUId;

	private ICallingLogRepository dbRepository;

	public override IItemAsyncMapper<CallingLogBusiness, CallingLogBusiness> Mapper => CallingLogMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, CallingLogBusiness> TimingCache { get; set; }

	public override void Dispose()
	{
		try
		{
			coGuid2UUId = null;
			dbRepository = null;
			base.Dispose();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 42);
		}
	}

	public CallingLogCacheRepository(ICallingLogRepository dbRepository, int cachingTime)
		: base((INoGuidRepository<CallingLogBusiness>)dbRepository, cachingTime)
	{
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			coGuid2UUId = new TimingCache<Guid, MapperItemContainer<string>>(base.ContextGuid, this, 30);
			this.dbRepository = dbRepository;
			TimingCache = new TimingCache<string, CallingLogBusiness>(base.ContextGuid, Mapper, base.CachingTime);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 55);
			throw;
		}
	}

	public Task<CallingLogBusiness> GetAsync(string uuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(uuid, token, tranName);
	}

	public async Task<CallingLogBusiness> GetLast(Guid coGuid, CancellationToken token, string tranName = "")
	{
		_ = 3;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			if (coGuid2UUId.TryGetValue(coGuid, out var ret) && ret != null)
			{
				return await GetAsync(ret?.Value ?? "", token, tranName);
			}
			CallingLogBusiness item = await dbRepository.GetLast(coGuid, token, tranName);
			if (item != null)
			{
				coGuid2UUId.TryRemove(coGuid, out var _);
				await coGuid2UUId.UpdateTimeAsync(coGuid, new MapperItemContainer<string>
				{
					Value = item.Uuid
				}, tranName);
				await TimingCache.UpdateTimeAsync(item.Uuid, item, tranName);
			}
			return item;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLast", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 77);
			throw;
		}
	}

	public async Task<List<CallingLogBusiness>> GetAllNotSentAsync(int count, CancellationToken token, string tranName)
	{
		int line = 0;
		try
		{
			line = 1;
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			line = 2;
			List<CallingLogBusiness> items = await dbRepository.GetAllNotSentAsync(count, token, tranName);
			line = 3;
			if (items != null)
			{
				line = 4;
				foreach (CallingLogBusiness item in items)
				{
					line = 5;
					if (item != null)
					{
						line = 6;
						await TimingCache.UpdateTimeAsync(item.Uuid, item, tranName);
					}
				}
			}
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 108);
			throw;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CallingLogBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			if ((item.CoGuid ?? Guid.Empty) != Guid.Empty)
			{
				await coGuid2UUId.UpdateTimeAsync(item.CoGuid.Value, new MapperItemContainer<string>
				{
					Value = item.Uuid
				}, tranName);
			}
			return await dbRepository.SaveAsync(item, tryCount, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 123);
			throw;
		}
	}

	public Task<MapperItemContainer<string>> MapAsync(string contextGuid, MapperItemContainer<string> source, MapperItemContainer<string> destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.Value = source.Value;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 140);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<MapperItemContainer<string>>> MapListAsync(string contextGuid, IEnumerable<MapperItemContainer<string>> source, List<MapperItemContainer<string>> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<List<CallingLogBusiness>> GetLastRecordsByTime(int count, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			List<CallingLogBusiness> items = await dbRepository.GetLastRecordsByTime(count, token, tranName);
			if (items != null)
			{
				foreach (CallingLogBusiness item in items)
				{
					if (item != null)
					{
						await TimingCache.UpdateTimeAsync(item.Uuid, item, tranName);
					}
				}
			}
			else
			{
				items = new List<CallingLogBusiness>();
			}
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetLastRecordsByTime", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 165);
			throw;
		}
	}

	public async Task<List<CallingLogBusiness>> SearchAsync<TKey>(Expression<Func<ICallingLog, bool>> predicate, Expression<Func<ICallingLog, TKey>> orderby, int count, bool descending, CancellationToken token, string tranName)
	{
		_ = 1;
		try
		{
			if (base.IsDisposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
			List<CallingLogBusiness> items = await dbRepository.SearchAsync(predicate, orderby, count, descending, token, tranName);
			if (items != null)
			{
				foreach (CallingLogBusiness item in items)
				{
					if (item != null)
					{
						await TimingCache.UpdateTimeAsync(item.Uuid, item, tranName);
					}
				}
			}
			else
			{
				items = new List<CallingLogBusiness>();
			}
			return items;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SearchAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLogCacheRepository.cs", 192);
			return new List<CallingLogBusiness>();
		}
	}

	public Task<IEnumerable<CallingLogBusiness>> GetAllAsync(long number, string tafsilName, CancellationToken token, DateTime startDate, DateTime endDate, short maxCount, EnumNotification? type, string tranName, Guid? tafsilGuid)
	{
		return dbRepository.GetAllAsync(number, tafsilName, token, startDate, endDate, maxCount, type, tranName, tafsilGuid);
	}

	public Task<CallingLogBusiness> GetFromDatabaseAsync(string uuid, CancellationToken token, string tranName = "")
	{
		return dbRepository.GetFromDatabaseAsync(uuid, token, tranName);
	}

	public Task<IEnumerable<CallingLogReportBusiness>> GetAllAsync(CallingLogFilter filter, CancellationToken token, string tranName)
	{
		return dbRepository.GetAllAsync(filter, token, tranName);
	}

	public Task<CallingLogReportBusiness> GetReportAsync(string uuid, CancellationToken token, string tranName)
	{
		return dbRepository.GetReportAsync(uuid, token, tranName);
	}

	public new Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public new Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CallingLogBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public new Task<IEnumerable<CallingLogBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public new void ResetCache()
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> DeleteRangeAsync(List<Guid> eConnectionData, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> UpdateMessageStatusAsync(string uuid, string message, short statusId, CancellationToken token, string tranName)
	{
		return dbRepository.UpdateMessageStatusAsync(uuid, message, statusId, token, tranName);
	}

	public Task<List<CallTitelSuggest>> GetCallTitelsSuggestAsync(CancellationToken token, string tranName)
	{
		return dbRepository.GetCallTitelsSuggestAsync(token, tranName);
	}
}
