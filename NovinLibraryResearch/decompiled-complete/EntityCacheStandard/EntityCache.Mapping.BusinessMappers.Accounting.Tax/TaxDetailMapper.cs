using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tax;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tax;

public class TaxDetailMapper : IItemAsyncMapper<WebTaxDetail, TaxDetailBusiness>, IItemAsyncMapper<TaxDetailBusiness, TaxDetailBusiness>, IItemAsyncMapper<TaxDetailBusiness, WebTaxDetail>
{
	public static TaxDetailMapper Instance { get; private set; } = new TaxDetailMapper();


	public Task<TaxDetailBusiness> MapAsync(string contextGuid, WebTaxDetail source, TaxDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new ArgumentException("Please Pass New Destination");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.FeeTaxPercent = source.FeeTaxPercent;
			destination.ProductTaxPercent = source.ProductTaxPercent;
			destination.ProfitTaxPercent = source.ProfitTaxPercent;
			destination.Ordinal = source.Ordinal;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxDetailMapper.cs", 40);
			throw;
		}
	}

	public Task<TaxDetailBusiness> MapAsync(string contextGuid, TaxDetailBusiness source, TaxDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new ArgumentException("Please Pass New Destination");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return Task.FromResult(destination);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.FeeTaxPercent = source.FeeTaxPercent;
			destination.ProductTaxPercent = source.ProductTaxPercent;
			destination.ProfitTaxPercent = source.ProfitTaxPercent;
			destination.Ordinal = source.Ordinal;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxDetailMapper.cs", 66);
			throw;
		}
	}

	public Task<WebTaxDetail> MapAsync(string contextGuid, TaxDetailBusiness source, WebTaxDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTaxDetail();
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.FeeTaxPercent = source.FeeTaxPercent;
			destination.ProductTaxPercent = source.ProductTaxPercent;
			destination.ProfitTaxPercent = source.ProfitTaxPercent;
			destination.Ordinal = source.Ordinal;
			destination.Description = source.Description;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxDetailMapper.cs", 91);
			throw;
		}
	}

	public Task<List<TaxDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTaxDetail> source, List<TaxDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TaxDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxDetailBusiness> source, List<TaxDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTaxDetail>> MapListAsync(string contextGuid, IEnumerable<TaxDetailBusiness> source, List<WebTaxDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
