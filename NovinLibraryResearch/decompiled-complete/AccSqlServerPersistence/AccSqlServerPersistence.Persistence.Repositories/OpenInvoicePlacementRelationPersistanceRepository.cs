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

internal class OpenInvoicePlacementRelationPersistanceRepository : PersistenceNoGuidRepository<OpenInvoicePlacementRelationBusiness, OpenInvoicePlacementRelation>, IOpenInvoicePlacementRelationRepository, INoGuidRepository<OpenInvoicePlacementRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<OpenInvoicePlacementRelationBusiness, OpenInvoicePlacementRelation>, IItemAsyncMapper<OpenInvoicePlacementRelation, OpenInvoicePlacementRelationBusiness>
	{
		public Task<OpenInvoicePlacementRelation> MapAsync(string contextGuid, OpenInvoicePlacementRelationBusiness source, OpenInvoicePlacementRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoicePlacementRelation();
				}
				destination.InvoiceGuid = source.InvoiceGuid;
				destination.PlacementGuid = source.PlacementGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 116);
				throw;
			}
		}

		public Task<OpenInvoicePlacementRelationBusiness> MapAsync(string contextGuid, OpenInvoicePlacementRelation source, OpenInvoicePlacementRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new OpenInvoicePlacementRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.InvoiceGuid = source.InvoiceGuid;
				destination.PlacementGuid = source.PlacementGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 140);
				throw;
			}
		}

		public Task<List<OpenInvoicePlacementRelation>> MapListAsync(string contextGuid, IEnumerable<OpenInvoicePlacementRelationBusiness> source, List<OpenInvoicePlacementRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<OpenInvoicePlacementRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<OpenInvoicePlacementRelation> source, List<OpenInvoicePlacementRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<OpenInvoicePlacementRelation, OpenInvoicePlacementRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<OpenInvoicePlacementRelationBusiness, OpenInvoicePlacementRelation> MapToDatabase => Mapper;

	public OpenInvoicePlacementRelationPersistanceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 37);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(OpenInvoicePlacementRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<OpenInvoicePlacementRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
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
			Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 47));
			foreach (OpenInvoicePlacementRelationBusiness item in items)
			{
				if (item == null)
				{
					WebErrorLog.Instance.StartLog("item of list is null!", "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 52);
					return res;
				}
				OpenInvoicePlacementRelation openInvoicePlacementRelation = await MapToDatabase.MapAsync(base.ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(openInvoicePlacementRelation, openInvoicePlacementRelation.InvoiceGuid, openInvoicePlacementRelation.PlacementGuid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 56);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 61);
			res.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 62);
		}
		return res;
	}

	public async Task GetAllAsync(OpenInvoiceBusiness master, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 69));
			try
			{
				int num;
				_ = num - 1;
				_ = 2;
				try
				{
					List<OpenInvoicePlacementRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<OpenInvoicePlacementRelation>(((IQueryable<OpenInvoicePlacementRelation>)c.OpenInvoicePlacementRelation).Where((OpenInvoicePlacementRelation x) => x.InvoiceGuid == master.Guid), token);
					if (list == null || !list.Any())
					{
						return;
					}
					foreach (OpenInvoicePlacementRelation item in list)
					{
						OpenInvoicePlacementRelationBusiness data = await MapFromDatabase.MapAsync(base.Context.Guid, item, null, tranName, token);
						await master.AddPlacementRelationAsync(data, token);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Order\\OpenInvoices\\OpenInvoicePlacementRelationPersistanceRepository.cs");
				}
			}
		}
	}
}
