using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Tolid;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.Tolid;

public class TolidExportMapper : IItemAsyncMapper<TolidFormulaExportBusiness, TolidExportBusiness>, IItemAsyncMapper<TolidFormulaExportBusiness, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailBusiness, TolidFormulaExportBusiness>, IItemAsyncMapper<TolidExportBusiness, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailBusiness, TolidExportBusiness>
{
	public static TolidExportMapper Instance { get; private set; } = new TolidExportMapper();


	public async Task<TolidExportBusiness> MapAsync(string contextGuid, TolidFormulaExportBusiness source, TolidExportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidExportBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = DateTime.Now;
			await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
			destination.DepotGuid = source.DepotGuid;
			destination.Description = source.Description;
			destination.Quantity = source.Quantity;
			destination.V2Guid = source.V2Guid;
			destination.Price = source.Price;
			destination.ContextGuid = contextGuid;
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				await destination.SetSerialListAsync(await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token), null, EnSystem.Mapper, clearList: true);
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidExportMapper.cs", 55);
			throw;
		}
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, TolidFormulaExportBusiness source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				OrderBusiness order = new OrderBusiness(contextGuid)
				{
					FactorType = Document.Tolid_Mahsul
				};
				destination = await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, order, token, tranName);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
			destination.Status = source.Status;
			await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
			await destination.SetDepotGuidAsync(source.DepotGuid);
			destination.Description = source.Description;
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
			await destination.SetVGuidAsync(EnSystem.Mapper, source.V2Guid, token, tranName);
			await destination.SetPriceAsync(EnSystem.Mapper, source.Price, token, tranName);
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				List<PrdSerialBusiness> list = await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token);
				if (list != null && list.Any())
				{
					foreach (PrdSerialBusiness item in list)
					{
						if (item != null)
						{
							await destination.SetSerialAsync(item, EnSystem.Mapper, clearList: true);
						}
					}
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidExportMapper.cs", 102);
			throw;
		}
	}

	public async Task<TolidFormulaExportBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, TolidFormulaExportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TolidFormulaExportBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
			await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
			TolidFormulaExportBusiness tolidFormulaExportBusiness = destination;
			tolidFormulaExportBusiness.DepotGuid = await source.GetDepotGuidAsync(token);
			destination.Description = source.Description;
			destination.Quantity = source.Quantity;
			destination.V2Guid = source.VGuid;
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				await destination.SetSerialListAsync(await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token), null, EnSystem.Mapper, clearList: true);
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidExportMapper.cs", 137);
			throw;
		}
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, TolidExportBusiness source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 7;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			short digitAfterPoint = (ClsCache.GetCache(contextGuid)?.MultiLanguageConfig?.DigitAfterPoint).GetValueOrDefault();
			if (destination == null)
			{
				OrderBusiness order = new OrderBusiness(contextGuid)
				{
					FactorType = Document.Tolid_Mahsul
				};
				destination = await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, order, token, tranName);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.ChangeInPrice = true;
			await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
			await destination.SetDepotGuidAsync(source.DepotGuid);
			destination.Description = source.Description;
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
			await destination.SetVGuidAsync(EnSystem.Mapper, source.V2Guid, token, tranName);
			await destination.SetPriceAsync(EnSystem.Mapper, Math.Round(source.Price, digitAfterPoint), token, tranName);
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.TolidFormulaIndex = source.TolidFormulaIndex;
			destination.ContextGuid = contextGuid;
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				List<PrdSerialBusiness> list = await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token);
				if (list != null && list.Any())
				{
					foreach (PrdSerialBusiness item in list)
					{
						if (item != null)
						{
							await destination.SetSerialAsync(item, EnSystem.Mapper, clearList: true);
						}
					}
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidExportMapper.cs", 189);
			throw;
		}
	}

	public async Task<TolidExportBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, TolidExportBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 3;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TolidExportBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = DateTime.Now;
			await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
			TolidExportBusiness tolidExportBusiness = destination;
			tolidExportBusiness.DepotGuid = await source.GetDepotGuidAsync(token);
			destination.Description = source.Description;
			destination.Quantity = source.Quantity;
			destination.V2Guid = source.VGuid;
			destination.Price = source.Price;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.TolidFormulaIndex = source.TolidFormulaIndex;
			destination.ContextGuid = contextGuid;
			if ((source?.SerialStr?.Any()).GetValueOrDefault())
			{
				await destination.SetSerialListAsync(await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token), null, EnSystem.Mapper, clearList: true);
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidExportMapper.cs", 228);
			throw;
		}
	}

	public Task<List<TolidExportBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<TolidExportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidExportBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaExportBusiness> source, List<TolidExportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaExportBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidFormulaExportBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<TolidFormulaExportBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidExportBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
