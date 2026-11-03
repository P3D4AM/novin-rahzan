using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Aghsat;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Aghsat;

public class AghsatMGroupMapper : IItemAsyncMapper<AghsatM_GroupBusiness, AghsatM_GroupBusiness>, IItemAsyncMapper<WebAghsatMGroup, AghsatM_GroupBusiness>, IItemAsyncMapper<AghsatM_GroupBusiness, WebAghsatMGroup>
{
	public static AghsatMGroupMapper Instance { get; private set; } = new AghsatMGroupMapper();


	public async Task<AghsatM_GroupBusiness> MapAsync(string contextGuid, AghsatM_GroupBusiness source, AghsatM_GroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new AghsatM_GroupBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Sum = source.Sum;
			destination.Count = source.Count;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.TafsilGuid = source.TafsilGuid;
			AghsatM_GroupBusiness aghsatM_GroupBusiness = destination;
			aghsatM_GroupBusiness.MoeinGuid = await source.GetMoeinGuidAsync(EnSystem.Mapper, token, tranName);
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNo = source.SanadNo;
			destination.ProfitSum = source.ProfitSum;
			destination.ContextGuid = contextGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			aghsatM_GroupBusiness = destination;
			aghsatM_GroupBusiness.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token, tranName);
			aghsatM_GroupBusiness = destination;
			aghsatM_GroupBusiness.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token, tranName);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMGroupMapper.cs", 50);
			throw;
		}
	}

	public Task<AghsatM_GroupBusiness> MapAsync(string contextGuid, WebAghsatMGroup source, AghsatM_GroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new AghsatM_GroupBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Sum = source.Sum;
			destination.Count = source.Count;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNo = source.SanadNo;
			destination.ProfitSum = source.ProfitSum;
			destination.ContextGuid = contextGuid;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.CreditMoeinGuid = source.CreditMoeinGuid;
			destination.CreditTafsilGuid = source.CreditTafsilGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMGroupMapper.cs", 87);
			throw;
		}
	}

	public async Task<WebAghsatMGroup> MapAsync(string contextGuid, AghsatM_GroupBusiness source, WebAghsatMGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebAghsatMGroup();
			}
			destination.Guid = source.Guid;
			destination.Sum = source.Sum;
			destination.Count = source.Count;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.TafsilGuid = source.TafsilGuid;
			WebAghsatMGroup webAghsatMGroup = destination;
			webAghsatMGroup.MoeinGuid = await source.GetMoeinGuidAsync(EnSystem.Mapper, token, tranName);
			destination.CashierGuid = source.CashierGuid;
			destination.SanadNo = source.SanadNo;
			destination.ProfitSum = source.ProfitSum;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			webAghsatMGroup = destination;
			webAghsatMGroup.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token, tranName);
			webAghsatMGroup = destination;
			webAghsatMGroup.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token, tranName);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Aghsat\\AghsatMGroupMapper.cs", 123);
			throw;
		}
	}

	public Task<List<AghsatM_GroupBusiness>> MapListAsync(string contextGuid, IEnumerable<AghsatM_GroupBusiness> source, List<AghsatM_GroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<AghsatM_GroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebAghsatMGroup> source, List<AghsatM_GroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebAghsatMGroup>> MapListAsync(string contextGuid, IEnumerable<AghsatM_GroupBusiness> source, List<WebAghsatMGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
