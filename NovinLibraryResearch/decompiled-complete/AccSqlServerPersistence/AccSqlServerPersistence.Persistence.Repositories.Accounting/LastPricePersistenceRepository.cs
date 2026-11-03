using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class LastPricePersistenceRepository : ILastPriceRepository, IHasContext, INoGuidRepository<LastPriceBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<LastPriceBusiness, LastPrice>, IItemAsyncMapper<LastPrice, LastPriceBusiness>
	{
		public Task<LastPrice> MapAsync(string contextGuid, LastPriceBusiness source, LastPrice destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
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
						destination = new LastPrice();
					}
					destination.Date = source.Date;
					destination.Modified = source.Modified;
					destination.Price = source.Price;
					destination.TafsilGuid = source.TafsilGuid;
					destination.PrdGuid = source.PrdGuid;
					destination.Invalidated = source.Invalidated;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.AsGuid = source.AsGuid;
					destination.AsType = source.AsType;
					destination.Guid = source.Guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 303);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<LastPriceBusiness> MapAsync(string contextGuid, LastPrice source, LastPriceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
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
						destination = new LastPriceBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Date = source.Date;
					destination.Modified = source.Modified;
					destination.Price = source.Price;
					destination.TafsilGuid = source.TafsilGuid;
					destination.PrdGuid = source.PrdGuid;
					destination.Invalidated = source.Invalidated;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.AsGuid = source.AsGuid;
					destination.AsType = source.AsType;
					destination.Guid = source.Guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 341);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<LastPrice>> MapListAsync(string contextGuid, IEnumerable<LastPriceBusiness> source, List<LastPrice> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<LastPriceBusiness>> MapListAsync(string contextGuid, IEnumerable<LastPrice> source, List<LastPriceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private readonly ContextHolder _DbContext;

	public bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public IItemAsyncMapper<LastPrice, LastPriceBusiness> MapFromDatabase => Mapper;

	public IItemAsyncMapper<LastPriceBusiness, LastPrice> MapToDatabase => Mapper;

	public bool HasStatus => false;

	public bool HasModified => true;

	public string ContextGuid => Context?.Guid ?? "";

	public WebContextHolder Context => _DbContext?.Context;

	public LastPricePersistenceRepository(ContextHolder db)
	{
		_DbContext = db;
	}

	public async Task<int> DbCountAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext baseModelContext = await _DbContext.GetContextAsync(tranName, 60000, "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 35);
		int result;
		try
		{
			result = ((IQueryable<LastPrice>)((DbContext)baseModelContext).Set<LastPrice>()).Count();
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
			WebErrorLog.Instance.StartLog(ex2, "", "DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 44);
			throw;
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await baseModelContext.DisposeAsync("DbCountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
			}
		}
		return result;
	}

	public virtual Task<IEnumerable<LastPriceBusiness>> GetAllAsync(CancellationToken token, string tranName)
	{
		return GetAllAsync(true, token, tranName);
	}

	public async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(bool? status, CancellationToken token, string tranName)
	{
		BaseModelContext c = await _DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 56);
		IEnumerable<LastPriceBusiness> result;
		try
		{
			_ = 1;
			try
			{
				List<LastPrice> source = ((IEnumerable<LastPrice>)((DbContext)c).Set<LastPrice>())?.ToList();
				result = await MapFromDatabase.MapListAsync(ContextGuid, source, null, tranName, token);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 67);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
			}
		}
		return result;
	}

	public async Task<LastPriceBusiness> GetAsync(LastPriceCompositeKey key, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await _DbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 79));
			LastPriceBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					LastPrice source = await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<LastPrice>((IQueryable<LastPrice>)c.LastPrice, (Expression<Func<LastPrice, bool>>)((LastPrice p) => p.TafsilGuid == key.TafsilGuid && p.PrdGuid == key.ProductGuid), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 89);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> SaveAllAsync(List<LastPriceBusiness> lst, bool flush, short tryCount, CancellationToken token, string tranName)
	{
		return SaveAll(lst, tryCount, token, tranName);
	}

	private async Task<ReturnedSaveFuncInfo> SaveAll(List<LastPriceBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await _DbContext.GetContextAsync(tranName, 60000, "SaveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 106));
				foreach (LastPriceBusiness item in lst)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					Acc_ModelContext acc_ModelContext = c;
					returnedSaveFuncInfo.AddReturnedValue(await acc_ModelContext.AddOrUpdateAsync(await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token), item.TafsilGuid, item.PrdGuid), ReturnedState.None, "SaveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 108);
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
				ret.AddReturnedValue(ex2, "", "SaveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 114);
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAll", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 115);
			}
			return ret;
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				Mapper = null;
				_DbContext.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 128);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveAsync(LastPriceBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await _DbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 137));
				LastPrice lastPrice = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(lastPrice, lastPrice.TafsilGuid, lastPrice.PrdGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 139);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 141);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<LastPriceBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			try
			{
				foreach (LastPriceBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 153);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 157);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 158);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SetSaveResult(SaveResponse<LastPriceCompositeKey> item, CancellationToken token, short tryCount, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		BaseModelContext context = await _DbContext.GetContextAsync(tranName, 60000, "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 166);
		using (new JaegerMonitor("SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				token.ThrowIfCancellationRequested();
				IQueryable<LastPrice> queryable = ((IQueryable<LastPrice>)((DbContext)context).Set<LastPrice>()).Where((LastPrice x) => x.TafsilGuid == item.Key.TafsilGuid && x.PrdGuid == item.Key.ProductGuid);
				LastPrice lastPrice = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<LastPrice>(queryable, token) : null);
				if (lastPrice == null)
				{
					return ret;
				}
				lastPrice.HostRefrenced = item.HostRefrenced;
				lastPrice.ServerStatus = (item.SaveResultStatus ? ServerStatusForClient.Sent : ServerStatusForClient.SendError);
				lastPrice.DeliveryMessage = item.Message;
				lastPrice.DeliveryShortMessage = item.Shortmessage;
				((DbContext)context).Update<LastPrice>(lastPrice);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 182);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 185);
				ret.AddReturnedValue(ex, "", "SetSaveResult", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 186);
			}
			return ret;
		}
	}

	public virtual async Task<List<LastPriceCompositeKey>> GetAllNotSentAsync(CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs"))
		{
			BaseModelContext c = await _DbContext.GetContextAsync(tranName, 60000, "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 194);
			List<LastPriceCompositeKey> result;
			try
			{
				_ = 1;
				try
				{
					DbSet<LastPrice> obj = ((DbContext)c).Set<LastPrice>();
					List<LastPrice> list = await ((obj != null) ? EntityFrameworkQueryableExtensions.ToListAsync<LastPrice>(((IQueryable<LastPrice>)obj).Where((LastPrice x) => x.ServerStatus != ServerStatusForClient.Delivered && x.ServerStatus != ServerStatusForClient.DirectDelivery && x.ServerStatus != ServerStatusForClient.Seen && x.ServerStatus != ServerStatusForClient.Sent), token) : null);
					result = ((list != null && list.Count <= 0) ? null : list?.Select((LastPrice q) => new LastPriceCompositeKey
					{
						TafsilGuid = q.TafsilGuid,
						ProductGuid = q.PrdGuid
					})?.ToList());
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 208);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllNotSentAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid productGuid, Guid tafsilGuid, Document asType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await _DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 219));
		IEnumerable<LastPriceBusiness> result;
		try
		{
			_ = 1;
			try
			{
				List<LastPrice> list = ((IQueryable<LastPrice>)c.LastPrice).Where((LastPrice q) => q.PrdGuid == productGuid && q.TafsilGuid == tafsilGuid && q.AsType == asType).ToList();
				result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, list, null, tranName, token)) : null);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 231);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
			}
		}
		return result;
	}

	public async Task<IEnumerable<LastPriceBusiness>> GetAllAsync(Guid orderGuid, Document asType, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await _DbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 242));
		IEnumerable<LastPriceBusiness> result;
		try
		{
			_ = 1;
			try
			{
				List<LastPrice> list = ((IQueryable<LastPrice>)c.LastPrice).Where((LastPrice q) => q.AsGuid == orderGuid && q.AsType == asType).ToList();
				result = ((list != null && list.Any()) ? (await MapFromDatabase.MapListAsync(ContextGuid, list, null, tranName, token)) : null);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				if (ex3.Message.Contains("Operation cancelled by user."))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs", 254);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\LastPricepersistencerepository.cs");
			}
		}
		return result;
	}

	public void ResetCache()
	{
		throw new NotImplementedException();
	}
}
