using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class ProductVahedPersistenceRepository : PersistenceSyncRepository<PrdVahedBusiness, ProductVahed>, IProductVahedRepository, ISyncRepository<PrdVahedBusiness>, IEditableDelivery, IRepository<PrdVahedBusiness>, INoGuidRepository<PrdVahedBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<PrdVahedBusiness, ProductVahed>, IItemAsyncMapper<ProductVahed, PrdVahedBusiness>, IItemMapper<ProductVahed, PrdVahedBusiness>
	{
		public PrdVahedBusiness Map(string contextGuid, ProductVahed source, PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs"))
			{
				try
				{
					if (source == null)
					{
						destination = null;
						return destination;
					}
					if (destination == null)
					{
						destination = new PrdVahedBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Status = source.Status;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
					destination.DecimalSales = source.DecimalSales;
					destination.TaxUnitGuid = source.TaxUnitGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 113);
					throw;
				}
				return destination;
			}
		}

		public Task<ProductVahed> MapAsync(string contextGuid, PrdVahedBusiness source, ProductVahed destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductVahed();
				}
				destination.Description = source.Description ?? "";
				destination.Name = source.Name;
				destination.Status = source.Status;
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DecimalSales = source.DecimalSales;
				destination.TaxUnitGuid = source.TaxUnitGuid;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 147);
				throw;
			}
		}

		public Task<PrdVahedBusiness> MapAsync(string contextGuid, ProductVahed source, PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName, token));
		}

		public Task<List<ProductVahed>> MapListAsync(string contextGuid, IEnumerable<PrdVahedBusiness> source, List<ProductVahed> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<PrdVahedBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductVahed> source, List<PrdVahedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public override IItemAsyncMapper<ProductVahed, PrdVahedBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<PrdVahedBusiness, ProductVahed> MapToDatabase => Mapper;

	public override IItemMapper<ProductVahed, PrdVahedBusiness> MapFromDatabaseNoneAsync => Mapper;

	public ProductVahedPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<PrdVahedBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 32));
		PrdVahedBusiness result;
		try
		{
			_ = 1;
			try
			{
				ProductVahed productVahed = ((IQueryable<ProductVahed>)c.Products_vahed).FirstOrDefault((ProductVahed p) => p.Name == name);
				PrdVahedBusiness prdVahedBusiness = ((productVahed != null) ? (await MapFromDatabase.MapAsync(ContextGuid, productVahed, null, tranName, token)) : null);
				result = prdVahedBusiness;
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
				WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 42);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs");
			}
		}
		return result;
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<PrdVahedBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				foreach (PrdVahedBusiness item in items)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 60);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 64);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 65);
			}
			return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\ProductVahedPersistenceRepository.cs", 78);
		}
	}
}
