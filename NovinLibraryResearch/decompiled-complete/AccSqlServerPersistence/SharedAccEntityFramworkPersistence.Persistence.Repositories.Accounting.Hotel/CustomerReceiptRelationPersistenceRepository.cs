using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.Hotel;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Hotel;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Hotel;

internal class CustomerReceiptRelationPersistenceRepository : PersistenceNoGuidRepository<CustomerReceiptRelationBusiness, CustomerReceiptRelation>, ICustomerReceiptRelationRepository, INoGuidRepository<CustomerReceiptRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CustomerReceiptRelationBusiness, CustomerReceiptRelation>, IItemAsyncMapper<CustomerReceiptRelation, CustomerReceiptRelationBusiness>
	{
		public Task<CustomerReceiptRelationBusiness> MapAsync(string contextGuid, CustomerReceiptRelation source, CustomerReceiptRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CustomerReceiptRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.CustomerGuid = source.CustomerGuid;
				destination.ReceiptGuid = source.ReceiptGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 141);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<CustomerReceiptRelation> MapAsync(string contextGuid, CustomerReceiptRelationBusiness source, CustomerReceiptRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CustomerReceiptRelation();
				}
				destination.CustomerGuid = source.CustomerGuid;
				destination.ReceiptGuid = source.ReceiptGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 163);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<CustomerReceiptRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerReceiptRelation> source, List<CustomerReceiptRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CustomerReceiptRelation>> MapListAsync(string contextGuid, IEnumerable<CustomerReceiptRelationBusiness> source, List<CustomerReceiptRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	internal class CustomerReciptBusinessWithEmptyConstructor : CustomerReceiptRelationBusiness
	{
		public CustomerReciptBusinessWithEmptyConstructor()
			: base(Guid.Empty.ToString())
		{
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CustomerReceiptRelation, CustomerReceiptRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CustomerReceiptRelationBusiness, CustomerReceiptRelation> MapToDatabase => Mapper;

	public CustomerReceiptRelationPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = false;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 38);
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CustomerReceiptRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs"))
		{
			_ = 2;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 46));
				CustomerReceiptRelation customerReceiptRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(customerReceiptRelation, customerReceiptRelation.CustomerGuid, customerReceiptRelation.ReceiptGuid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 48);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 53);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 54);
			}
			return res;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CustomerReceiptRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(items?.Any() ?? false))
			{
				return res;
			}
			foreach (CustomerReceiptRelationBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 65);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 69);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 70);
		}
		return res;
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(Guid receiptGuid, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 79));
			IQueryable<CustomerReceiptRelation> queryable = ((IQueryable<CustomerReceiptRelation>)acc_ModelContext.CustomerReciptRelation).Where((CustomerReceiptRelation q) => q.ReceiptGuid == receiptGuid);
			if (queryable == null || !queryable.Any())
			{
				return res;
			}
			((DbContext)acc_ModelContext).RemoveRange((IEnumerable<object>)queryable);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 86);
			res.AddReturnedValue(ex, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 87);
		}
		return res;
	}

	public async Task<IEnumerable<CustomerReceiptRelationBusiness>> GetAllAsync(Guid receiptGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 93));
		IEnumerable<CustomerReceiptRelationBusiness> result;
		try
		{
			_ = 1;
			try
			{
				List<CustomerReceiptRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<CustomerReceiptRelation>(((IQueryable<CustomerReceiptRelation>)EntityFrameworkQueryableExtensions.Include<CustomerReceiptRelation, AccSqlServerPersistence.Entities.Customer>((IQueryable<CustomerReceiptRelation>)c.CustomerReciptRelation, (Expression<Func<CustomerReceiptRelation, AccSqlServerPersistence.Entities.Customer>>)((CustomerReceiptRelation q) => q.Customer))).Where((CustomerReceiptRelation q) => q.ReceiptGuid == receiptGuid), token);
				result = ((list != null && list.Any()) ? list.Select((CustomerReceiptRelation q) => new CustomerReciptBusinessWithEmptyConstructor
				{
					ContextGuid = base.ContextGuid,
					CustomerFamily = q.Customer.Family,
					CustomerGuid = q.CustomerGuid,
					CustomerName = q.Customer.Name,
					ReceiptGuid = q.ReceiptGuid
				})?.ToList() : null);
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs", 111);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Hotel\\CustomerReceiptionRelationPersistenceRepository.cs");
			}
		}
		return result;
	}
}
