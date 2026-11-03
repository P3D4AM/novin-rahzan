using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class AghsatMMapper : IItemAsyncMapper<AghsatMBusiness, WebAghsatAshkhas>, IItemAsyncMapper<AghsatMBusiness, AghsatMBusiness>, IItemAsyncMapper<WebAghsatAshkhas, AghsatMBusiness>
{
	public static AghsatMMapper Instance { get; private set; } = new AghsatMMapper();


	public Task<AghsatMBusiness> MapAsync(string contextGuid, WebAghsatAshkhas source, AghsatMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AghsatMBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id2 = source.Id2;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.Date = source.Date;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.Price = source.Price;
			destination.TypePardakht = source.TypePardakht;
			destination.Mounth = source.Mounth;
			destination.GuidGroup = source.GuidGroup;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.MetaData = source.MetaData;
			destination.ContextGuid = contextGuid;
			destination.VajhEltezam = source.VajhEltezam;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.SanadNo = source.SanadNo;
			destination.VajhEltezamModified = source.VajhEltezamModified;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			destination.ReceptionDateM = source.ReceptionDateM;
			destination.ReceptionSum = source.ReceptionSum;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMMapper.cs", 57);
			throw;
		}
	}

	public async Task<AghsatMBusiness> MapAsync(string contextGuid, AghsatMBusiness source, AghsatMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AghsatMBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id2 = source.Id2;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.Date = source.Date;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.Price = source.GetPrice();
			destination.TypePardakht = source.TypePardakht;
			destination.Mounth = source.Mounth;
			destination.GuidGroup = source.GuidGroup;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.MetaData = source.MetaData;
			destination.ContextGuid = contextGuid;
			destination.VajhEltezam = source.VajhEltezam;
			destination.DateUtc = source.DateUtc;
			destination.SanadNo = source.SanadNo;
			destination.VajhEltezamModified = source.VajhEltezamModified;
			AghsatMBusiness aghsatMBusiness = destination;
			aghsatMBusiness.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token);
			aghsatMBusiness = destination;
			aghsatMBusiness.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token);
			destination.ReceptionDateM = source.ReceptionDateM;
			destination.ReceptionSum = source.ReceptionSum;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMMapper.cs", 102);
			throw;
		}
	}

	public async Task<WebAghsatAshkhas> MapAsync(string contextGuid, AghsatMBusiness source, WebAghsatAshkhas destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 5;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebAghsatAshkhas();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Id2 = source.Id2;
			destination.Description = source.Description;
			destination.DateSarResid = source.DateSarResid;
			destination.Date = source.Date;
			destination.ReceptionGuid = source.ReceptionGuid;
			destination.Price = source.GetPrice();
			destination.ReceptionSum = source.ReceptionSum;
			destination.UserName = source.UserName;
			destination.Code = source.TafsilCode;
			destination.FullName = source.FullName;
			destination.DateSh = source.DateSh;
			destination.DateSarResidSh = source.DateSarResidSh;
			destination.TypePardakht = source.TypePardakht;
			destination.Mounth = source.Mounth;
			destination.GuidGroup = source.GuidGroup;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.MetaData = source.MetaData;
			destination.VajhEltezam = source.VajhEltezam;
			destination.DateUtc = source.DateUtc;
			destination.SanadNo = source.SanadNo;
			destination.VajhEltezamModified = source.VajhEltezamModified;
			WebAghsatAshkhas webAghsatAshkhas = destination;
			webAghsatAshkhas.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token);
			webAghsatAshkhas = destination;
			webAghsatAshkhas.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token);
			destination.ReceptionDateM = source.ReceptionDateM;
			destination.ReceptionSum = source.ReceptionSum;
			AghsatM_GroupBusiness group = await source.GetGroupAsync(token, tranName);
			if (group != null)
			{
				webAghsatAshkhas = destination;
				webAghsatAshkhas.MoeinGuid = await group.GetMoeinGuidAsync(EnSystem.Mapper, token, tranName);
				destination.TafsilGuid = group.TafsilGuid;
				destination.UserGuid = group.CashierGuid;
				webAghsatAshkhas = destination;
				webAghsatAshkhas.UserName = (await group.GetCashierAsync(token, tranName))?.Name;
				TafsilBusiness tafsilBusiness = await group.GetTafsilAsync(EnSystem.Mapper, token, tranName);
				destination.FullCode = (tafsilBusiness?.Code ?? 0).ToString();
				destination.Code = (tafsilBusiness?.Code ?? 0).ToString();
				destination.FullName = tafsilBusiness?.Name;
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMMapper.cs", 164);
			throw;
		}
	}

	public Task<List<WebAghsatAshkhas>> MapListAsync(string contextGuid, IEnumerable<AghsatMBusiness> source, List<WebAghsatAshkhas> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AghsatMBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatMBusiness> source, List<AghsatMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AghsatMBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAghsatAshkhas> source, List<AghsatMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
