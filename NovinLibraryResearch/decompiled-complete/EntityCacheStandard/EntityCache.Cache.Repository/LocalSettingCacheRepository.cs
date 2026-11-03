using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class LocalSettingCacheRepository : CacheRepositoryNoGuiId<LocalSettingBusiness, string>, ILocalSettingRepository, INoGuidRepository<LocalSettingBusiness>, IDisposable
{
	private bool _isDisposed;

	private ILocalSettingRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, LocalSettingBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<LocalSettingBusiness, LocalSettingBusiness> Mapper => LocalSettingMapper.Instance;

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				dbRepository = null;
				TimingCache?.Dispose();
				TimingCache = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 36);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LocalSettingBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(LocalSettingBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		return SaveAsync(item.Key, item.Value, tryCount, token, tranName);
	}

	public LocalSettingCacheRepository(ILocalSettingRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<LocalSettingBusiness>)dbRepository, CachingTime)
	{
		this.dbRepository = dbRepository ?? throw new ArgumentNullException("dbRepository");
		TimingCache = new TimingCache<string, LocalSettingBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public async Task<string> GetAsync(string key, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				if (TimingCache.TryGetValue(key, out var ret) && ret != null)
				{
					return ret.Value;
				}
				line = 5;
				string result = await dbRepository.GetAsync(key, token, tranName);
				line = 6;
				if (!string.IsNullOrEmpty(result))
				{
					line = 7;
					await TimingCache.UpdateTimeAsync(key, new LocalSettingBusiness(base.ContextGuid)
					{
						Key = key,
						Value = result
					});
				}
				line = 9;
				return result;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}//IsDisposed:{base.IsDisposed}", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 76);
				throw;
			}
		}
	}

	public string Get(string key, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs"))
		{
			int num = 0;
			try
			{
				num = 1;
				if (TimingCache.TryGetValue(key, out var ret) && ret != null)
				{
					return ret.Value;
				}
				num = 5;
				string text = dbRepository.Get(key, token, tranName);
				num = 6;
				if (text != null)
				{
					num = 7;
					TimingCache.UpdateTimeAsync(key, new LocalSettingBusiness(base.ContextGuid)
					{
						Key = key,
						Value = text
					});
				}
				num = 9;
				return text;
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{num}//IsDisposed:{base.IsDisposed}", "Get", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 110);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(string name, string value, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.SaveAsync(name, value, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 121);
				res.throwExceptionIfError();
				await TimingCache.UpdateTimeAsync(name, new LocalSettingBusiness(base.ContextGuid)
				{
					Key = name,
					Value = value
				});
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 127);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 128);
			}
			return res;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string name, short tryCount, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbRepository.RemoveAsync(name, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 139);
				TimingCache.TryRemove(name, out var _);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 144);
				res.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\LocalSettingCacheRepository.cs", 145);
			}
			return res;
		}
	}
}
