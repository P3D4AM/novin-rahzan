using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.DepotReceipt;
using Microsoft.EntityFrameworkCore;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.DepotReceipts;

internal class DepotReceiptDetailPersistenceRepository : PersistenceMasterRepository<DepotReceiptDetailBusiness, DepotReceiptDetail>, IDepotReceiptDetailRepository, IPersistenceMasterRepository<DepotReceiptDetailBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<DepotReceiptDetailBusiness, DepotReceiptDetail>, IItemAsyncMapper<DepotReceiptDetail, DepotReceiptDetailBusiness>
	{
		public Task<DepotReceiptDetail> MapAsync(string contextGuid, DepotReceiptDetailBusiness source, DepotReceiptDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new DepotReceiptDetail();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.MasterGuid = source.MasterGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.DepotGuid = source.DepotGuid;
				destination.Quantity = source.Quantity;
				destination.V2Guid = source.V2Guid;
				destination.V2Quantity = source.V2Quantity;
				destination.Ordinal = source.Ordinal;
				destination.ReferenceGuid = source.ReferenceGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs", 88);
				throw;
			}
		}

		public async Task<DepotReceiptDetailBusiness> MapAsync(string contextGuid, DepotReceiptDetail source, DepotReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			if (destination == null)
			{
				throw new ArgumentNullException("Please Pass New Destination");
			}
			try
			{
				if (source == null)
				{
					destination = null;
					return destination;
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.MasterGuid = source.MasterGuid;
				await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
				await destination.SetDepotGuidAsync(source.DepotGuid, token, tranName);
				await destination.SetV2GuidAsync(EnSystem.Mapper, source.V2Guid, token, tranName);
				await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
				await destination.SetV2QtAsync(source.V2Quantity, token, tranName);
				destination.Ordinal = source.Ordinal;
				destination.ReferenceGuid = source.ReferenceGuid;
				destination.ChangeStatus = EntityChangeSt.Edited;
				return destination;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs", 122);
				throw;
			}
		}

		public Task<List<DepotReceiptDetail>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptDetailBusiness> source, List<DepotReceiptDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<DepotReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptDetail> source, List<DepotReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<DepotReceiptDetail, DepotReceiptDetailBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<DepotReceiptDetailBusiness, DepotReceiptDetail> MapToDatabase => Mapper;

	public DepotReceiptDetailPersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
	}

	public override Task<List<DepotReceiptDetailBusiness>> GetAllAsync(Guid MasterGuid, CancellationToken token, string tranName)
	{
		throw new NotImplementedException("please use another overLoad");
	}

	public async Task GetAllAsync(DepotReceiptBusiness master, CancellationToken token, string tranName)
	{
		if (base.IsDisposed || token.IsCancellationRequested)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs", 32));
			try
			{
				int num;
				_ = num - 1;
				_ = 3;
				try
				{
					IQueryable<DepotReceiptDetail> queryable = ((IQueryable<DepotReceiptDetail>)c.DepotReceiptDetail).Where((DepotReceiptDetail x) => x.MasterGuid == master.Guid);
					if (queryable == null || !queryable.Any())
					{
						goto end_IL_0137;
					}
					object obj;
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IOrderedQueryable<DepotReceiptDetail> orderedQueryable = queryable.OrderBy((DepotReceiptDetail q) => q.Ordinal);
						obj = ((orderedQueryable != null) ? EntityFrameworkQueryableExtensions.ToListAsync<DepotReceiptDetail>((IQueryable<DepotReceiptDetail>)orderedQueryable, token) : null);
					}
					foreach (DepotReceiptDetail item in await (Task<List<DepotReceiptDetail>>)obj)
					{
						IItemAsyncMapper<DepotReceiptDetail, DepotReceiptDetailBusiness> mapFromDatabase = MapFromDatabase;
						string guid = base.Context.Guid;
						DepotReceiptDetail source = item;
						DepotReceiptDetailBusiness det = await mapFromDatabase.MapAsync(guid, source, await DepotReceiptDetailBusiness.GetFactoryAsync(base.ContextGuid, master, token, tranName), tranName, token);
						await master.AddToDetailListAsync(det);
					}
					goto end_IL_0120;
					end_IL_0137:;
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs", 48);
					throw;
				}
				return;
				end_IL_0120:;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\DepotReceipts\\DepotReceiptDetailPersistenceRepository.cs");
				}
			}
		}
	}
}
