using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.Global;

public class TempBarcodePrintMapper : IItemAsyncMapper<TempBarcodePrintBusiness, TempBarcodePrintBusiness>
{
	public static TempBarcodePrintMapper Instance { get; private set; } = new TempBarcodePrintMapper();


	public Task<TempBarcodePrintBusiness> MapAsync(string contextGuid, TempBarcodePrintBusiness source, TempBarcodePrintBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TempBarcodePrintBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ProductGuid = source.ProductGuid;
			destination.TafsilGuid = source.TafsilGuid;
			destination.DetailGuid = source.DetailGuid;
			destination.Barcode = source.Barcode;
			destination.GuarantyCode = source.GuarantyCode;
			destination.AsType = source.AsType;
			destination.MasterDescription = source.MasterDescription;
			destination.DetailDescription = source.DetailDescription;
			destination.MasterDate = source.MasterDate;
			destination.GuarantyStartDate = source.GuarantyStartDate;
			destination.GuarantyEndDate = source.GuarantyEndDate;
			destination.MasterGuid = source.MasterGuid;
			destination.TempDescription = source.TempDescription;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\TempBarcodePrintMapper.cs", 48);
			throw;
		}
	}

	public Task<List<TempBarcodePrintBusiness>> MapListAsync(string contextGuid, IEnumerable<TempBarcodePrintBusiness> source, List<TempBarcodePrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TempBarcodePrintBusiness>> MapBarcodeListAsync(string contextGuid, OrderDetailBusiness source, List<TempBarcodePrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				return Task.FromResult(new List<TempBarcodePrintBusiness>());
			}
			if (destination == null)
			{
				destination = new List<TempBarcodePrintBusiness>();
			}
			for (int i = 0; i < (int)source.Quantity; i = checked(i + 1))
			{
				if (token.IsCancellationRequested)
				{
					throw new OperationCanceledException();
				}
				TempBarcodePrintBusiness item = new TempBarcodePrintBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					ProductGuid = source.ProductGuid,
					TafsilGuid = source.MasterOrder.TafsilGuid,
					DetailGuid = source.Guid,
					AsType = source.MasterOrder.FactorType,
					MasterDescription = source.MasterOrder.Description,
					DetailDescription = source.Description,
					MasterDate = source.MasterOrder.GetDate(),
					MasterGuid = source.MasterOrder.Guid,
					TempDescription = source.Serial
				};
				destination.Add(item);
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapBarcodeListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\TempBarcodePrintMapper.cs", 83);
			throw;
		}
	}

	public Task<List<TempBarcodePrintBusiness>> MapSerialListAsync(string contextGuid, OrderDetailBusiness source, List<TempBarcodePrintBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!(source?.SerialStr?.Any()).GetValueOrDefault())
			{
				return Task.FromResult(new List<TempBarcodePrintBusiness>());
			}
			if (destination == null)
			{
				destination = new List<TempBarcodePrintBusiness>();
			}
			foreach (PrdSerialBusiness item2 in source.SerialStr)
			{
				if (token.IsCancellationRequested)
				{
					throw new OperationCanceledException();
				}
				TempBarcodePrintBusiness item = new TempBarcodePrintBusiness(contextGuid)
				{
					Guid = Guid.NewGuid(),
					Modified = DateTime.Now,
					ProductGuid = source.ProductGuid,
					TafsilGuid = source.MasterOrder.TafsilGuid,
					DetailGuid = source.Guid,
					AsType = source.MasterOrder.FactorType,
					MasterDescription = source.MasterOrder.Description,
					DetailDescription = source.Description,
					MasterDate = source.MasterOrder.GetDate(),
					Barcode = item2.Serial,
					GuarantyCode = item2.GarrantyCode,
					GuarantyEndDate = item2.EndDate,
					GuarantyStartDate = item2.StartDate,
					MasterGuid = source.MasterOrder.Guid,
					TempDescription = source.Serial
				};
				destination.Add(item);
			}
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapSerialListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\Global\\TempBarcodePrintMapper.cs", 120);
			throw;
		}
	}
}
