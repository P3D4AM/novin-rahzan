using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.GoldAndCurrency;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class DecimalMapper : IItemAsyncMapper<DecimalBusiness, WebDecimal>, IItemAsyncMapper<DecimalBusiness, DecimalBusiness>, IItemAsyncMapper<WebDecimal, DecimalBusiness>, IItemAsyncMapper<WebGoldAndCurrencyReport, DecimalBusiness>
{
	public static DecimalMapper Instance { get; private set; } = new DecimalMapper();


	public Task<DecimalBusiness> MapAsync(string contextGuid, WebDecimal source, DecimalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DecimalBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Country = source.Country;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Abstract = source.Abstract;
			destination.LastUpdate = source.LastUpdate;
			destination.LastPrice = source.LastPrice;
			destination.IconIndex = source.IconIndex;
			destination.Symbol = source.Symbol;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalMapper.cs", 46);
			throw;
		}
	}

	public Task<DecimalBusiness> MapAsync(string contextGuid, DecimalBusiness source, DecimalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DecimalBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Country = source.Country;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Abstract = source.Abstract;
			destination.LastUpdate = source.LastUpdate;
			destination.LastPrice = source.LastPrice;
			destination.IconIndex = source.IconIndex;
			destination.Symbol = source.Symbol;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalMapper.cs", 76);
			throw;
		}
	}

	public Task<WebDecimal> MapAsync(string contextGuid, DecimalBusiness source, WebDecimal destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDecimal();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Country = source.Country;
			destination.Description = source.Description;
			destination.Status = source.Status;
			destination.Abstract = source.Abstract;
			destination.LastUpdate = source.LastUpdate;
			destination.LastPrice = source.LastPrice;
			destination.IconIndex = source.IconIndex;
			destination.Symbol = source.Symbol;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalMapper.cs", 105);
			throw;
		}
	}

	public Task<DecimalBusiness> MapAsync(string contextGuid, WebGoldAndCurrencyReport source, DecimalBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DecimalBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = Guid.NewGuid();
			destination.Modified = DateTime.Now;
			destination.Name = source.PersianName;
			destination.Country = "";
			destination.Description = "";
			destination.Status = true;
			destination.Abstract = source.Sign;
			destination.LastUpdate = DateTime.Now;
			destination.LastPrice = source.Price;
			destination.IconIndex = 0;
			destination.Symbol = source.Namad;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DecimalMapper.cs", 135);
			throw;
		}
	}

	public Task<List<DecimalBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDecimal> source, List<DecimalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DecimalBusiness>> MapListAsync(string contextGuid, IEnumerable<DecimalBusiness> source, List<DecimalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDecimal>> MapListAsync(string contextGuid, IEnumerable<DecimalBusiness> source, List<WebDecimal> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DecimalBusiness>> MapListAsync(string contextGuid, IEnumerable<WebGoldAndCurrencyReport> source, List<DecimalBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
