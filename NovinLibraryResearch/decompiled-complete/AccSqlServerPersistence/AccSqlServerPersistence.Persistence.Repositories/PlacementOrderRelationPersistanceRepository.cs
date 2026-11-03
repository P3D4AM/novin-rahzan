using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order.OpenInvoices;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Bill;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class PlacementOrderRelationPersistanceRepository : PersistenceNoGuidRepository<PlacementOrderRelationBusiness, PlacementOrderRelation>, IPlacementOrderRelationRepository, INoGuidRepository<PlacementOrderRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PlacementOrderRelationBusiness, PlacementOrderRelation>, IItemAsyncMapper<PlacementOrderRelation, PlacementOrderRelationBusiness>
	{
		public Task<PlacementOrderRelation> MapAsync(string contextGuid, PlacementOrderRelationBusiness source, PlacementOrderRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PlacementOrderRelation();
				}
				destination.PlacementGuid = source.PlacementGuid;
				destination.OrderGuid = source.OrderGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 145);
				throw;
			}
		}

		public Task<PlacementOrderRelationBusiness> MapAsync(string contextGuid, PlacementOrderRelation source, PlacementOrderRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new PlacementOrderRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.PlacementGuid = source.PlacementGuid;
				destination.OrderGuid = source.OrderGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 168);
				throw;
			}
		}

		public Task<List<PlacementOrderRelation>> MapListAsync(string contextGuid, IEnumerable<PlacementOrderRelationBusiness> source, List<PlacementOrderRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PlacementOrderRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<PlacementOrderRelation> source, List<PlacementOrderRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<PlacementOrderRelation, PlacementOrderRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PlacementOrderRelationBusiness, PlacementOrderRelation> MapToDatabase => Mapper;

	public PlacementOrderRelationPersistanceRepository(ContextHolder db)
		: base(db)
	{
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 37);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(PlacementOrderRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PlacementOrderRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (!(items?.Any() ?? false))
		{
			throw new Exception("items is null or empty");
		}
		try
		{
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 47));
			foreach (PlacementOrderRelationBusiness item in items)
			{
				if (item == null)
				{
					WebErrorLog.Instance.StartLog("item of list is null!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 52);
					return res;
				}
				PlacementOrderRelation placementOrderRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(placementOrderRelation, placementOrderRelation.PlacementGuid, placementOrderRelation.OrderGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 56);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 61);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 62);
		}
		return res;
	}

	public async Task<IEnumerable<PlacementOrderRelationReportBusiness>> GetAllModelAsync(Guid? placementGuid, Guid? orderGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 68));
		IEnumerable<PlacementOrderRelationReportBusiness> result;
		try
		{
			_ = 1;
			try
			{
				IQueryable<PlacementOrderRelation> queryable = (IQueryable<PlacementOrderRelation>)EntityFrameworkQueryableExtensions.Include<PlacementOrderRelation, Placement>((IQueryable<PlacementOrderRelation>)EntityFrameworkQueryableExtensions.Include<PlacementOrderRelation, Tafsil>((IQueryable<PlacementOrderRelation>)EntityFrameworkQueryableExtensions.Include<PlacementOrderRelation, Order>((IQueryable<PlacementOrderRelation>)c.PlacementOrderRelation, (Expression<Func<PlacementOrderRelation, Order>>)((PlacementOrderRelation q) => q.OrderItem)), (Expression<Func<PlacementOrderRelation, Tafsil>>)((PlacementOrderRelation q) => q.OrderItem.TafsilItem)), (Expression<Func<PlacementOrderRelation, Placement>>)((PlacementOrderRelation q) => q.PlacementItem));
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					if ((placementGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((PlacementOrderRelation q) => q.PlacementGuid == ((Guid?)placementGuid).Value);
					}
					if ((orderGuid ?? Guid.Empty) != Guid.Empty)
					{
						queryable = queryable?.Where((PlacementOrderRelation q) => q.OrderGuid == ((Guid?)orderGuid).Value);
					}
					List<IGrouping<Guid, PlacementOrderRelation>> list = (await ((queryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<PlacementOrderRelation>(queryable, token) : null))?.GroupBy((PlacementOrderRelation q) => q.PlacementGuid)?.ToList();
					if (list == null || !list.Any())
					{
						result = null;
					}
					else
					{
						List<PlacementOrderRelationReportBusiness> list2 = list?.Select((IGrouping<Guid, PlacementOrderRelation> q) => new PlacementOrderRelationReportBusiness
						{
							DailyOrderNumber = q.Max((PlacementOrderRelation q) => q.OrderItem.DailyFactorNo),
							DateM = q.Max((PlacementOrderRelation q) => q.OrderItem.Date),
							Discount = q.Sum((PlacementOrderRelation q) => q.OrderItem.Discount),
							Id2 = q.FirstOrDefault().OrderItem.Id2,
							OrderGuid = q.FirstOrDefault().OrderGuid,
							Tax = q.Sum((PlacementOrderRelation q) => q.OrderItem.Tax),
							TotalSum = q.Sum((PlacementOrderRelation q) => q.OrderItem.TotalSum),
							TafsilName = q.FirstOrDefault().OrderItem.TafsilItem.Name,
							PlacementTitle = q.FirstOrDefault().PlacementItem.Title,
							PlacementGuid = q.Key
						})?.ToList();
						result = list2;
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 97);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(List<PlacementOrderRelationBusiness> list, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("TranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		if (!(list?.Any() ?? false))
		{
			throw new Exception("items is null or empty");
		}
		try
		{
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 113));
			List<PlacementOrderRelation> list2 = await MapToDatabase.MapListAsync(base.ContextGuid, list, null, tranName, token);
			dbcontext.PlacementOrderRelation.RemoveRange((IEnumerable<PlacementOrderRelation>)list2);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 119);
			res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\PlacementOrderRelationPersistanceRepository.cs", 120);
		}
		return res;
	}
}
