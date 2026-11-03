using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business.OrderDepot.OpenInvoices;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Order.OpenInvoices;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class OpenInvoicePersistanceRepository : PersistenceRepository<OpenInvoiceBusiness, OpenInvoice>, IOpenInvoiceRepository, IRepository<OpenInvoiceBusiness>, INoGuidRepository<OpenInvoiceBusiness>, IDisposable, IEditableDelivery
{
	private class mapper : IItemAsyncMapper<OpenInvoiceBusiness, OpenInvoice>, IItemAsyncMapper<OpenInvoice, OpenInvoiceBusiness>
	{
		public Task<OpenInvoice> MapAsync(string contextGuid, OpenInvoiceBusiness source, OpenInvoice destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoice();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.PeopleCount = source.PeopleCount;
				destination.SanadNo = source.SanadNo;
				destination.Sum = source.Sum;
				destination.Number = source.Number;
				destination.SettlementDate = source.SettlementDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 220);
				throw;
			}
		}

		public Task<OpenInvoiceBusiness> MapAsync(string contextGuid, OpenInvoice source, OpenInvoiceBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoiceBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.DateM = source.DateM;
				destination.TafsilGuid = source.TafsilGuid;
				destination.CashierGuid = source.CashierGuid;
				destination.Description = source.Description;
				destination.ReceptionGuid = source.ReceptionGuid;
				destination.PardakhtGuid = source.PardakhtGuid;
				destination.PeopleCount = source.PeopleCount;
				destination.SanadNo = source.SanadNo;
				destination.Sum = source.Sum;
				destination.Number = source.Number;
				destination.SettlementDate = source.SettlementDate;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 255);
				throw;
			}
		}

		public Task<List<OpenInvoice>> MapListAsync(string contextGuid, IEnumerable<OpenInvoiceBusiness> source, List<OpenInvoice> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OpenInvoiceBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoice> source, List<OpenInvoiceBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private OpenInvoicePlacementRelationPersistanceRepository _placementRepo;

	private OpenInvoiceOrderRelationPersistanceRepository _orderRepo;

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<OpenInvoice, OpenInvoiceBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OpenInvoiceBusiness, OpenInvoice> MapToDatabase => Mapper;

	public override IItemMapper<OpenInvoice, OpenInvoiceBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public OpenInvoicePersistanceRepository(ContextHolder db)
		: base(db)
	{
		try
		{
			_placementRepo = new OpenInvoicePlacementRelationPersistanceRepository(db);
			_orderRepo = new OpenInvoiceOrderRelationPersistanceRepository(db);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 38);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 50);
		}
	}

	public async Task<int> GetNextNumberAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 54));
		int result;
		try
		{
			_ = 1;
			try
			{
				result = ((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<OpenInvoice>(((IQueryable<OpenInvoice>)c.OpenInvoice).OrderByDescending((OpenInvoice x) => x.Number).Take(1), token))?.Number ?? 0) + 1;
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
				WebErrorLog.Instance.StartLog(ex3, "", "GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 65);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetNextNumberAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs");
			}
		}
		return result;
	}

	public async Task<Guid> Id2ToGuidAsync(int id2, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 76));
		using (new JaegerMonitor("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs"))
		{
			Guid result;
			try
			{
				_ = 1;
				try
				{
					result = (await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<OpenInvoice>((IQueryable<OpenInvoice>)c.OpenInvoice, (Expression<Func<OpenInvoice, bool>>)((OpenInvoice x) => x.Number == id2), token))?.Guid ?? Guid.Empty;
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
					WebErrorLog.Instance.StartLog(ex3, "", "Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 88);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("Id2ToGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs");
				}
			}
			return result;
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(OpenInvoiceBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 105));
				OpenInvoice openInvoice = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(openInvoice, openInvoice.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 107);
				if (res.HasError)
				{
					return res;
				}
				if (item.PlacementRelationList?.Any() ?? false)
				{
					IQueryable<OpenInvoicePlacementRelation> queryable = ((IQueryable<OpenInvoicePlacementRelation>)dbcontext.OpenInvoicePlacementRelation).Where((OpenInvoicePlacementRelation q) => q.InvoiceGuid == item.Guid);
					if (queryable != null && queryable.Any())
					{
						IQueryable<OpenInvoicePlacementRelation> queryable2 = queryable.Where((OpenInvoicePlacementRelation q) => !item.PlacementRelationList.Select((OpenInvoicePlacementRelationBusiness q) => q.PlacementGuid).Contains(q.PlacementGuid));
						if (queryable2 != null && queryable2.Any())
						{
							dbcontext.OpenInvoicePlacementRelation.RemoveRange((IEnumerable<OpenInvoicePlacementRelation>)queryable2);
						}
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _placementRepo.SaveRangeAsync(item.PlacementRelationList, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 117);
				}
				if (item.OrderRelationList?.Any() ?? false)
				{
					IQueryable<OpenInvoiceOrderRelation> queryable3 = ((IQueryable<OpenInvoiceOrderRelation>)dbcontext.OpenInvoiceOrderRelation).Where((OpenInvoiceOrderRelation q) => q.InvoiceGuid == item.Guid);
					if (queryable3 != null && queryable3.Any())
					{
						IQueryable<OpenInvoiceOrderRelation> queryable4 = queryable3.Where((OpenInvoiceOrderRelation q) => !item.OrderRelationList.Select((OpenInvoiceOrderRelationBusiness q) => q.OrderGuid).Contains(q.OrderGuid));
						if (queryable4 != null && queryable4.Any())
						{
							dbcontext.OpenInvoiceOrderRelation.RemoveRange((IEnumerable<OpenInvoiceOrderRelation>)queryable4);
						}
					}
					returnedSaveFuncInfo = res;
					returnedSaveFuncInfo.AddReturnedValue(await _orderRepo.SaveRangeAsync(item.OrderRelationList, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 127);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 132);
				res.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 133);
			}
			return res;
		}
	}

	public async Task<OpenInvoiceBusiness> GetByOrderGuidAsync(Guid orderGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 139));
		OpenInvoiceBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				if (orderGuid == Guid.Empty)
				{
					result = null;
				}
				else
				{
					OpenInvoiceOrderRelation openInvoiceOrderRelation = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<OpenInvoiceOrderRelation>((IQueryable<OpenInvoiceOrderRelation>)c.OpenInvoiceOrderRelation, (Expression<Func<OpenInvoiceOrderRelation, bool>>)((OpenInvoiceOrderRelation q) => q.OrderGuid == orderGuid), token);
					result = ((openInvoiceOrderRelation != null) ? (await GetAsync(openInvoiceOrderRelation.InvoiceGuid, token, tranName)) : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 151);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetByOrderGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<OpenInvoiceBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 162));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs"))
		{
			OpenInvoiceBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					OpenInvoice openInvoice = await c.OpenInvoice.FindAsync(new object[1] { guid });
					if (openInvoice == null)
					{
						result = null;
					}
					else
					{
						OpenInvoiceBusiness ret = await MapFromDatabase.MapAsync(ContextGuid, openInvoice, null, tranName, token);
						await _orderRepo.GetAllAsync(ret, token, tranName);
						await _placementRepo.GetAllAsync(ret, token, tranName);
						result = ret;
					}
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs", 178);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePersistanceRepository.cs");
				}
			}
			return result;
		}
	}
}
