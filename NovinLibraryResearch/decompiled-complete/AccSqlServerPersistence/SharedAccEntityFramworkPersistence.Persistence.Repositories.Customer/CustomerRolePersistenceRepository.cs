using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence;
using AccSqlServerPersistence.Model;
using AccSqlServerPersistence.Persistence;
using EntityCache.Business.Customers;
using EntityCache.Core;
using EntityCache.Core.Repository;
using EntityCache.Core.Repository.Customer;
using EntityCache.Models.Accounting;
using Microsoft.EntityFrameworkCore;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Customer;

namespace SharedAccEntityFramworkPersistence.Persistence.Repositories.Customer;

internal class CustomerRolePersistenceRepository : PersistenceSyncRepository<CustomerRoleBusiness, CustomerRole>, ICustomerRoleRepository, ISyncRepository<CustomerRoleBusiness>, IEditableDelivery, IRepository<CustomerRoleBusiness>, INoGuidRepository<CustomerRoleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CustomerRole, CustomerRoleBusiness>, IItemAsyncMapper<CustomerRoleBusiness, CustomerRole>
	{
		public Task<CustomerRoleBusiness> MapAsync(string contextGuid, CustomerRole source, CustomerRoleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CustomerRoleBusiness(contextGuid);
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name;
				destination.WordPressName = source.WordPressName;
				destination.PriceTypeGuid = source.PriceTypeGuid;
				destination.Status = source.Status;
				destination.Description = source.Description;
				destination.MetaData = source.MetaData.FromJson<Dictionary<string, string>>("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 121);
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage;
				destination.DeliveryShortMessage = source.DeliveryShortMessage;
				destination.HostRefrenced = source.HostRefrenced;
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 134);
				throw;
			}
		}

		public Task<CustomerRole> MapAsync(string contextGuid, CustomerRoleBusiness source, CustomerRole destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CustomerRole();
				}
				destination.Guid = source.Guid;
				destination.Modified = source.Modified;
				destination.Name = source.Name ?? "";
				destination.WordPressName = source.WordPressName ?? "";
				destination.PriceTypeGuid = source.PriceTypeGuid;
				destination.Status = source.Status;
				destination.Description = source.Description ?? "";
				destination.MetaData = Json.ToStringJson(source.MetaData) ?? "";
				destination.ServerStatus = source.ServerStatus;
				destination.DeliveryDate = source.DeliveryDate;
				destination.DeliveryId = source.DeliveryId;
				destination.DeliveryMessage = source.DeliveryMessage ?? "";
				destination.DeliveryShortMessage = source.DeliveryShortMessage ?? "";
				destination.HostRefrenced = source.HostRefrenced ?? "";
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 168);
				throw;
			}
		}

		public Task<List<CustomerRoleBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerRole> source, List<CustomerRoleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CustomerRole>> MapListAsync(string contextGuid, IEnumerable<CustomerRoleBusiness> source, List<CustomerRole> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemMapper<CustomerRole, CustomerRoleBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public override IItemAsyncMapper<CustomerRole, CustomerRoleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CustomerRoleBusiness, CustomerRole> MapToDatabase => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public CustomerRolePersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 42);
		}
	}

	public async Task<CustomerRoleBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 46));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs"))
		{
			CustomerRoleBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CustomerRole customerRole = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CustomerRole>((IQueryable<CustomerRole>)c.CustomerRole, (Expression<Func<CustomerRole, bool>>)((CustomerRole x) => x.Name == name), token);
					if (customerRole == null)
					{
						result = null;
					}
					else
					{
						CustomerRoleBusiness customerRoleBusiness = await MapFromDatabase.MapAsync(ContextGuid, customerRole, null, tranName, token);
						customerRoleBusiness.ChangeStatus = EntityChangeSt.Edited;
						result = customerRoleBusiness;
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 60);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<IEnumerable<CustomerRoleModel>> GetAllModelAsync(bool? status, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 71));
		using (new JaegerMonitor("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs"))
		{
			IEnumerable<CustomerRoleModel> result;
			try
			{
				_ = 1;
				try
				{
					IQueryable<CustomerRole> queryable = (IQueryable<CustomerRole>)EntityFrameworkQueryableExtensions.Include<CustomerRole, ProductPriceLevel>((IQueryable<CustomerRole>)c.CustomerRole, (Expression<Func<CustomerRole, ProductPriceLevel>>)((CustomerRole q) => q.ProductPriceLevel));
					if (status.HasValue)
					{
						queryable = queryable?.Where((CustomerRole q) => q.Status == ((bool?)status).Value);
					}
					if (queryable == null || !queryable.Any())
					{
						result = null;
					}
					else
					{
						IQueryable<CustomerRoleModel> queryable2 = queryable.Select((CustomerRole q) => new CustomerRoleModel
						{
							Description = q.Description,
							Guid = q.Guid,
							Name = q.Name,
							WordPressName = q.WordPressName,
							PriceLevelName = q.ProductPriceLevel.Name,
							ServerStatus = q.ServerStatus
						});
						result = await ((queryable2 != null) ? EntityFrameworkQueryableExtensions.ToListAsync<CustomerRoleModel>(queryable2, token) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs", 92);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAllModelAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerRolePersistenceRepository.cs");
				}
			}
			return result;
		}
	}
}
