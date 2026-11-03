using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Model;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.ProductAttrebutes;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;
using SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.ProductAttributes;

namespace AccSqlServerPersistence.Persistence.Repositories.Accounting;

internal class ProductAttributeBasePersistenceRepository : PersistenceRepository<ProductAttributeBusiness, ProductAttribute>, IProductAttributeRepository, IRepository<ProductAttributeBusiness>, INoGuidRepository<ProductAttributeBusiness>, IDisposable
{
	private class Mapper : IItemAsyncMapper<ProductAttributeBusiness, ProductAttribute>, IItemAsyncMapper<ProductAttribute, ProductAttributeBusiness>
	{
		public Task<ProductAttribute> MapAsync(string contextGuid, ProductAttributeBusiness source, ProductAttribute destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductAttribute();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.Ordinal = source.Ordinal;
				destination.Slug = source.Slug;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 213);
				throw;
			}
		}

		public Task<ProductAttributeBusiness> MapAsync(string contextGuid, ProductAttribute source, ProductAttributeBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductAttributeBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Ordinal = source.Ordinal;
				destination.Status = source.Status;
				destination.Slug = source.Slug;
				destination.ChangeStatus = EntityChangeSt.Edited;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 241);
				throw;
			}
		}

		public Task<List<ProductAttribute>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeBusiness> source, List<ProductAttribute> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductAttributeBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttribute> source, List<ProductAttributeBusiness> destination, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private ProductAttributeValuePersistenceRepository _detRepo;

	private Mapper mapper { get; set; } = new Mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ProductAttribute, ProductAttributeBusiness> MapFromDatabase => mapper;

	public override IItemAsyncMapper<ProductAttributeBusiness, ProductAttribute> MapToDatabase => mapper;

	public override IItemMapper<ProductAttribute, ProductAttributeBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ProductAttributeBasePersistenceRepository(ContextHolder db)
		: base(db)
	{
		_detRepo = new ProductAttributeValuePersistenceRepository(db);
	}

	public override void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 43);
		}
	}

	public override async Task<ProductAttributeBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 47));
		ProductAttributeBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				ProductAttribute productAttribute = await c.ProductAttributes.FindAsync(new object[1] { guid });
				if (productAttribute == null)
				{
					result = null;
				}
				else
				{
					ProductAttributeBusiness mappedData = await MapFromDatabase.MapAsync(ContextGuid, productAttribute, null, tranName, token);
					List<ProductAttributeValueBusiness> list = await _detRepo.GetAllAsync(guid, token, tranName);
					if (list != null && list.Any())
					{
						await mappedData.AddDetailListAsync(list, token);
					}
					result = mappedData;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 63);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public async Task<ProductAttributeBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 74));
		ProductAttributeBusiness result;
		try
		{
			int num;
			_ = num - 1;
			_ = 3;
			try
			{
				ProductAttribute obj = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductAttribute>((IQueryable<ProductAttribute>)c.ProductAttributes, (Expression<Func<ProductAttribute, bool>>)((ProductAttribute q) => q.Name == name), token);
				if (obj == null)
				{
					result = null;
				}
				else
				{
					ProductAttributeBusiness mappedData = await MapFromDatabase.MapAsync(ContextGuid, obj, null, tranName, token);
					List<ProductAttributeValueBusiness> list = await _detRepo.GetAllAsync(obj.Guid, token, tranName);
					if (list != null && list.Any())
					{
						await mappedData.AddDetailListAsync(list, token);
					}
					result = mappedData;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 90);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(ProductAttributeBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs"))
		{
			_ = 3;
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 106));
				ProductAttribute productAttribute = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(productAttribute, productAttribute.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 108);
				if (res.HasError)
				{
					return res;
				}
				IQueryable<ProductAttributeValue> queryable = ((IQueryable<ProductAttributeValue>)dbcontext.ProductAttributeValue).Where((ProductAttributeValue q) => q.MasterGuid == item.Guid);
				if (queryable != null && queryable.Any())
				{
					List<Guid> existGuidList = item.DetailList?.Select((ProductAttributeValueBusiness q) => q.Guid)?.ToList();
					if (existGuidList?.Any() ?? false)
					{
						IQueryable<ProductAttributeValue> queryable2 = queryable.Where((ProductAttributeValue q) => !existGuidList.Contains(q.Guid));
						if (queryable2 != null && queryable2.Any())
						{
							dbcontext.ProductAttributeValue.RemoveRange((IEnumerable<ProductAttributeValue>)queryable2);
						}
					}
				}
				if (res.HasError || !(item.DetailList?.Any() ?? false))
				{
					return res;
				}
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await _detRepo.SaveAsync(item.DetailList?.ToList(), item.Guid, tryCount, token, tranName), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 122);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 127);
				res.AddReturnedValue(ex2, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 128);
			}
			return res;
		}
	}

	public async Task<IEnumerable<ProductAttributeReportBusiness>> GetAllModelAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 134));
		using (new JaegerMonitor("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs"))
		{
			IEnumerable<ProductAttributeReportBusiness> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<ProductAttribute> queryable = (IQueryable<ProductAttribute>)c.ProductAttributes;
					if (status.HasValue)
					{
						queryable = queryable?.Where((ProductAttribute q) => q.Status == ((bool?)status).Value);
					}
					result = ((queryable != null && queryable.Any()) ? (await EntityFrameworkQueryableExtensions.ToListAsync<ProductAttributeReportBusiness>(queryable.Select((ProductAttribute q) => new ProductAttributeReportBusiness
					{
						Guid = q.Guid,
						Status = q.Status,
						Modified = q.Modified,
						Name = q.Name,
						Ordinal = q.Ordinal,
						Slug = q.Slug
					}), token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 155);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public virtual Task<IEnumerable<ProductAttributeValueReportBusiness>> GetAllValueAsync(CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<short> GetNextOrdinalAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 167));
		using (new JaegerMonitor("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs"))
		{
			short result;
			try
			{
				_ = 1;
				try
				{
					result = (short)(((await EntityFrameworkQueryableExtensions.SingleOrDefaultAsync<ProductAttribute>(((IQueryable<ProductAttribute>)c.ProductAttributes).OrderByDescending((ProductAttribute x) => x.Ordinal).Take(1), token))?.Ordinal ?? 0) + 1);
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
					WebErrorLog.Instance.StartLog(ex3, "", "GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs", 179);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetNextOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeBasePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
