using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdAnbarGardaniDetMapper : IItemAsyncMapper<PrdAnbarGardaniDetBusiness, WebPrdAnbarGardaniDet>, IItemAsyncMapper<PrdAnbarGardaniDetBusiness, PrdAnbarGardaniDetBusiness>, IItemAsyncMapper<WebPrdAnbarGardaniDet, PrdAnbarGardaniDetBusiness>
{
	public static PrdAnbarGardaniDetMapper Instance { get; private set; } = new PrdAnbarGardaniDetMapper();


	public async Task<WebPrdAnbarGardaniDet> MapAsync(string contextGuid, PrdAnbarGardaniDetBusiness source, WebPrdAnbarGardaniDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdAnbarGardaniDet();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuid = source.DptGuid;
			destination.QtKasr = source.QtKasr;
			destination.QtEzafe = source.QtEzafe;
			destination.UnitPrice = source.UnitPrice;
			destination.PrdGuid = source.PrdGuid;
			destination.Status = source.Status;
			destination.QtRealAmount = source.QtRealAmount;
			List<PrdSerialBusiness> list = await source.GetSerialsAsync(token, tranName);
			if (list != null && list.Any())
			{
				WebPrdAnbarGardaniDet webPrdAnbarGardaniDet = destination;
				webPrdAnbarGardaniDet.SerialStr = await PrdSerialMapper.Instance.MapListAsync(contextGuid, list, new List<WebPrdSerial>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniDetMapper.cs", 48);
			throw;
		}
	}

	public async Task<PrdAnbarGardaniDetBusiness> MapAsync(string contextGuid, PrdAnbarGardaniDetBusiness source, PrdAnbarGardaniDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarGardaniDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuid = source.DptGuid;
			destination.UnitPrice = source.UnitPrice;
			destination.QtRealAmount = source.QtRealAmount;
			destination.PrdGuid = source.PrdGuid;
			destination.Status = source.Status;
			destination.QtKasr = source.QtKasr;
			destination.QtEzafe = source.QtEzafe;
			List<PrdSerialBusiness> list = await source.GetSerialsAsync(token, tranName);
			if (list != null && list.Any())
			{
				destination.AddRangeToSerialList(EnSystem.Mapper, await PrdSerialMapper.Instance.MapListAsync(contextGuid, list, new List<PrdSerialBusiness>(), tranName, token), token, tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniDetMapper.cs", 85);
			throw;
		}
	}

	public async Task<PrdAnbarGardaniDetBusiness> MapAsync(string contextGuid, WebPrdAnbarGardaniDet source, PrdAnbarGardaniDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarGardaniDetBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuid = source.DptGuid;
			destination.QtKasrForDisplay = source.QtKasr;
			destination.QtEzafeForDisplay = source.QtEzafe;
			destination.UnitPrice = source.UnitPrice;
			destination.PrdGuid = source.PrdGuid;
			destination.Status = source.Status;
			destination.QtRealAmountForDisplay = source.QtRealAmount;
			destination.ContextGuid = contextGuid;
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				destination.AddRangeToSerialList(EnSystem.Mapper, await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token), token, tranName);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniDetMapper.cs", 121);
			throw;
		}
	}

	public Task<List<WebPrdAnbarGardaniDet>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniDetBusiness> source, List<WebPrdAnbarGardaniDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdAnbarGardaniDetBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniDetBusiness> source, List<PrdAnbarGardaniDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdAnbarGardaniDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdAnbarGardaniDet> source, List<PrdAnbarGardaniDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
