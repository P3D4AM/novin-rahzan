using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class ReceptionHavaleMapper : IItemAsyncMapper<ReceptionHavaleBusiness, WebReceptionHavale>, IItemAsyncMapper<ReceptionHavaleBusiness, ReceptionHavaleBusiness>, IItemAsyncMapper<WebReceptionHavale, ReceptionHavaleBusiness>
{
	public IReception Master { get; private set; }

	public ReceptionHavaleMapper(IReception master)
	{
		Master = master;
	}

	public static ReceptionHavaleMapper GetInstance(IReception master)
	{
		return new ReceptionHavaleMapper(master);
	}

	public async Task<WebReceptionHavale> MapAsync(string contextGuid, ReceptionHavaleBusiness source, WebReceptionHavale destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceptionHavale();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date = source.Date;
			destination.DateUtc = source.Date.ToUniversalTime();
			destination.Description = source.Description;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Number = source.Number;
			destination.Price = source.Price;
			destination.Type = source.HavaleType;
			destination.TerminalId = source.TerminalId;
			destination.PAN = source.PAN;
			destination.STAN = source.STAN;
			destination.RRN = source.RRN;
			destination.MerchantId = source.MerchantId;
			destination.MessageId = source.MessageId;
			destination.DateTasviye = source.DateTasviye;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.Ordinal = source.Ordinal;
			destination.BankRealAccount = source.BankRealAccount;
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
			destination.DateUtc = source.DateUtc;
			if ((Master?.TafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				destination.CustomerNationalCode = (await CusBusiness.GetAsync(contextGuid, EnSystem.Mapper, Master.TafsilGuid, token, tranName))?.CodeMeli ?? "";
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionHavaleMapper.cs", 61);
			throw;
		}
		return destination;
	}

	public Task<List<WebReceptionHavale>> MapListAsync(string contextGuid, IEnumerable<ReceptionHavaleBusiness> source, List<WebReceptionHavale> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionHavaleBusiness> MapAsync(string contextGuid, ReceptionHavaleBusiness source, ReceptionHavaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionHavaleBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date = source.Date;
			destination.MasterGuid = source.MasterGuid;
			destination.Description = source.Description;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.TerminalId = source.TerminalId;
			destination.PAN = source.PAN;
			destination.STAN = source.STAN;
			destination.RRN = source.RRN;
			destination.MerchantId = source.MerchantId;
			destination.MessageId = source.MessageId;
			destination.DateTasviye = source.DateTasviye;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.BankRealAccount = source.BankRealAccount;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
			destination.DateUtc = source.DateUtc;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionHavaleMapper.cs", 107);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ReceptionHavaleBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceptionHavaleBusiness> source, List<ReceptionHavaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<ReceptionHavaleBusiness> MapAsync(string contextGuid, WebReceptionHavale source, ReceptionHavaleBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new ReceptionHavaleBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Date = (source.DateUtc.HasValue ? source.DateUtc.Value.ToLocalTime() : source.Date);
			destination.Description = source.Description;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.Number = source.Number;
			destination.SetPrice(EnSystem.Mapper, source.Price);
			destination.HavaleType = source.Type;
			destination.TerminalId = source.TerminalId;
			destination.PAN = source.PAN;
			destination.STAN = source.STAN;
			destination.RRN = source.RRN;
			destination.MerchantId = source.MerchantId;
			destination.MessageId = source.MessageId;
			destination.DateTasviye = source.DateTasviye;
			destination.Qt = source.Qt;
			destination.DecimalGuid = source.DecimalGuid;
			destination.BankRealAccount = source.BankRealAccount;
			destination.SetOrdinal(EnSystem.Mapper, source.Ordinal);
			destination.WageGuid = source.WageGuid;
			destination.WageTafsilGuid = source.WageTafsilGuid;
			destination.WageMoeinGuid = source.WageMoeinGuid;
			destination.WagePrice = source.WagePrice;
			destination.DateUtc = source.Date.ToUniversalTime();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Khazane\\Reception\\ReceptionHavaleMapper.cs", 153);
			throw;
		}
		return Task.FromResult(destination);
	}

	public Task<List<ReceptionHavaleBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReceptionHavale> source, List<ReceptionHavaleBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
