using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Accounting.ProductAttrebutes;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.ProductAttributes;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.ProductAttributes;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.ProductAttributes;

internal class ProductAttributeValuePersistenceRepository : PersistenceMasterRepository<ProductAttributeValueBusiness, ProductAttributeValue>, IProductAttributeValueRepository, IPersistenceMasterRepository<ProductAttributeValueBusiness>, IDisposable
{
	private class Mapper : IItemAsyncMapper<ProductAttributeValueBusiness, ProductAttributeValue>, IItemAsyncMapper<ProductAttributeValue, ProductAttributeValueBusiness>
	{
		public Task<ProductAttributeValue> MapAsync(string contextGuid, ProductAttributeValueBusiness source, ProductAttributeValue destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductAttributeValue();
				}
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeValuePersistenceRepository.cs", 63);
				throw;
			}
		}

		public Task<ProductAttributeValueBusiness> MapAsync(string contextGuid, ProductAttributeValue source, ProductAttributeValueBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductAttributeValueBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.MasterGuid = source.MasterGuid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.Status = source.Status;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeValuePersistenceRepository.cs", 90);
				throw;
			}
		}

		public Task<List<ProductAttributeValue>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeValueBusiness> source, List<ProductAttributeValue> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductAttributeValueBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductAttributeValue> source, List<ProductAttributeValueBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private Mapper mapper { get; set; } = new Mapper();


	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ProductAttributeValue, ProductAttributeValueBusiness> MapFromDatabase => mapper;

	public override IItemAsyncMapper<ProductAttributeValueBusiness, ProductAttributeValue> MapToDatabase => mapper;

	public ProductAttributeValuePersistenceRepository(ContextHolder dbContext)
		: base(dbContext)
	{
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\ProductAttributes\\ProductAttributeValuePersistenceRepository.cs", 36);
		}
	}
}
