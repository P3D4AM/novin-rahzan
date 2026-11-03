using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class SanadPrintMapper : IItemAsyncMapper<SanadPrintBusiness, WebSanadPrint>, IItemAsyncMapper<SanadPrintBusiness, SanadPrintBusiness>, IItemAsyncMapper<WebSanadPrint, SanadPrintBusiness>
{
	public static SanadPrintMapper Instance { get; private set; } = new SanadPrintMapper();


	public Task<WebSanadPrint> MapAsync(string contextGuid, SanadPrintBusiness source, WebSanadPrint destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebSanadPrint();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Tel = source.Tel;
			destination.Address = source.Address;
			destination.Mob = source.Mob;
			destination.MobileLong = source.MobileLong;
			destination.DateM = source.DateM;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadPrintMapper.cs", 49);
			throw;
		}
	}

	public Task<SanadPrintBusiness> MapAsync(string contextGuid, SanadPrintBusiness source, SanadPrintBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadPrintBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Tel = source.Tel;
			destination.Address = source.Address;
			destination.Mob = source.Mob;
			destination.MobileLong = source.MobileLong;
			destination.DateM = source.DateM;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadPrintMapper.cs", 85);
			throw;
		}
	}

	public Task<SanadPrintBusiness> MapAsync(string contextGuid, WebSanadPrint source, SanadPrintBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new SanadPrintBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Name = source.Name;
			destination.Tel = source.Tel;
			destination.Address = source.Address;
			destination.Mob = source.Mob;
			destination.MobileLong = source.MobileLong;
			destination.DateM = source.DateM;
			destination.AsGuid = source.AsGuid;
			destination.AsType = source.AsType;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Sanad\\SanadPrintMapper.cs", 121);
			throw;
		}
	}

	public Task<List<SanadPrintBusiness>> MapListAsync(string contextGuid, IEnumerable<SanadPrintBusiness> source, List<SanadPrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<SanadPrintBusiness>> MapListAsync(string contextGuid, IEnumerable<WebSanadPrint> source, List<SanadPrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebSanadPrint>> MapListAsync(string contextGuid, IEnumerable<SanadPrintBusiness> source, List<WebSanadPrint> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
