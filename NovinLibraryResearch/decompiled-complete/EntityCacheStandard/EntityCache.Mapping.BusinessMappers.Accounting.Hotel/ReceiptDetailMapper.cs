using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.Hotel;
using PacketParser.Entities;
using PacketParser.Entities.Hotel;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Hotel;

public class ReceiptDetailMapper : IItemAsyncMapper<ReceiptDetailBusiness, ReceiptDetailBusiness>, IItemAsyncMapper<WebReceiptDetail, ReceiptDetailBusiness>, IItemAsyncMapper<ReceiptDetailBusiness, WebReceiptDetail>
{
	public static ReceiptDetailMapper Instance { get; private set; } = new ReceiptDetailMapper();


	public async Task<ReceiptDetailBusiness> MapAsync(string contextGuid, ReceiptDetailBusiness source, ReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new NullReferenceException("Please New kon destination ");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			await destination.SetRoomGuidAsync(EnSystem.Mapper, source.Guid, token, tranName);
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.Tax = source.Tax;
			destination.TaxPercent = source.TaxPercent;
			destination.Avarez = source.Avarez;
			destination.AvarezPercent = source.AvarezPercent;
			destination.ExtraCount = source.ExtraCount;
			destination.ExtraPrice = source.ExtraPrice;
			destination.Discount = source.Discount;
			destination.DiscountPercent = source.DiscountPercent;
			destination.PricePerNight = source.PricePerNight;
			destination.Penalty = source.Penalty;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\ReceiptDetailMapper.cs", 50);
			throw;
		}
	}

	public async Task<WebReceiptDetail> MapAsync(string contextGuid, ReceiptDetailBusiness source, WebReceiptDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebReceiptDetail();
			}
			destination.Guid = source.Guid;
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.Tax = source.Tax;
			destination.TaxPercent = source.TaxPercent;
			destination.Avarez = source.Avarez;
			destination.AvarezPercent = source.AvarezPercent;
			destination.ExtraCount = source.ExtraCount;
			destination.ExtraPrice = source.ExtraPrice;
			destination.Discount = source.Discount;
			destination.DiscountPercent = source.DiscountPercent;
			destination.PricePerNight = source.PricePerNight;
			destination.Penalty = source.Penalty;
			destination.RoomGuid = source.RoomGuid;
			if (source.ServiceList != null)
			{
				WebReceiptDetail webReceiptDetail = destination;
				webReceiptDetail.ServiceList = await ProductMapper.Instance.MapListAsync(contextGuid, source.ServiceList.ToList(), new List<WebProductCompact>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\ReceiptDetailMapper.cs", 87);
			throw;
		}
	}

	public async Task<ReceiptDetailBusiness> MapAsync(string contextGuid, WebReceiptDetail source, ReceiptDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new NullReferenceException("Please New kon destination ");
		}
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			destination.Guid = source.Guid;
			await destination.SetRoomGuidAsync(EnSystem.Mapper, source.Guid, token, tranName);
			destination.MasterGuid = source.MasterGuid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.Tax = source.Tax;
			destination.TaxPercent = source.TaxPercent;
			destination.Avarez = source.Avarez;
			destination.AvarezPercent = source.AvarezPercent;
			destination.ExtraCount = source.ExtraCount;
			destination.ExtraPrice = source.ExtraPrice;
			destination.Discount = source.Discount;
			destination.DiscountPercent = source.DiscountPercent;
			destination.PricePerNight = source.PricePerNight;
			destination.Penalty = source.Penalty;
			if (source.ServiceList != null)
			{
				await destination.AddRangeServesList(await ProductMapper.Instance.MapListAsync(contextGuid, source.ServiceList.ToList(), new List<ProductCompactBusiness>(), tranName, token), clearlist: true);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Hotel\\ReceiptDetailMapper.cs", 127);
			throw;
		}
	}

	public Task<List<ReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<ReceiptDetailBusiness> source, List<ReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<WebReceiptDetail>> MapListAsync(string contextGuid, IEnumerable<ReceiptDetailBusiness> source, List<WebReceiptDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<ReceiptDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebReceiptDetail> source, List<ReceiptDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
