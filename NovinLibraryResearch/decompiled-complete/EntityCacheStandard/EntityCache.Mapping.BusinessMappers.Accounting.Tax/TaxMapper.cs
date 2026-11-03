using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Tax;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tax;

public class TaxMapper : IItemAsyncMapper<WebTax, TaxBusiness>, IItemAsyncMapper<TaxBusiness, TaxBusiness>, IItemAsyncMapper<TaxBusiness, WebTax>
{
	public static TaxMapper Instance { get; private set; } = new TaxMapper();


	public async Task<TaxBusiness> MapAsync(string contextGuid, WebTax source, TaxBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TaxBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Slug = source.Slug;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				foreach (WebTaxDetail detail in source.DetailList)
				{
					TaxDetailMapper instance = TaxDetailMapper.Instance;
					WebTaxDetail source2 = detail;
					await destination.AddDetailListAsync(await instance.MapAsync(contextGuid, source2, await TaxDetailBusiness.GetFactoryAsync(destination, token, tranName), tranName, token));
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxMapper.cs", 53);
			throw;
		}
	}

	public async Task<TaxBusiness> MapAsync(string contextGuid, TaxBusiness source, TaxBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TaxBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Slug = source.Slug;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				foreach (TaxDetailBusiness detail in source.DetailList)
				{
					TaxDetailMapper instance = TaxDetailMapper.Instance;
					TaxDetailBusiness source2 = detail;
					await destination.AddDetailListAsync(await instance.MapAsync(contextGuid, source2, await TaxDetailBusiness.GetFactoryAsync(destination, token, tranName), tranName, token));
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxMapper.cs", 93);
			throw;
		}
	}

	public async Task<WebTax> MapAsync(string contextGuid, TaxBusiness source, WebTax destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTax();
			}
			destination.Guid = source.Guid;
			destination.Name = source.Name;
			destination.Slug = source.Slug;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			if ((source?.DetailList?.Any()).GetValueOrDefault())
			{
				WebTax webTax = destination;
				webTax.DetailList = await TaxDetailMapper.Instance.MapListAsync(contextGuid, source.DetailList, new List<WebTaxDetail>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tax\\TaxMapper.cs", 125);
			throw;
		}
	}

	public Task<List<TaxBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTax> source, List<TaxBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TaxBusiness>> MapListAsync(string contextGuid, IEnumerable<TaxBusiness> source, List<TaxBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebTax>> MapListAsync(string contextGuid, IEnumerable<TaxBusiness> source, List<WebTax> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
