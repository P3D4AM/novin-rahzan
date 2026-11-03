using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class PardakhtBankMapper : IItemAsyncMapper<PardakhtBankBusiness, WebPardakhtBank>, IItemAsyncMapper<PardakhtBankBusiness, PardakhtBankBusiness>, IItemAsyncMapper<WebPardakhtBank, PardakhtBankBusiness>
{
	public static PardakhtBankMapper Instance { get; private set; } = new PardakhtBankMapper();


	public Task<WebPardakhtBank> MapAsync(string contextGuid, PardakhtBankBusiness source, WebPardakhtBank destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPardakhtBank();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateUtc;
			destination.Description = source.RowDesc;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.Ordinal = source.Ordinal;
			destination.BankRealAccount = source.BankRealAccount;
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtBankMapper.cs", 47);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<WebPardakhtBank>> MapListAsync(string contextGuid, IEnumerable<PardakhtBankBusiness> source, List<WebPardakhtBank> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtBankBusiness> MapAsync(string contextGuid, PardakhtBankBusiness source, PardakhtBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtBankBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.RowDesc = source.RowDesc;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.BankRealAccount = source.BankRealAccount;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.ContextGuid = contextGuid;
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtBankMapper.cs", 85);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PardakhtBankBusiness>> MapListAsync(string contextGuid, IEnumerable<PardakhtBankBusiness> source, List<PardakhtBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<PardakhtBankBusiness> MapAsync(string contextGuid, WebPardakhtBank source, PardakhtBankBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PardakhtBankBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DateM = source.DateM;
			destination.DateUtc = source.DateM.ToUniversalTime();
			destination.RowDesc = source.Description;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.DecimalGuid = source.DecimalGuid;
			destination.MasterGuid = source.MasterGuid;
			destination.BankRealAccount = source.BankRealAccount;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Pardakht\\PardakhtBankMapper.cs", 123);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<PardakhtBankBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPardakhtBank> source, List<PardakhtBankBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
