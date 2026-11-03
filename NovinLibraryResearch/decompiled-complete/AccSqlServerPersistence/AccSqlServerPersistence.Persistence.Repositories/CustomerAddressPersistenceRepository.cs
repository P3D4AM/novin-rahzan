using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccSqlServerPersistence.Entities;
using EntityCache.Business;
using EntityCache.Core;
using EntityCache.Core.Repository;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace AccSqlServerPersistence.Persistence.Repositories;

internal class CustomerAddressPersistenceRepository : PersistenceRepository<CustomerAddressBusiness, CustomerAddress>, ICustomerAddressRepository, IRepository<CustomerAddressBusiness>, INoGuidRepository<CustomerAddressBusiness>, IDisposable
{
	private class mapper : IItemAsyncMapper<CustomerAddressBusiness, CustomerAddress>, IItemAsyncMapper<CustomerAddress, CustomerAddressBusiness>
	{
		public Task<CustomerAddress> MapAsync(string contextGuid, CustomerAddressBusiness source, CustomerAddress destination, string tranName, CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerAddressPersistenceRepository.cs"))
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
						destination = new CustomerAddress();
					}
					destination.State = source.State;
					destination.City = source.City;
					destination.Address = source.Address;
					destination.PostalCode = source.PostalCode;
					destination.FullName = source.FullName;
					destination.Phone = source.Phone;
					destination.CellPhone = source.CellPhone;
					destination.NationalCode = source.NationalCode;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerAddressPersistenceRepository.cs", 73);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<CustomerAddressBusiness> MapAsync(string contextGuid, CustomerAddress source, CustomerAddressBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			using (new JaegerMonitor("MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerAddressPersistenceRepository.cs"))
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
						destination = new CustomerAddressBusiness(contextGuid);
					}
					destination.ContextGuid = contextGuid;
					destination.State = source.State;
					destination.City = source.City;
					destination.Address = source.Address;
					destination.PostalCode = source.PostalCode;
					destination.FullName = source.FullName;
					destination.Phone = source.Phone;
					destination.CellPhone = source.CellPhone;
					destination.NationalCode = source.NationalCode;
					destination.Guid = source.Guid;
					destination.Modified = source.Modified;
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
					WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerAddressPersistenceRepository.cs", 106);
					throw;
				}
				return Task.FromResult(destination);
			}
		}

		public Task<List<CustomerAddress>> MapListAsync(string contextGuid, IEnumerable<CustomerAddressBusiness> source, List<CustomerAddress> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}

		public Task<List<CustomerAddressBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerAddress> source, List<CustomerAddressBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
		{
			return source.MapListAsync(contextGuid, this, destination, tranName, 20);
		}
	}

	public override bool HasStatus => true;

	public override bool HasModified => true;

	public new bool IsDisposed { get; private set; }

	private mapper Mapper { get; set; } = new mapper();


	public override IItemAsyncMapper<CustomerAddress, CustomerAddressBusiness> MapFromDatabase => Mapper;

	public override IItemAsyncMapper<CustomerAddressBusiness, CustomerAddress> MapToDatabase => Mapper;

	public override IItemMapper<CustomerAddress, CustomerAddressBusiness> MapFromDatabaseNoneAsync
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CustomerAddressPersistenceRepository(ContextHolder db)
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Persistence\\Repositories\\Customer\\CustomerAddressPersistenceRepository.cs", 33);
		}
	}
}
