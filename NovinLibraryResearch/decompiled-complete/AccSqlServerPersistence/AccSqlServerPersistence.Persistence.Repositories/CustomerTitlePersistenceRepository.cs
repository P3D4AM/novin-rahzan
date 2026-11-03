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

internal class CustomerTitlePersistenceRepository : PersistenceSyncRepository<CusTitleBusiness, CustomerTitle>, ICustomerTitleRepository, ISyncRepository<CusTitleBusiness>, IEditableDelivery, IRepository<CusTitleBusiness>, INoGuidRepository<CusTitleBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CusTitleBusiness, CustomerTitle>, IItemAsyncMapper<CustomerTitle, CusTitleBusiness>, IItemMapper<CustomerTitle, CusTitleBusiness>
	{
		public CusTitleBusiness Map(string contextGuid, CustomerTitle source, CusTitleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs"))
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
						destination = new CusTitleBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
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
					WebErrorLog.Instance.StartLog(ex2, "", "Map", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 114);
					throw;
				}
				return destination;
			}
		}

		public Task<CustomerTitle> MapAsync(string contextGuid, CusTitleBusiness source, CustomerTitle destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					destination = new CustomerTitle();
				}
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
				WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 144);
				throw;
			}
			return Task.FromResult(destination);
		}

		public Task<CusTitleBusiness> MapAsync(string contextGuid, CustomerTitle source, CusTitleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return Task.FromResult(Map(contextGuid, source, destination, tranName));
		}

		public Task<List<CustomerTitle>> MapListAsync(string contextGuid, IEnumerable<CusTitleBusiness> source, List<CustomerTitle> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CusTitleBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerTitle> source, List<CusTitleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CustomerTitle, CusTitleBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CusTitleBusiness, CustomerTitle> MapToDatabase => Mapper;

	public override IItemMapper<CustomerTitle, CusTitleBusiness> MapFromDatabaseNoneAsync => Mapper;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	internal CustomerTitlePersistenceRepository(ContextHolder db)
		: base(db)
	{
	}

	public async Task<CusTitleBusiness> GetAsync(string name, CancellationToken token, string tranName)
	{
		Acc_ModelContext c = (Acc_ModelContext)(await dbContext.GetContextAsync(tranName, 60000, "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 33));
		using (new JaegerMonitor("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs"))
		{
			CusTitleBusiness result;
			try
			{
				int num;
				_ = num - 1;
				_ = 1;
				try
				{
					CustomerTitle customerTitle = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<CustomerTitle>((IQueryable<CustomerTitle>)c.CustomersTitle, (Expression<Func<CustomerTitle, bool>>)((CustomerTitle x) => x.Name == name), token);
					result = ((customerTitle != null) ? (await MapFromDatabase.MapAsync(ContextGuid, customerTitle, null, tranName, token)) : null);
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
					WebErrorLog.Instance.StartLog(ex2, "", "GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 45);
					throw;
				}
			}
			finally
			{
				if (string.IsNullOrEmpty(tranName))
				{
					await c.DisposeAsync("GetAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs");
				}
			}
			return result;
		}
	}

	public override async Task<ReturnedSaveFuncInfo> SaveRangeAsync(IEnumerable<CusTitleBusiness> lst, short tryCount, CancellationToken token, string tranName)
	{
		if (string.IsNullOrEmpty(tranName))
		{
			throw new ArgumentNullException("tranName");
		}
		using (new JaegerMonitor("SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (lst != null && lst.Any())
				{
					foreach (CusTitleBusiness item in lst)
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SaveAsync(item, tryCount, token, tranName), ReturnedState.None, "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 64);
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 68);
				ret.AddReturnedValue(ex, "", "SaveRangeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 69);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerTitlePersistenceRepository.cs", 82);
		}
	}
}
