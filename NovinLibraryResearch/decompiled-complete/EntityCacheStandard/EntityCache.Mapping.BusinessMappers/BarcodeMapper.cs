using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class BarcodeMapper : IItemAsyncMapper<BarcodeBusiness, WebBarcode>, IItemAsyncMapper<BarcodeBusiness, BarcodeBusiness>, IItemAsyncMapper<WebBarcode, BarcodeBusiness>
{
	public static BarcodeMapper Instance { get; private set; } = new BarcodeMapper();


	public Task<WebBarcode> MapAsync(string contextGuid, BarcodeBusiness source, WebBarcode destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebBarcode();
			}
			destination.BarCode = source.BarCode;
			destination.CreateDate = source.CreateDate;
			destination.Ordinal = source.Ordinal;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Price = source.Price;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.Quantity = source.Quantity;
			destination.Type = source.Type;
			destination.DepotGuid = source.DepotGuid;
			destination.ChangeStatus = source.ChangeStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\BarcodeMapper.cs", 41);
			throw;
		}
	}

	public Task<List<WebBarcode>> MapListAsync(string contextGuid, IEnumerable<BarcodeBusiness> source, List<WebBarcode> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BarcodeBusiness> MapAsync(string contextGuid, BarcodeBusiness source, BarcodeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BarcodeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.BarCode = source.BarCode;
			destination.CreateDate = source.CreateDate;
			destination.Ordinal = source.Ordinal;
			destination.Description = source.Description;
			destination.Name = source.Name;
			destination.Price = source.Price;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.Quantity = source.Quantity;
			destination.Type = source.Type;
			destination.DepotGuid = source.DepotGuid;
			destination.ChangeStatus = source.ChangeStatus;
			destination.PrdGuid = source.PrdGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\BarcodeMapper.cs", 74);
			throw;
		}
	}

	public Task<List<BarcodeBusiness>> MapListAsync(string contextGuid, IEnumerable<BarcodeBusiness> source, List<BarcodeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<BarcodeBusiness> MapAsync(string contextGuid, WebBarcode source, BarcodeBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new BarcodeBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.BarCode = source.BarCode;
			destination.CreateDate = source.CreateDate;
			destination.Description = source.Description ?? "";
			destination.Ordinal = source.Ordinal;
			destination.Name = source.Name;
			destination.Price = source.Price;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.Quantity = source.Quantity;
			destination.Type = source.Type;
			destination.DepotGuid = source.DepotGuid;
			destination.ChangeStatus = source.ChangeStatus;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\BarcodeMapper.cs", 106);
			throw;
		}
	}

	public Task<List<BarcodeBusiness>> MapListAsync(string contextGuid, IEnumerable<WebBarcode> source, List<BarcodeBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
