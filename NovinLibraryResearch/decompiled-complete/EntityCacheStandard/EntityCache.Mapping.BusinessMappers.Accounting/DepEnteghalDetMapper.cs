using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class DepEnteghalDetMapper : IItemAsyncMapper<DepEnteghalDetBusiness, WebDepEnteghalDet>, IItemAsyncMapper<DepEnteghalDetBusiness, DepEnteghalDetBusiness>, IItemAsyncMapper<WebDepEnteghalDet, DepEnteghalDetBusiness>
{
	public static DepEnteghalDetMapper Instance { get; private set; } = new DepEnteghalDetMapper();


	public Task<WebDepEnteghalDet> MapAsync(string contextGuid, DepEnteghalDetBusiness source, WebDepEnteghalDet destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebDepEnteghalDet();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuidSrc = source.DptGuidSrc;
			destination.DptGuidDst = source.DptGuidDst;
			destination.PrdGuid = source.ProductGuid;
			destination.Quantity = source.Quantity;
			destination.VahedGuid = source.VahedGuid;
			destination.Description = source.Description;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.V2Qt = source.V2Qt;
			destination.V2Price = source.V2Price;
			destination.Ordinal = source.Ordinal;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalDetMapper.cs", 44);
			throw;
		}
	}

	public async Task<DepEnteghalDetBusiness> MapAsync(string contextGuid, DepEnteghalDetBusiness source, DepEnteghalDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepEnteghalDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuidSrc = source.DptGuidSrc;
			destination.DptGuidDst = source.DptGuidDst;
			await destination.SetPrdGuidAsync(source.ProductGuid, EnSystem.Mapper);
			destination.Quantity = source.Quantity;
			destination.VahedGuid = source.VahedGuid;
			destination.Description = source.Description;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.V2Qt = source.V2Qt;
			destination.V2Price = source.V2Price;
			destination.Ordinal = source.Ordinal;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalDetMapper.cs", 77);
			throw;
		}
	}

	public async Task<DepEnteghalDetBusiness> MapAsync(string contextGuid, WebDepEnteghalDet source, DepEnteghalDetBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepEnteghalDetBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.MasterGuid = source.MasterGuid;
			destination.DptGuidSrc = source.DptGuidSrc;
			destination.DptGuidDst = source.DptGuidDst;
			await destination.SetPrdGuidAsync(source.PrdGuid, EnSystem.Mapper);
			destination.Quantity = source.Quantity;
			destination.VahedGuid = source.VahedGuid;
			destination.Description = source.Description;
			destination.Serial = source.Serial;
			destination.Price = source.Price;
			destination.V2Qt = source.V2Qt;
			destination.V2Price = source.V2Price;
			destination.Ordinal = source.Ordinal;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepEnteghalDetMapper.cs", 110);
			throw;
		}
	}

	public Task<List<DepEnteghalDetBusiness>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalDetBusiness> source, List<DepEnteghalDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebDepEnteghalDet>> MapListAsync(string contextGuid, IEnumerable<DepEnteghalDetBusiness> source, List<WebDepEnteghalDet> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DepEnteghalDetBusiness>> MapListAsync(string contextGuid, IEnumerable<WebDepEnteghalDet> source, List<DepEnteghalDetBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
