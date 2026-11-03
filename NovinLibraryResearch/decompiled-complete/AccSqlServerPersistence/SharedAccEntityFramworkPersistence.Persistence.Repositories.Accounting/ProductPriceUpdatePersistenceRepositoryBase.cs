using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

internal abstract class ProductPriceUpdatePersistenceRepositoryBase : PersistenceRepository<ProductPriceUpdateBusiness, ProductPriceUpdate>, IProductPriceUpdateRepository, IRepository<ProductPriceUpdateBusiness>, INoGuidRepository<ProductPriceUpdateBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductPriceUpdateBusiness, ProductPriceUpdate>, IItemAsyncMapper<ProductPriceUpdate, ProductPriceUpdateBusiness>
	{
		public Task<ProductPriceUpdate> MapAsync(string contextGuid, ProductPriceUpdateBusiness source, ProductPriceUpdate destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductPriceUpdate();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ProductGuid = source.ProductGuid;
				destination.DateM = source.DateM;
				destination.Status = source.Status;
				destination.RegularPrice = source.RegularPrice;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs", 98);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ProductPriceUpdateBusiness> MapAsync(string contextGuid, ProductPriceUpdate source, ProductPriceUpdateBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductPriceUpdateBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ProductGuid = source.ProductGuid;
				destination.DateM = source.DateM;
				destination.Status = source.Status;
				destination.RegularPrice = source.RegularPrice;
				destination.ContextGuid = contextGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs", 125);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ProductPriceUpdate>> MapListAsync(string contextGuid, IEnumerable<ProductPriceUpdateBusiness> source, List<ProductPriceUpdate> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductPriceUpdateBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceUpdate> source, List<ProductPriceUpdateBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<ProductPriceUpdate, ProductPriceUpdateBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<ProductPriceUpdate, ProductPriceUpdateBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductPriceUpdateBusiness, ProductPriceUpdate> MapToDatabase => Mapper;

	public new bool IsDisposed { get; private set; }

	public override bool HasStatus => false;

	public override bool HasModified => true;

	internal ProductPriceUpdatePersistenceRepositoryBase(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs", 43);
		}
	}

	public virtual Task<ReturnedSaveFuncInfo> InsertBatchAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<IEnumerable<ProductPriceUpdateModel>> GetAllModelAsync(Guid productGuid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs", 48));
		IEnumerable<ProductPriceUpdateModel> result;
		try
		{
			_ = 1;
			try
			{
				DbSet<ProductPriceUpdate> productPriceUpdate = c.ProductPriceUpdate;
				object obj;
				if (productPriceUpdate == null)
				{
					obj = null;
				}
				else
				{
					IQueryable<ProductPriceUpdate> queryable = ((IQueryable<ProductPriceUpdate>)productPriceUpdate).Where((ProductPriceUpdate q) => q.ProductGuid == productGuid);
					if (queryable == null)
					{
						obj = null;
					}
					else
					{
						IQueryable<ProductPriceUpdateModel> queryable2 = queryable.Select((ProductPriceUpdate q) => new ProductPriceUpdateModel
						{
							DateM = q.DateM,
							Guid = q.Guid,
							ProductGuid = q.ProductGuid,
							RegularPrice = q.RegularPrice,
							ProductCode = q.Product.IdProduct,
							ProductName = q.Product.Name
						});
						obj = ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<ProductPriceUpdateModel>(queryable2, token) : null);
					}
				}
				result = await (Task<List<ProductPriceUpdateModel>>)obj;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs", 65);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductPriceUpdatePersistenceRepositoryBase.cs");
			}
		}
		return result;
	}
}
