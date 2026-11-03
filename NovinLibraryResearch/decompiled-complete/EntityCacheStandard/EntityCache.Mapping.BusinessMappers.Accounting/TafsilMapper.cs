using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class TafsilMapper : IItemAsyncMapper<TafsilBusiness, WebTafsil>, IItemAsyncMapper<TafsilBusiness, TafsilBusiness>, IItemAsyncMapper<WebTafsil, TafsilBusiness>
{
	public static TafsilMapper Instance { get; private set; } = new TafsilMapper();


	public Task<WebTafsil> MapAsync(string contextGuid, TafsilBusiness source, WebTafsil destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsil();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.HType = source.HType;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.SerializedData = source.SerializedData;
			destination.SumAghsat = source.SumAghsat;
			destination.Version = source.Version;
			destination.Account = source.Account;
			destination.IsSpecial = source.IsSpecial;
			destination.ClientVerion = 1;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilMapper.cs", 54);
			throw;
		}
	}

	public Task<List<WebTafsil>> MapListAsync(string contextGuid, IEnumerable<TafsilBusiness> source, List<WebTafsil> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TafsilBusiness> MapAsync(string contextGuid, TafsilBusiness source, TafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.HType = source.HType;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.SerializedData = source.SerializedData;
			destination.SumAghsat = source.SumAghsat;
			destination.Version = source.Version;
			destination.IsSpecial = source.IsSpecial;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilMapper.cs", 95);
			throw;
		}
	}

	public Task<List<TafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilBusiness> source, List<TafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TafsilBusiness> MapAsync(string contextGuid, WebTafsil source, TafsilBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.TafsilGroupGuid = source.TafsilGroupGuid;
			destination.HType = source.HType;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.SerializedData = source.SerializedData;
			destination.SumAghsat = source.SumAghsat;
			destination.Version = source.Version;
			destination.IsSpecial = source.IsSpecial;
			destination.Account = source.Account;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilMapper.cs", 137);
			throw;
		}
	}

	public Task<List<TafsilBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsil> source, List<TafsilBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
