using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.MetaData;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class TafsilGroupMapper : IItemAsyncMapper<TafsilGroupBusiness, WebTafsilGroup>, IItemAsyncMapper<TafsilGroupBusiness, TafsilGroupBusiness>, IItemAsyncMapper<WebTafsilGroup, TafsilGroupBusiness>
{
	public static TafsilGroupMapper Instance { get; private set; } = new TafsilGroupMapper();


	public async Task<WebTafsilGroup> MapAsync(string contextGuid, TafsilGroupBusiness source, WebTafsilGroup destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebTafsilGroup();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Tbl = source.Tbl;
			destination.Description = source.Description;
			destination.CodeMin = source.CodeMin;
			destination.CodeMax = source.CodeMax;
			destination.Status = source.Status;
			WebTafsilGroup webTafsilGroup = destination;
			webTafsilGroup.MoeinGuid = await source.GetMoeinGuidAsync(token, tranName);
			destination.MoeinSellGuid = source.MoeinSellGuid;
			destination.MoeinBuyGuid = source.MoeinBuyGuid;
			destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
			destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
			destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
			destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
			destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
			destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
			destination.Checked = source.Checked;
			destination.Id = source.Id;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.BazaryabDefaultGuid = source.BazaryabDefaultGuid;
			destination.Type = source.Type;
			if (source.MetaData != null)
			{
				destination.MetaData = new TafsilGroupMetaData
				{
					Level = source.MetaData.Level
				};
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilGroupMapper.cs", 58);
			throw;
		}
	}

	public Task<List<WebTafsilGroup>> MapListAsync(string contextGuid, IEnumerable<TafsilGroupBusiness> source, List<WebTafsilGroup> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<TafsilGroupBusiness> MapAsync(string contextGuid, TafsilGroupBusiness source, TafsilGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Tbl = source.Tbl;
			destination.Description = source.Description;
			destination.CodeMin = source.CodeMin;
			destination.CodeMax = source.CodeMax;
			destination.Status = source.Status;
			TafsilGroupBusiness tafsilGroupBusiness = destination;
			tafsilGroupBusiness.SetMoeinGuid(await source.GetMoeinGuidAsync(token, tranName));
			destination.MoeinSellGuid = source.MoeinSellGuid;
			destination.MoeinBuyGuid = source.MoeinBuyGuid;
			destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
			destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
			destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
			destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
			destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
			destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
			destination.Checked = source.Checked;
			destination.IsChecked = source.IsChecked;
			destination.Id = source.Id;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.ChangeStatus = source.ChangeStatus;
			destination.BazaryabDefaultGuid = source.BazaryabDefaultGuid;
			destination.Type = source.Type;
			if (source.MetaData != null)
			{
				destination.MetaData = new TafsilGroupMetaData
				{
					Level = source.MetaData.Level
				};
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilGroupMapper.cs", 109);
			throw;
		}
	}

	public Task<List<TafsilGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<TafsilGroupBusiness> source, List<TafsilGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<TafsilGroupBusiness> MapAsync(string contextGuid, WebTafsilGroup source, TafsilGroupBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TafsilGroupBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Name = source.Name;
			destination.Tbl = source.Tbl;
			destination.Description = source.Description;
			destination.CodeMin = source.CodeMin;
			destination.CodeMax = source.CodeMax;
			destination.Status = source.Status;
			destination.SetMoeinGuid(source.MoeinGuid);
			destination.MoeinSellGuid = source.MoeinSellGuid;
			destination.MoeinBuyGuid = source.MoeinBuyGuid;
			destination.MoeinBackSellGuid = source.MoeinBackSellGuid;
			destination.MoeinBackBuyGuid = source.MoeinBackBuyGuid;
			destination.MoeinReceptionGuid = source.MoeinReceptionGuid;
			destination.MoeinPardakhtGuid = source.MoeinPardakhtGuid;
			destination.MoeinAmaniMaNazdDigaranGuid = source.MoeinAmaniMaNazdDigaranGuid;
			destination.MoeinAmaniDigaranNazdMaGuid = source.MoeinAmaniDigaranNazdMaGuid;
			destination.Checked = source.Checked;
			destination.Id = source.Id;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.DeliveryDate = source.DeliveryDate;
			destination.BazaryabDefaultGuid = source.BazaryabDefaultGuid;
			destination.Type = source.Type;
			if (source.MetaData != null)
			{
				destination.MetaData = new TafsilGroupMetaData
				{
					Level = source.MetaData.Level
				};
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Coding\\TafsilGroupMapper.cs", 158);
			throw;
		}
	}

	public Task<List<TafsilGroupBusiness>> MapListAsync(string contextGuid, IEnumerable<WebTafsilGroup> source, List<TafsilGroupBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
