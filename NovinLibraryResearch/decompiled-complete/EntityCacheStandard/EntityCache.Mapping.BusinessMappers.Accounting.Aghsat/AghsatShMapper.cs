using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;

public class AghsatShMapper : IItemAsyncMapper<AghsatShBusiness, WebAghsatShakhsi>, IItemAsyncMapper<AghsatShBusiness, AghsatShBusiness>, IItemAsyncMapper<WebAghsatShakhsi, AghsatShBusiness>
{
	public static AghsatShMapper Instance { get; private set; } = new AghsatShMapper();


	public Task<WebAghsatShakhsi> MapAsync(string contextGuid, AghsatShBusiness source, WebAghsatShakhsi destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebAghsatShakhsi();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.DateM = source.DateM;
			destination.UserGuid = source.UserGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Price = source.Price;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Source = source.Source;
			destination.ChangeStatus = source.ChangeStatus;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.PardakhtDateM = source.PardakhtDateM;
			destination.PardakhtSum = source.PardakhtSum;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatShMapper.cs", 54);
			throw;
		}
	}

	public Task<AghsatShBusiness> MapAsync(string contextGuid, AghsatShBusiness source, AghsatShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AghsatShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.TafsilGuid = source.TafsilGuid;
			destination.FullName = source.FullName;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.DateM = source.DateM;
			destination.UserGuid = source.UserGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Price = source.Price;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Source = source.Source;
			destination.ChangeStatus = source.ChangeStatus;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateUtc;
			destination.PardakhtDateM = source.PardakhtDateM;
			destination.PardakhtSum = source.PardakhtSum;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatShMapper.cs", 97);
			throw;
		}
	}

	public async Task<AghsatShBusiness> MapAsync(string contextGuid, WebAghsatShakhsi source, AghsatShBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AghsatShBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ID2 = source.ID2;
			destination.TafsilGuid = source.TafsilGuid;
			AghsatShBusiness aghsatShBusiness = destination;
			aghsatShBusiness.FullName = (await TafsilBusiness.GetAsync(contextGuid, EnSystem.Mapper, source.TafsilGuid, tranName, token, "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatShMapper.cs"))?.Name;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.DateM = source.DateM;
			destination.UserGuid = source.UserGuid;
			destination.PardakhtGuid = source.PardakhtGuid;
			destination.Price = source.Price;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.Source = source.Source;
			destination.ChangeStatus = source.ChangeStatus;
			destination.MetaData = source.MetaData;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.PardakhtDateM = source.PardakhtDateM;
			destination.PardakhtSum = source.PardakhtSum;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatShMapper.cs", 140);
			throw;
		}
	}

	public Task<List<AghsatShBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatShBusiness> source, List<AghsatShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AghsatShBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAghsatShakhsi> source, List<AghsatShBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAghsatShakhsi>> MapListAsync(string contextGuid, IEnumerable<AghsatShBusiness> source, List<WebAghsatShakhsi> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
