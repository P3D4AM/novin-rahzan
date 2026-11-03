using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Customers;
using PacketParser.Entities;
using PacketParser.Entities.WordPress;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Customer;

public class CustomerRoleMapper : IItemAsyncMapper<WebCustomerRole, CustomerRoleBusiness>, IItemAsyncMapper<CustomerRoleBusiness, CustomerRoleBusiness>, IItemAsyncMapper<CustomerRoleBusiness, WebCustomerRole>
{
	public static CustomerRoleMapper Instance { get; private set; } = new CustomerRoleMapper();


	public Task<CustomerRoleBusiness> MapAsync(string contextGuid, WebCustomerRole source, CustomerRoleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.MetaData = source.MetaData;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerRoleMapper.cs", 45);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<CustomerRoleBusiness> MapAsync(string contextGuid, CustomerRoleBusiness source, CustomerRoleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
			destination.MetaData = source.MetaData;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerRoleMapper.cs", 77);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<WebCustomerRole> MapAsync(string contextGuid, CustomerRoleBusiness source, WebCustomerRole destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebCustomerRole();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.WordPressName = source.WordPressName;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.MetaData = source.MetaData;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Customer\\CustomerRoleMapper.cs", 109);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<CustomerRoleBusiness>> MapListAsync(string contextGuid, IEnumerable<WebCustomerRole> source, List<CustomerRoleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<CustomerRoleBusiness>> MapListAsync(string contextGuid, IEnumerable<CustomerRoleBusiness> source, List<CustomerRoleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebCustomerRole>> MapListAsync(string contextGuid, IEnumerable<CustomerRoleBusiness> source, List<WebCustomerRole> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
