using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PardakhtCheckMMapper : IItemAsyncMapper<PardakhtCheckMBusiness, WebPardakhtCheckM>, IItemAsyncMapper<PardakhtCheckMBusiness, PardakhtCheckMBusiness>, IItemAsyncMapper<WebPardakhtCheckM, PardakhtCheckMBusiness>
{
	public static PardakhtCheckMMapper Instance { get; private set; } = new PardakhtCheckMMapper();


	public async Task<WebPardakhtCheckM> MapAsync(string contextGuid, PardakhtCheckMBusiness source, WebPardakhtCheckM destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPardakhtCheckM();
			}
			destination.Description = source.RowDesc;
			destination.GuidCheck = source.GuidCheck;
			destination.Price = source.Price;
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			WebPardakhtCheckM webPardakhtCheckM = destination;
			webPardakhtCheckM.CreditMoeinGuid = await source.GetCreditMoeinGuidAsync(token, tranName);
			webPardakhtCheckM = destination;
			webPardakhtCheckM.CreditTafsilGuid = await source.GetCreditTafsilGuidAsync(token, tranName);
			destination.SayadiName = source.SayadiName;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.Ordinal = source.Ordinal;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMMapper.cs", 44);
			throw;
		}
	}

	public Task<List<WebPardakhtCheckM>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckMBusiness> source, List<WebPardakhtCheckM> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<PardakhtCheckMBusiness> MapAsync(string contextGuid, PardakhtCheckMBusiness source, PardakhtCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtCheckMBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.RowDesc = source.RowDesc;
			destination.GuidCheck = source.GuidCheck;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			PardakhtCheckMBusiness pardakhtCheckMBusiness = destination;
			pardakhtCheckMBusiness.SetCreditMoeinGuid(await source.GetCreditMoeinGuidAsync(token, tranName));
			pardakhtCheckMBusiness = destination;
			pardakhtCheckMBusiness.SetCreditTafsilGuid(await source.GetCreditTafsilGuidAsync(token, tranName));
			destination.SayadiName = source.SayadiName;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMMapper.cs", 80);
			throw;
		}
	}

	public Task<List<PardakhtCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtCheckMBusiness> source, List<PardakhtCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtCheckMBusiness> MapAsync(string contextGuid, WebPardakhtCheckM source, PardakhtCheckMBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtCheckMBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.RowDesc = source.Description;
			destination.GuidCheck = source.GuidCheck;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.SetCreditMoeinGuid(source.CreditMoeinGuid);
			destination.SetCreditTafsilGuid(source.CreditTafsilGuid);
			destination.SayadiName = source.SayadiName;
			destination.SayadiNationalCode = source.SayadiNationalCode;
			destination.SayadiRegistered = source.SayadiRegistered;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtCheckMMapper.cs", 116);
			throw;
		}
	}

	public Task<List<PardakhtCheckMBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPardakhtCheckM> source, List<PardakhtCheckMBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
