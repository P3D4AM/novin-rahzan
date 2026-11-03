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
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting;

internal abstract class ProductDecimalRelationPersistenceBaseRepository : PersistenceNoGuidRepository<ProductDecimalRelationBusiness, ProductDecimalRelation>, IProductDecimalRelationRepository, INoGuidRepository<ProductDecimalRelationBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductDecimalRelationBusiness, ProductDecimalRelation>, IItemAsyncMapper<ProductDecimalRelation, ProductDecimalRelationBusiness>
	{
		public Task<ProductDecimalRelationBusiness> MapAsync(string contextGuid, ProductDecimalRelation source, ProductDecimalRelationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductDecimalRelationBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.ProductGuid = source.ProductGuid;
				destination.DecimalGuid = source.DecimalGuid;
				destination.Amount = source.Amount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 84);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<ProductDecimalRelation> MapAsync(string contextGuid, ProductDecimalRelationBusiness source, ProductDecimalRelation destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductDecimalRelation();
				}
				destination.ProductGuid = source.ProductGuid;
				destination.DecimalGuid = source.DecimalGuid;
				destination.Amount = source.Amount;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 107);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<ProductDecimalRelationBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductDecimalRelation> source, List<ProductDecimalRelationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductDecimalRelation>> MapListAsync(string contextGuid, IEnumerable<ProductDecimalRelationBusiness> source, List<ProductDecimalRelation> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private bool _isDisposed;

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<ProductDecimalRelation, ProductDecimalRelationBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductDecimalRelationBusiness, ProductDecimalRelation> MapToDatabase => Mapper;

	public ProductDecimalRelationPersistenceBaseRepository(ContextHolder db)
		: base(db)
	{
	}

	public override void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				Mapper = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 38);
		}
	}

	public override Task<ReturnedSaveFuncInfo> SaveAsync(ProductDecimalRelationBusiness item, short TryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<ProductDecimalRelationBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		throw new NotImplementedException();
	}

	public async Task<ReturnedSaveFuncInfo> RemoveRangeAsync(CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 49));
				List<ProductDecimalRelation> list = await EntityFrameworkQueryableExtensions.ToListAsync<ProductDecimalRelation>((IQueryable<ProductDecimalRelation>)dbcontext.ProductPrices, token);
				if (list != null && list.Any())
				{
					dbcontext.ProductPrices.RemoveRange((IEnumerable<ProductDecimalRelation>)list);
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 57);
				ret.AddReturnedValue(ex2, "", "RemoveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductDecimalRelationPersistenceRepository.cs", 58);
			}
			return ret;
		}
	}
}
