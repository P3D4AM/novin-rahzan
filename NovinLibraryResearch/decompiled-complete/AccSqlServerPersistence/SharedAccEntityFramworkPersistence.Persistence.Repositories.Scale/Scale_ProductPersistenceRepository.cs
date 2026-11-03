using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Scale;
using EntityCache.Core;
using EntityCache.Core.Repository.Scale;
using PacketParser.Entities;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Scale;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Scale;

internal class Scale_ProductPersistenceRepository : PersistenceRepository<Scale_ProductBusiness, Scale_Product>, IScale_ProductRepository, IRepository<Scale_ProductBusiness>, INoGuidRepository<Scale_ProductBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<Scale_Product, Scale_ProductBusiness>, IItemAsyncMapper<Scale_ProductBusiness, Scale_Product>
	{
		public Task<Scale_ProductBusiness> MapAsync(string contextGuid, Scale_Product source, Scale_ProductBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_ProductBusiness(contextGuid);
				}
				destination.Name = source.Name;
				destination.Code = source.Code;
				destination.GroupGuid = source.GroupGuid;
				destination.VahedGuid = source.VahedGuid;
				destination.IsWeight = source.IsWeight;
				destination.Price = source.Price;
				destination.Barcode = source.Barcode;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ProductPersistenceRepository.cs", 76);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<Scale_Product> MapAsync(string contextGuid, Scale_ProductBusiness source, Scale_Product destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new Scale_Product();
				}
				destination.Name = source.Name;
				destination.Code = source.Code;
				destination.GroupGuid = source.GroupGuid;
				destination.VahedGuid = source.VahedGuid;
				destination.IsWeight = source.IsWeight;
				destination.Price = source.Price;
				destination.Barcode = source.Barcode;
				destination.Modified = source.Modified;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Description = source.Description;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ProductPersistenceRepository.cs", 108);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<List<Scale_ProductBusiness>> MapListAsync(string contextGuid, IEnumerable<Scale_Product> source, List<Scale_ProductBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<Scale_Product>> MapListAsync(string contextGuid, IEnumerable<Scale_ProductBusiness> source, List<Scale_Product> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<Scale_Product, Scale_ProductBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<Scale_Product, Scale_ProductBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<Scale_ProductBusiness, Scale_Product> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public Scale_ProductPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Scale\\Scale_ProductPersistenceRepository.cs", 44);
		}
	}
}
