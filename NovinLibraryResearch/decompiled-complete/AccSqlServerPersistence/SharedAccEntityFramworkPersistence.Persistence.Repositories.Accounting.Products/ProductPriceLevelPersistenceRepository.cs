using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache;
using EntityCache.Business.Products;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;
using PacketParser.Mapping;
using PacketParser.Records.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Accounting.Products;

internal class ProductPriceLevelPersistenceRepository : PersistenceSyncRepository<ProductPriceLevelBusiness, ProductPriceLevel>, IProductPriceLevelRepository, ISyncRepository<ProductPriceLevelBusiness>, IEditableDelivery, IRepository<ProductPriceLevelBusiness>, INoGuidRepository<ProductPriceLevelBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<ProductPriceLevelBusiness, ProductPriceLevel>, IItemAsyncMapper<ProductPriceLevel, ProductPriceLevelBusiness>
	{
		public Task<ProductPriceLevel> MapAsync(string contextGuid, ProductPriceLevelBusiness source, ProductPriceLevel destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductPriceLevel();
				}
				destination.Guid = source.Guid;
				destination.Name = source.Name.Trim() ?? "";
				destination.Description = source.Description ?? "";
				destination.Status = source.Status;
				destination.Ordinal = source.Ordinal;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.Type = source.Type;
				destination.DiscountCoefficient = source.DiscountCoefficient;
				destination.IsSystem = source.IsSystem;
				destination.IsDefault = source.IsDefault;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 209);
				throw;
			}
		}

		public Task<ProductPriceLevelBusiness> MapAsync(string contextGuid, ProductPriceLevel source, ProductPriceLevelBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new ProductPriceLevelBusiness(contextGuid);
				}
				destination.ContextGuid = contextGuid;
				destination.Guid = source.Guid;
				destination.Name = source.Name;
				destination.Description = source.Description;
				destination.Status = source.Status;
				destination.Ordinal = source.Ordinal;
				destination.Modified = source.Modified;
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
				destination.ChangeStatus = EntityChangeSt.Edited;
				destination.Type = source.Type;
				destination.DiscountCoefficient = source.DiscountCoefficient;
				destination.IsSystem = source.IsSystem;
				destination.IsDefault = source.IsDefault;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 247);
				throw;
			}
		}

		public Task<List<ProductPriceLevel>> MapListAsync(string contextGuid, IEnumerable<ProductPriceLevelBusiness> source, List<ProductPriceLevel> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<ProductPriceLevelBusiness>> MapListAsync(string contextGuid, IEnumerable<ProductPriceLevel> source, List<ProductPriceLevelBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<ProductPriceLevel, ProductPriceLevelBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<ProductPriceLevel, ProductPriceLevelBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<ProductPriceLevelBusiness, ProductPriceLevel> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public ProductPriceLevelPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 46);
		}
	}

	public async Task<ProductPriceLevelBusiness> GetAsync(string name, EnPriceLevelType type, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 50));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs"))
		{
			ProductPriceLevelBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					ProductPriceLevel productPriceLevel = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductPriceLevel>((IQueryable<ProductPriceLevel>)c.ProductPriceLevel, (Expression<Func<ProductPriceLevel, bool>>)((ProductPriceLevel x) => x.Name == name.FixString() && x.Type == type), token);
					result = ((productPriceLevel != null) ? (await MapFromDatabase.MapAsync(ContextGuid, productPriceLevel, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 62);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<short> GetMaxOrdinalAsync(CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetMaxOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 73));
		using (new JaegerMonitor("GetMaxOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs"))
		{
			short result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<ProductPriceLevel> queryable = ((IQueryable<ProductPriceLevel>)c.ProductPriceLevel).OrderByDescending((ProductPriceLevel x) => x.Ordinal).Take(1);
					ProductPriceLevel productPriceLevel = await ((queryable != null) ? EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<ProductPriceLevel>(queryable, token) : null);
					result = (short)((productPriceLevel == null) ? 1 : ((short)(productPriceLevel.Ordinal + 1)));
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetMaxOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 85);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetMaxOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<ReturnedSaveFuncInfo> ChangeOrdinalAsync(List<ChangeOrdinalRecord> list, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (!(list?.Any() ?? false))
			{
				return res;
			}
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 100));
			List<ProductPriceLevel> changedList = new List<ProductPriceLevel>();
			short maxOrdinal = await GetMaxOrdinalAsync(token, tranName);
			foreach (ChangeOrdinalRecord item in list)
			{
				ProductPriceLevel productPriceLevel = await c.ProductPriceLevel.FindAsync(new object[1] { item.Guid });
				if (productPriceLevel != null)
				{
					productPriceLevel.Ordinal = maxOrdinal++;
					changedList.Add(productPriceLevel);
				}
			}
			if (!(changedList?.Any() ?? false))
			{
				return res;
			}
			((DbContext)c).UpdateRange((IEnumerable<object>)changedList);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
			returnedSaveFuncInfo.AddReturnedValue(await ClsCache.TransactionDestiny(ContextGuid, tranName, res.HasError, disposeContext: false, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 115), ReturnedState.None, "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 115);
			changedList = new List<ProductPriceLevel>();
			foreach (ChangeOrdinalRecord record in list)
			{
				ProductPriceLevel productPriceLevel2 = await c.ProductPriceLevel.FindAsync(new object[1] { record.Guid });
				if (productPriceLevel2 != null)
				{
					productPriceLevel2.Ordinal = record.Ordinal;
					productPriceLevel2.ServerStatus = ServerStatusForClient.None;
					productPriceLevel2.Modified = DateTime.Now;
					changedList.Add(productPriceLevel2);
				}
			}
			if (!(changedList?.Any() ?? false))
			{
				return res;
			}
			((DbContext)c).UpdateRange((IEnumerable<object>)changedList);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 134);
			res.AddReturnedValue(ex, "", "ChangeOrdinalAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 135);
		}
		return res;
	}

	public async Task<IEnumerable<ProductPriceLevelBusiness>> GetAllAsync(ProductPriceLevelFilter filter, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 141));
		IEnumerable<ProductPriceLevelBusiness> result;
		try
		{
			int num;
			_ = num - 1;
			_ = 1;
			try
			{
				IQueryable<ProductPriceLevel> queryable = (IQueryable<ProductPriceLevel>)c.ProductPriceLevel;
				if (filter != null)
				{
					if (filter.Status.HasValue)
					{
						queryable = queryable?.Where((ProductPriceLevel q) => q.Status == filter.Status.Value);
					}
					if (!filter.IsLoadSystem)
					{
						queryable = queryable?.Where((ProductPriceLevel q) => !q.IsSystem);
					}
					if (filter.Type.HasValue)
					{
						queryable = queryable?.Where((ProductPriceLevel q) => q.Type == filter.Type.Value);
					}
				}
				if (queryable == null || !queryable.Any())
				{
					result = null;
				}
				else
				{
					IItemAsyncMapper<ProductPriceLevel, ProductPriceLevelBusiness> mapFromDatabase = MapFromDatabase;
					string contextGuid = ContextGuid;
					result = await mapFromDatabase.MapListAsync(contextGuid, await EntityFrameworkQueryableExtensions.ToListAsync<ProductPriceLevel>(queryable, token), null, tranName, token);
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (SqlException val)
			{
				SqlException val2 = val;
				if (((Exception)(object)val2).Message.Contains("Operation cancelled by user"))
				{
					throw new OperationCanceledException();
				}
				WebErrorLog.Instance.StartLog((Exception)(object)val2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 159);
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs", 165);
				throw;
			}
		}
		finally
		{
			if (string.IsNullOrEmpty(tranName))
			{
				await c.DisposeAsync("GetAllAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Accounting\\Products\\ProductPriceLevelPersistenceRepository.cs");
			}
		}
		return result;
	}
}
