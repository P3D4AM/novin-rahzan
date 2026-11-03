using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class PrdSerialMapper : IItemAsyncMapper<PrdSerialBusiness, WebPrdSerial>, IItemAsyncMapper<PrdSerialBusiness, PrdSerialBusiness>, IItemAsyncMapper<WebPrdSerial, PrdSerialBusiness>
{
	public static PrdSerialMapper Instance { get; private set; } = new PrdSerialMapper();


	public async Task<WebPrdSerial> MapAsync(string contextGuid, PrdSerialBusiness source, WebPrdSerial destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebPrdSerial();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.Serial = source.Serial;
			destination.GarrantyCode = source.GarrantyCode;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.IsTransfer = source.IsTransfer;
			destination.TechnicalInfo1 = source.TechnicalInfo1;
			destination.TechnicalInfo2 = source.TechnicalInfo2;
			destination.TechnicalInfo3 = source.TechnicalInfo3;
			destination.InsertDate = source.InsertDate;
			if (source.DetList?.Any() ?? false)
			{
				destination.DetList = new List<WebPrdSerialDetail>();
				foreach (PrdSerialDetailBusiness det in source.DetList)
				{
					WebPrdSerialDetail item = await PrdSerailDetailMapper.Instance.MapAsync(contextGuid, det, new WebPrdSerialDetail(), tranName, token);
					destination.DetList.Add(item);
				}
			}
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerialMapper.cs", 63);
			throw;
		}
	}

	public async Task<PrdSerialBusiness> MapAsync(string contextGuid, PrdSerialBusiness source, PrdSerialBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSerialBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.Serial = source.Serial;
			destination.GarrantyCode = source.GarrantyCode;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.IsTransfer = source.IsTransfer;
			destination.TechnicalInfo1 = source.TechnicalInfo1;
			destination.TechnicalInfo2 = source.TechnicalInfo2;
			destination.TechnicalInfo3 = source.TechnicalInfo3;
			destination.InsertDate = source.InsertDate;
			if (source.DetList?.Any() ?? false)
			{
				foreach (PrdSerialDetailBusiness det in source.DetList)
				{
					destination.AddDet(await PrdSerailDetailMapper.Instance.MapAsync(contextGuid, det, new PrdSerialDetailBusiness(contextGuid), tranName, token));
				}
			}
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerialMapper.cs", 112);
			throw;
		}
	}

	public async Task<PrdSerialBusiness> MapAsync(string contextGuid, WebPrdSerial source, PrdSerialBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new PrdSerialBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ProductGuid = source.ProductGuid;
			destination.Serial = source.Serial;
			destination.GarrantyCode = source.GarrantyCode;
			destination.StartDate = source.StartDate;
			destination.EndDate = source.EndDate;
			destination.ServerStatus = source.ServerStatus;
			destination.DeliveryDate = source.DeliveryDate;
			destination.DeliveryId = source.DeliveryId;
			destination.DeliveryMessage = source.DeliveryMessage;
			destination.DeliveryShortMessage = source.DeliveryShortMessage;
			destination.HostRefrenced = source.HostRefrenced;
			destination.IsTransfer = source.IsTransfer;
			destination.TechnicalInfo1 = source.TechnicalInfo1;
			destination.TechnicalInfo2 = source.TechnicalInfo2;
			destination.TechnicalInfo3 = source.TechnicalInfo3;
			if (source.InsertDate != DateTime.MinValue)
			{
				destination.InsertDate = source.InsertDate;
			}
			if (source.DetList?.Any() ?? false)
			{
				foreach (WebPrdSerialDetail det in source.DetList)
				{
					destination.AddDet(await PrdSerailDetailMapper.Instance.MapAsync(contextGuid, det, new PrdSerialDetailBusiness(contextGuid), tranName, token));
				}
			}
			return destination;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user."))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\PrdSerialMapper.cs", 162);
			throw;
		}
	}

	public Task<List<WebPrdSerial>> MapListAsync(string contextGuid, IEnumerable<PrdSerialBusiness> source, List<WebPrdSerial> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSerialBusiness>> MapListAsync(string contextGuid, IEnumerable<PrdSerialBusiness> source, List<PrdSerialBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<PrdSerialBusiness>> MapListAsync(string contextGuid, IEnumerable<WebPrdSerial> source, List<PrdSerialBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
