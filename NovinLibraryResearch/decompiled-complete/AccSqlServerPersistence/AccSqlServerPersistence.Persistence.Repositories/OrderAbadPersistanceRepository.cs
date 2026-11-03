using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.OrderDepot;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class OrderAbadPersistanceRepository : PersistenceRepository<OrderAbadBusiness, OrderAbad>, IOrderAbadRepository, IRepository<OrderAbadBusiness>, INoGuidRepository<OrderAbadBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OrderAbadBusiness, OrderAbad>, IItemAsyncMapper<OrderAbad, OrderAbadBusiness>
	{
		public Task<OrderAbad> MapAsync(string contextGuid, OrderAbadBusiness source, OrderAbad destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OrderAbad();
				}
				destination.OrderType = source.OrderType;
				destination.DetailGuid = source.DetailGuid;
				destination.RefrencedGuid = source.RefrencedGuid;
				destination.ShapeInfo = source.ShapeInfo;
				destination.Guid = source.Guid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 138);
				throw;
			}
		}

		public Task<OrderAbadBusiness> MapAsync(string contextGuid, OrderAbad source, OrderAbadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs"))
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
						destination = new OrderAbadBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.OrderType = source.OrderType;
					destination.DetailGuid = source.DetailGuid;
					destination.RefrencedGuid = source.RefrencedGuid;
					destination.ShapeInfo = source.ShapeInfo;
					destination.Guid = source.Guid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 166);
					throw;
				}
			}
		}

		public Task<List<OrderAbad>> MapListAsync(string contextGuid, IEnumerable<OrderAbadBusiness> source, List<OrderAbad> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OrderAbadBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderAbad> source, List<OrderAbadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private AbadDetPersistanceRepository _detRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<OrderAbad, OrderAbadBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OrderAbadBusiness, OrderAbad> MapToDatabase => Mapper;

	public override IItemMapper<OrderAbad, OrderAbadBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public OrderAbadPersistanceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new AbadDetPersistanceRepository(db);
	}

	public async Task<OrderAbadBusiness> GetAsync(Guid DetailGuid, Document OrderType, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 34));
			OrderAbadBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					OrderAbad obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<OrderAbad>((IQueryable<OrderAbad>)c.OrderAbads, (Expression<Func<OrderAbad, bool>>)((OrderAbad p) => p.DetailGuid == DetailGuid && p.OrderType == OrderType), token);
					if (obj == null)
					{
						result = null;
					}
					else
					{
						OrderAbadBusiness mappedItem = await MapFromDatabase.MapAsync(ContextGuid, obj, null, tranName, token);
						IQueryable<AbadDet> queryable = ((IQueryable<AbadDet>)c.AbadDets).Where((AbadDet q) => q.MasterGuid == obj.Guid);
						if (queryable != null && queryable.Any() && _detRepo != null)
						{
							mappedItem.AddRangeToDetList(await _detRepo.MapFromDatabase.MapListAsync(ContextGuid, queryable, null, tranName, token), clearDets: true);
						}
						result = mappedItem;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 52);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> RemoveAsync(Guid OrderGuid, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 68));
				OrderAbad obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<OrderAbad>((IQueryable<OrderAbad>)dbcontext.OrderAbads, (Expression<Func<OrderAbad, bool>>)((OrderAbad w) => w.RefrencedGuid == OrderGuid), token);
				if (obj == null)
				{
					return ret;
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.RemoveAsync(obj.Guid, tryCount, token, tranName), ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 71);
				dbcontext.OrderAbads.Remove(obj);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 76);
				ret.AddReturnedValue(ret, ReturnedState.None, "RemoveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 77);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(OrderAbadBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 89));
				OrderAbad orderAbad = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(orderAbad, orderAbad.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 91);
				if (res.HasError)
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.ListAbad?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 93);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 97);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 98);
			}
			return res;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OrderAbadPersistanceRepository.cs", 111);
		}
	}
}
