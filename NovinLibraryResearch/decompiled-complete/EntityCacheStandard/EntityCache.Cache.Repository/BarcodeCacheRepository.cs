using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Mapping.BusinessMappers;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Product;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository;

public class BarcodeCacheRepository : CacheRepositoryNoGuiId<BarcodeBusiness, string>, IBarcodeRepository, INoGuidRepository<BarcodeBusiness>, IDisposable
{
	private ConcurrentDictionary<string, BarcodeBusiness> Entities = new ConcurrentDictionary<string, BarcodeBusiness>();

	private IBarcodeRepository _dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, BarcodeBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<BarcodeBusiness, BarcodeBusiness> Mapper => BarcodeMapper.Instance;

	public BarcodeCacheRepository(INoGuidRepository<BarcodeBusiness> dbRepository, int CachingTime)
		: base(dbRepository, CachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		_dbRepository = (IBarcodeRepository)dbRepository;
		TimingCache = new TimingCache<string, BarcodeBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				Entities = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 45);
		}
	}

	public async Task<BarcodeBusiness> GetAsync(string barcode, CancellationToken token, string tranName)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs"))
		{
			try
			{
				if (string.IsNullOrEmpty(barcode))
				{
					return null;
				}
				Entities.TryGetValue(barcode, out var value);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (value != null)
				{
					return value;
				}
				if (token.IsCancellationRequested)
				{
					return null;
				}
				BarcodeBusiness barcodeBusiness = await _dbRepository.GetAsync(barcode, token, tranName);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				if (barcodeBusiness != null)
				{
					Entities.TryAdd(barcode, barcodeBusiness);
				}
				return barcodeBusiness;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 66);
				throw;
			}
		}
	}

	public async Task<List<BarcodeBusiness>> GetByPrdGuidAsync(List<Guid> prdGuid, CancellationToken token, string tranName)
	{
		int line = 0;
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs"))
		{
			try
			{
				line = 1;
				List<BarcodeBusiness> list = await _dbRepository.GetByPrdGuidAsync(prdGuid, token, tranName);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				line = 2;
				if (list != null && list.Count > 0)
				{
					foreach (BarcodeBusiness item in list)
					{
						line = 3;
						Entities.TryAdd(item.BarCode, item);
						line = 4;
						if (token.IsCancellationRequested)
						{
							return null;
						}
					}
				}
				return list;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"line={line}", "GetByPrdGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 94);
				throw;
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsyncByPrdGuid(Guid prdGuid, short tryCount, string tranName, CancellationToken token)
	{
		int line = 0;
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveAsync_prdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				line = 1;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 107);
				}
				await GetByPrdGuidAsync(new List<Guid> { prdGuid }, token, tranName);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 109);
				}
				line = 2;
				IEnumerable<BarcodeBusiness> enumerable = Entities?.Values?.Where((BarcodeBusiness p) => p.PrdGuid == prdGuid);
				if (enumerable != null && enumerable.Any())
				{
					foreach (BarcodeBusiness item in enumerable)
					{
						if (item != null)
						{
							Entities.TryRemove(item.BarCode, out var _);
						}
					}
				}
				line = 10;
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 117);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.RemoveAsyncByPrdGuid(prdGuid, tryCount, tranName, token), ReturnedState.None, "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 118);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"The Error Line IS : {line}//IsDisposed:{base.IsDisposed}", "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 122);
				ret.AddReturnedValue(ex, "", "RemoveAsyncByPrdGuid", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 123);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveAsync(string barCode, short tryCount, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveAsync_StringBarcode", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Entities.TryRemove(barCode, out var _);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 135);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.RemoveAsync(barCode, tryCount, tranName, token), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 136);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 140);
				ret.AddReturnedValue(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 141);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<Guid> prdGuid, short tryCount, string tranName, CancellationToken token)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				List<BarcodeBusiness> list = await GetByPrdGuidAsync(prdGuid, token, tranName);
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 153);
				}
				if (list != null && list.Any())
				{
					foreach (BarcodeBusiness item in list)
					{
						if (token.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 157);
						}
						Entities.TryRemove(item.BarCode, out var _);
					}
				}
				if (token.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 160);
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _dbRepository.RemoveRangeAsync(prdGuid, tryCount, tranName, token), ReturnedState.None, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 161);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 165);
				ret.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\BarcodeCacheRepository.cs", 166);
			}
			return ret;
		}
	}

	public Task<ReturnedSaveFuncInfo> FillBarcodesAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.FillBarcodesAsync(token, tranName);
	}

	public Task<IEnumerable<BarcodeModel>> GetAllConflictsAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllConflictsAsync(token, tranName);
	}
}
