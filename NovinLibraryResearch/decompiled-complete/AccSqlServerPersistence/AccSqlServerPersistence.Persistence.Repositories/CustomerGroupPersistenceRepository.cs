using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Model;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using Microsoft.EntityFrameworkCore;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CustomerGroupPersistenceRepository : PersistenceSyncRepository<CusGroupBusiness, CustomerGroup>, ICustomerGroupRepository, ISyncRepository<CusGroupBusiness>, IEditableDelivery, IRepository<CusGroupBusiness>, INoGuidRepository<CusGroupBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CusGroupBusiness, CustomerGroup>, IItemAsyncMapper<CustomerGroup, CusGroupBusiness>, IItemMapper<CustomerGroup, CusGroupBusiness>, IItemAsyncMapper<WebCusGroup, CustomerGroup>
	{
		public Task<CustomerGroup> MapAsync(string contextGuid, CusGroupBusiness source, CustomerGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
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
						destination = new CustomerGroup();
					}
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
					destination.TafsilGroupGuid = source.TafsilGroupGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 182);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CusGroupBusiness> MapAsync(string contextGuid, CustomerGroup source, CusGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public CusGroupBusiness Map(string contextGuid, CustomerGroup source, CusGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
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
						destination = new CusGroupBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
					destination.TafsilGroupGuid = source.TafsilGroupGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 220);
					throw;
				}
				return destination;
			}
		}

		public Task<List<CustomerGroup>> MapListAsync(string contextGuid, IEnumerable<CusGroupBusiness> source, List<CustomerGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CusGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerGroup> source, List<CusGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<CustomerGroup> MapAsync(string contextGuid, WebCusGroup source, CustomerGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
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
						destination = new CustomerGroup();
					}
					destination.Description = source.Description;
					destination.Name = source.Name;
					destination.Special = source.Special;
					destination.Status = source.Status;
					destination.MoeinDefaultGuid = source.MoeinDefaultGuid;
					destination.TafsilGroupGuid = source.TafsilGroupGuid;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
					destination.ServerStatus = source.ServerStatus;
					destination.DeliveryId = source.DeliveryId;
					destination.DeliveryMessage = source.DeliveryMessage;
					destination.DeliveryShortMessage = source.DeliveryShortMessage;
					destination.HostRefrenced = source.HostRefrenced;
					destination.DeliveryDate = source.DeliveryDate;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 259);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<CustomerGroup>> MapListAsync(string contextGuid, IEnumerable<WebCusGroup> source, List<CustomerGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CustomerGroup, CusGroupBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CusGroupBusiness, CustomerGroup> MapToDatabase => Mapper;

	public override IItemMapper<CustomerGroup, CusGroupBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	public CustomerGroupPersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<CusGroupBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
		{
			Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 34));
			CusGroupBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CustomerGroup source = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CustomerGroup>((IQueryable<CustomerGroup>)c.Customers_group, (Expression<Func<CustomerGroup, bool>>)((CustomerGroup x) => x.Name == name), token);
					result = await MapFromDatabase.MapAsync(ContextGuid, source, null, tranName, token);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 44);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public async Task<int> CountInGroupActiveAsync(Guid selectedGroupGuid, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("CountInGroupActiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
		{
			Acc_ModelContext acc_ModelContext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "CountInGroupActiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 56));
			int result;
			try
			{
				int num = ((IQueryable<Customer>)acc_ModelContext.Customers).Where((Customer p) => p.GroupGuid == selectedGroupGuid && p.Status).ToList().Count();
				result = num;
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
				WebErrorLog.Instance.StartLog(ex2, "", "CountInGroupActiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 66);
				throw;
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await acc_ModelContext.DisposeAsync("CountInGroupActiveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveAsync(CusGroupBusiness item, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				Acc_ModelContext dbcontext = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 84));
				item.Modified = DateTime.Now;
				CustomerGroup customerGroup = await MapToDatabase.MapAsync(ContextGuid, item, null, tranName, token);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await dbcontext.AddOrUpdateAsync(customerGroup, customerGroup.Guid), ReturnedState.None, "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 87);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 91);
				ret.AddReturnedValue(ex, "", "SaveAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 92);
			}
			return ret;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CusGroupBusiness> items, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			foreach (CusGroupBusiness item in items)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 105);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 109);
			ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 110);
		}
		return ret;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 126);
		}
	}

	public async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<WebCusGroup> items, CancellationToken token, string tranName)
	{
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (string.IsNullOrEmpty(tranName))
			{
				throw new ArgumentNullException("tranName");
			}
			try
			{
				Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 136));
				List<CustomerGroup> list = await Mapper.MapListAsync(ContextGuid, items, null, tranName, token);
				if (list == null || !list.Any())
				{
					return ret;
				}
				foreach (CustomerGroup item in list)
				{
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await c.AddOrUpdateAsync(item, token, item.Guid), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 140);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 144);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerGroupPersistenceRepository.cs", 145);
			}
			return ret;
		}
	}
}
