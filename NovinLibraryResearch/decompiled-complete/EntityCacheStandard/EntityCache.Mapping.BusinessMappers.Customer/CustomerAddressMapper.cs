using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Customer;

public class CustomerAddressMapper : IItemAsyncMapper<CustomerAddressBusiness, WebCustomerAddress>, IItemAsyncMapper<CustomerAddressBusiness, CustomerAddressBusiness>, IItemAsyncMapper<WebCustomerAddress, CustomerAddressBusiness>
{
	public static CustomerAddressMapper Instance { get; private set; } = new CustomerAddressMapper();


	public Task<WebCustomerAddress> MapAsync(string contextGuid, CustomerAddressBusiness source, WebCustomerAddress destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCustomerAddress();
			}
			destination.Id = source.Id;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerAddressMapper.cs", 42);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebCustomerAddress>> MapListAsync(string contextGuid, IEnumerable<CustomerAddressBusiness> source, List<WebCustomerAddress> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CustomerAddressBusiness> MapAsync(string contextGuid, CustomerAddressBusiness source, CustomerAddressBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.Id = source.Id;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerAddressMapper.cs", 76);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CustomerAddressBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerAddressBusiness> source, List<CustomerAddressBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<CustomerAddressBusiness> MapAsync(string contextGuid, WebCustomerAddress source, CustomerAddressBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.Id = source.Id;
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
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerAddressMapper.cs", 111);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CustomerAddressBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCustomerAddress> source, List<CustomerAddressBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
