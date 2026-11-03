using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PrdVahedMapper : IItemAsyncMapper<PrdVahedBusiness, WebPrdVahed>, IItemAsyncMapper<PrdVahedBusiness, PrdVahedBusiness>, IItemAsyncMapper<WebPrdVahed, PrdVahedBusiness>
{
	public static PrdVahedMapper Instance { get; private set; } = new PrdVahedMapper();


	public Task<WebPrdVahed> MapAsync(string contextGuid, PrdVahedBusiness source, WebPrdVahed destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdVahed();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DecimalSales = source.DecimalSales;
			destination.TaxUnitGuid = source.TaxUnitGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdVahedMapper.cs", 42);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebPrdVahed>> MapListAsync(string contextGuid, IEnumerable<PrdVahedBusiness> source, List<WebPrdVahed> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PrdVahedBusiness> MapAsync(string contextGuid, PrdVahedBusiness source, PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdVahedBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DecimalSales = source.DecimalSales;
			destination.TaxUnitGuid = source.TaxUnitGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdVahedMapper.cs", 77);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PrdVahedBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdVahedBusiness> source, List<PrdVahedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PrdVahedBusiness> MapAsync(string contextGuid, WebPrdVahed source, PrdVahedBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdVahedBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DecimalSales = source.DecimalSales;
			destination.TaxUnitGuid = source.TaxUnitGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\PrdVahedMapper.cs", 111);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PrdVahedBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdVahed> source, List<PrdVahedBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
