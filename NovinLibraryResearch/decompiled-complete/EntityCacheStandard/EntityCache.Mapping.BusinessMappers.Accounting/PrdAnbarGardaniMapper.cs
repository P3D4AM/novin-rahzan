using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdAnbarGardaniMapper : IItemAsyncMapper<PrdAnbarGardaniBusiness, WebPrdAnbarGardani>, IItemAsyncMapper<PrdAnbarGardaniBusiness, PrdAnbarGardaniBusiness>, IItemAsyncMapper<WebPrdAnbarGardani, PrdAnbarGardaniBusiness>
{
	public static PrdAnbarGardaniMapper Instance { get; private set; } = new PrdAnbarGardaniMapper();


	public async Task<WebPrdAnbarGardani> MapAsync(string contextGuid, PrdAnbarGardaniBusiness source, WebPrdAnbarGardani destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdAnbarGardani();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateSabt = source.DateSabt;
			destination.Number = source.Number;
			WebPrdAnbarGardani webPrdAnbarGardani = destination;
			webPrdAnbarGardani.SanadNo = await source.GetSanadNumberAsync(token, tranName);
			destination.SanadSt = source.SanadSt;
			destination.KasrDebitTafsilGuid = source.KasrDebitTafsilGuid;
			destination.KasrDebitMoeinGuid = source.KasrDebitMoeinGuid;
			destination.KasrCreditTafsilGuid = source.KasrCreditTafsilGuid;
			destination.KasrCreditMoeinGuid = source.KasrCreditMoeinGuid;
			destination.EzafeDebitTafsilGuid = source.EzafeDebitTafsilGuid;
			destination.EzafeCreditTafsilGuid = source.EzafeCreditTafsilGuid;
			destination.EzafeDebitMoeinGuid = source.EzafeDebitMoeinGuid;
			destination.EzafeCreditMoeinGuid = source.EzafeCreditMoeinGuid;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.StatusId = source.StatusId;
			if (source.ListAnbarGardaniDet != null)
			{
				List<WebPrdAnbarGardaniDet> detList = destination.DetList;
				detList.AddRange(await PrdAnbarGardaniDetMapper.Instance.MapListAsync(contextGuid, source.ListAnbarGardaniDet, new List<WebPrdAnbarGardaniDet>(), tranName, token));
			}
			destination.Type = source.Type;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniMapper.cs", 53);
			throw;
		}
		return destination;
	}

	public Task<PrdAnbarGardaniBusiness> MapAsync(string contextGuid, PrdAnbarGardaniBusiness source, PrdAnbarGardaniBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarGardaniBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateSabt = source.DateSabt;
			destination.Number = source.Number;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.KasrDebitTafsilGuid = source.KasrDebitTafsilGuid;
			destination.KasrDebitMoeinGuid = source.KasrDebitMoeinGuid;
			destination.KasrCreditTafsilGuid = source.KasrCreditTafsilGuid;
			destination.KasrCreditMoeinGuid = source.KasrCreditMoeinGuid;
			destination.EzafeDebitTafsilGuid = source.EzafeDebitTafsilGuid;
			destination.EzafeCreditTafsilGuid = source.EzafeCreditTafsilGuid;
			destination.EzafeDebitMoeinGuid = source.EzafeDebitMoeinGuid;
			destination.EzafeCreditMoeinGuid = source.EzafeCreditMoeinGuid;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.StatusId = source.StatusId;
			if (source.ListAnbarGardaniDet != null)
			{
				destination.AddRangeToDetList(source.ListAnbarGardaniDet.ToList(), clearList: true);
			}
			destination.Type = source.Type;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniMapper.cs", 95);
			throw;
		}
		return Task.FromResult(destination);
	}

	public async Task<PrdAnbarGardaniBusiness> MapAsync(string contextGuid, WebPrdAnbarGardani source, PrdAnbarGardaniBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdAnbarGardaniBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateSabt = source.DateSabt;
			destination.Number = source.Number;
			destination.SanadNo = source.SanadNo;
			destination.SanadSt = source.SanadSt;
			destination.KasrDebitTafsilGuid = source.KasrDebitTafsilGuid;
			destination.KasrDebitMoeinGuid = source.KasrDebitMoeinGuid;
			destination.KasrCreditTafsilGuid = source.KasrCreditTafsilGuid;
			destination.KasrCreditMoeinGuid = source.KasrCreditMoeinGuid;
			destination.EzafeDebitTafsilGuid = source.EzafeDebitTafsilGuid;
			destination.EzafeCreditTafsilGuid = source.EzafeCreditTafsilGuid;
			destination.EzafeDebitMoeinGuid = source.EzafeDebitMoeinGuid;
			destination.EzafeCreditMoeinGuid = source.EzafeCreditMoeinGuid;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.StatusId = source.StatusId;
			if (source.DetList != null)
			{
				PrdAnbarGardaniBusiness prdAnbarGardaniBusiness = destination;
				prdAnbarGardaniBusiness.AddRangeToDetList(await PrdAnbarGardaniDetMapper.Instance.MapListAsync(contextGuid, source.DetList, new List<PrdAnbarGardaniDetBusiness>()), clearList: true);
			}
			destination.Type = source.Type;
			destination.DateUtc = source.DateM.ToUniversalTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdAnbarGardaniMapper.cs", 138);
			throw;
		}
		return destination;
	}

	public Task<List<WebPrdAnbarGardani>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniBusiness> source, List<WebPrdAnbarGardani> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdAnbarGardaniBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdAnbarGardaniBusiness> source, List<PrdAnbarGardaniBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdAnbarGardaniBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdAnbarGardani> source, List<PrdAnbarGardaniBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
