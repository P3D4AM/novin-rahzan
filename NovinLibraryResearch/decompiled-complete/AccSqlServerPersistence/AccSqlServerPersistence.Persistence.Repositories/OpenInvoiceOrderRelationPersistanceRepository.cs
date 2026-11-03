using System;
using System.Collections.Generic;
using System.Linq;
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
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class OpenInvoiceOrderRelationPersistanceRepository : PersistenceNoGuidRepository<OpenInvoiceOrderRelationBusiness, OpenInvoiceOrderRelation>, IOpenInvoiceOrderRelationRepository, INoGuidRepository<OpenInvoiceOrderRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OpenInvoiceOrderRelationBusiness, OpenInvoiceOrderRelation>, IItemAsyncMapper<OpenInvoiceOrderRelation, OpenInvoiceOrderRelationBusiness>
	{
		public Task<OpenInvoiceOrderRelation> MapAsync(string contextGuid, OpenInvoiceOrderRelationBusiness source, OpenInvoiceOrderRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoiceOrderRelation();
				}
				destination.InvoiceGuid = source.InvoiceGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 116);
				throw;
			}
		}

		public Task<OpenInvoiceOrderRelationBusiness> MapAsync(string contextGuid, OpenInvoiceOrderRelation source, OpenInvoiceOrderRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoiceOrderRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.InvoiceGuid = source.InvoiceGuid;
				destination.OrderGuid = source.OrderGuid;
				destination.Ordinal = source.Ordinal;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 140);
				throw;
			}
		}

		public Task<List<OpenInvoiceOrderRelation>> MapListAsync(string contextGuid, IEnumerable<OpenInvoiceOrderRelationBusiness> source, List<OpenInvoiceOrderRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OpenInvoiceOrderRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoiceOrderRelation> source, List<OpenInvoiceOrderRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<OpenInvoiceOrderRelation, OpenInvoiceOrderRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OpenInvoiceOrderRelationBusiness, OpenInvoiceOrderRelation> MapToDatabase => Mapper;

	public OpenInvoiceOrderRelationPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 37);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(OpenInvoiceOrderRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<OpenInvoiceOrderRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
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
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 47));
			foreach (OpenInvoiceOrderRelationBusiness item in items)
			{
				if (item == null)
				{
					WebErrorLog.Instance.StartLog("item of list is null!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 52);
					return res;
				}
				OpenInvoiceOrderRelation openInvoiceOrderRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(openInvoiceOrderRelation, openInvoiceOrderRelation.InvoiceGuid, openInvoiceOrderRelation.OrderGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 56);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 61);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 62);
		}
		return res;
	}

	public async Task GetAllAsync(OpenInvoiceBusiness master, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 69));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					List<OpenInvoiceOrderRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<OpenInvoiceOrderRelation>(((IQueryable<OpenInvoiceOrderRelation>)c.OpenInvoiceOrderRelation).Where((OpenInvoiceOrderRelation x) => x.InvoiceGuid == master.Guid), token);
					if (list == null || !list.Any())
					{
						return;
					}
					foreach (OpenInvoiceOrderRelation item in list)
					{
						OpenInvoiceOrderRelationBusiness data = await MapFromDatabase.MapAsync(base.Context.Guid, item, null, tranName, token);
						await master.AddOrderRelationAsync(data, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoiceOrderRelationPersistanceRepository.cs");
				}
			}
		}
	}
}
