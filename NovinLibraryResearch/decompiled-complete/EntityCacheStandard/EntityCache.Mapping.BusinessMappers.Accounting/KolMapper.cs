using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class KolMapper : IItemAsyncMapper<KolBusiness, WebKol>, IItemAsyncMapper<KolBusiness, KolBusiness>, IItemAsyncMapper<WebKol, KolBusiness>
{
	public static KolMapper Instance { get; private set; } = new KolMapper();


	public Task<WebKol> MapAsync(string contextGuid, KolBusiness source, WebKol destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebKol();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.GroupGuid = source.GroupGuid;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.Account = source.Account;
			destination.DateM = source.DateM;
			destination.GardeshHesab = source.GardeshHesab;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\KolMapper.cs", 49);
			throw;
		}
	}

	public Task<KolBusiness> MapAsync(string contextGuid, KolBusiness source, KolBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new KolBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.SetGroupGuid(EnSystem.Mapper, source.GroupGuid);
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.Account = source.Account;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.ChangeStatus = source.ChangeStatus;
			destination.GardeshHesab = source.GardeshHesab;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\KolMapper.cs", 87);
			throw;
		}
	}

	public Task<KolBusiness> MapAsync(string contextGuid, WebKol source, KolBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new KolBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.SetGroupGuid(EnSystem.Mapper, source.GroupGuid);
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.System = source.System;
			destination.Account = source.Account;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.GardeshHesab = source.GardeshHesab;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\KolMapper.cs", 125);
			throw;
		}
	}

	public Task<List<WebKol>> MapListAsync(string contextGuid, IEnumerable<KolBusiness> source, List<WebKol> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<KolBusiness>> MapListAsync(string contextGuid, IEnumerable<KolBusiness> source, List<KolBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<KolBusiness>> MapListAsync(string contextGuid, IEnumerable<WebKol> source, List<KolBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
