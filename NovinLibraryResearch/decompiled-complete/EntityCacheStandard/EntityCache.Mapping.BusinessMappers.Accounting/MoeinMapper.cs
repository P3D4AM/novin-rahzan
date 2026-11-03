using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class MoeinMapper : IItemAsyncMapper<MoeinBusiness, WebMoein>, IItemAsyncMapper<MoeinBusiness, MoeinBusiness>, IItemAsyncMapper<WebMoein, MoeinBusiness>
{
	public static MoeinMapper Instance { get; private set; } = new MoeinMapper();


	public async Task<WebMoein> MapAsync(string contextGuid, MoeinBusiness source, WebMoein destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebMoein();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.KolGuid = source.KolGuid;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Mahiat = source.Mahiat;
			destination.System = source.System;
			destination.Peygiri = source.Peygiri;
			destination.Tedadi = source.Tedadi;
			destination.Account = source.Account;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.GardeshHesab = source.GardeshHesab;
			IEnumerable<MoeinTafsilGroupRelationsBusiness> enumerable = await MoeinTafsilGroupRelationsBusiness.GetAllAsync(contextGuid, source.Guid, null, token, tranName);
			if (enumerable != null && enumerable.Any())
			{
				destination.TafsilGroupRelationList = await MoeinTafsilGroupRelationMapper.Instance.MapListAsync(contextGuid, enumerable, new List<WebMoeinTafsilGroupRelation>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinMapper.cs", 57);
			throw;
		}
	}

	public Task<MoeinBusiness> MapAsync(string contextGuid, MoeinBusiness source, MoeinBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MoeinBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.KolGuid = source.KolGuid;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Mahiat = source.Mahiat;
			destination.System = source.System;
			destination.Peygiri = source.Peygiri;
			destination.Tedadi = source.Tedadi;
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
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinMapper.cs", 97);
			throw;
		}
	}

	public Task<MoeinBusiness> MapAsync(string contextGuid, WebMoein source, MoeinBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new MoeinBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.KolGuid = source.KolGuid;
			destination.Code = source.Code;
			destination.Status = source.Status;
			destination.Description = source.Description;
			destination.Mahiat = source.Mahiat;
			destination.System = source.System;
			destination.Peygiri = source.Peygiri;
			destination.Tedadi = source.Tedadi;
			destination.Account = source.Account;
			destination.DateM = source.DateM;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.GardeshHesab = source.GardeshHesab;
			destination.ContextGuid = contextGuid;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\MoeinMapper.cs", 136);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebMoein>> MapListAsync(string contextGuid, IEnumerable<MoeinBusiness> source, List<WebMoein> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MoeinBusiness>> MapListAsync(string contextGuid, IEnumerable<MoeinBusiness> source, List<MoeinBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<MoeinBusiness>> MapListAsync(string contextGuid, IEnumerable<WebMoein> source, List<MoeinBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
