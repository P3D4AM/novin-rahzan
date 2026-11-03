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

public class TolidMavadMapper : IItemAsyncMapper<TolidFormulaMavadBusiness, TolidMavadBusiness>, IItemAsyncMapper<TolidFormulaMavadBusiness, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailBusiness, TolidFormulaMavadBusiness>, IItemAsyncMapper<TolidMavadBusiness, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailBusiness, TolidMavadBusiness>
{
	public static TolidMavadMapper Instance { get; private set; } = new TolidMavadMapper();


	public async Task<TolidMavadBusiness> MapAsync(string contextGuid, TolidFormulaMavadBusiness source, TolidMavadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new TolidMavadBusiness(contextGuid);
			}
			destination.Modified = DateTime.Now;
			destination.Guid = source.Guid;
			await destination.SetProductGuid(source.ProductGuid, EnSystem.Mapper, tranName);
			await destination.SetDepotGuid(source.DepotGuid, EnSystem.Mapper, tranName);
			destination.Quantity = source.Quantity;
			destination.Serial = source.Serial;
			destination.Description = source.Description;
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMavadMapper.cs", 56);
			throw;
		}
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, TolidFormulaMavadBusiness source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					FactorType = Document.Tolid_Mavad
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMavadMapper.cs", 103);
			throw;
		}
	}

	public async Task<TolidFormulaMavadBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, TolidFormulaMavadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TolidFormulaMavadBusiness(contextGuid);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
			await destination.SetProductGuid(source.ProductGuid, EnSystem.Mapper, tranName);
			TolidFormulaMavadBusiness tolidFormulaMavadBusiness = destination;
			await tolidFormulaMavadBusiness.SetDepotGuid(await source.GetDepotGuidAsync(), EnSystem.Mapper, tranName);
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMavadMapper.cs", 139);
			throw;
		}
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, TolidMavadBusiness source, OrderDetailBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
					FactorType = Document.Tolid_Mavad
				};
				destination = await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, order, token, tranName);
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.ContextGuid = contextGuid;
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMavadMapper.cs", 191);
			throw;
		}
	}

	public async Task<TolidMavadBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, TolidMavadBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new TolidMavadBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Modified = DateTime.Now;
			destination.Guid = source.Guid;
			await destination.SetProductGuid(source.ProductGuid, EnSystem.Mapper, tranName);
			TolidMavadBusiness tolidMavadBusiness = destination;
			await tolidMavadBusiness.SetDepotGuid(await source.GetDepotGuidAsync(), EnSystem.Mapper, tranName);
			destination.Quantity = source.Quantity;
			destination.Description = source.Description;
			destination.V2Guid = source.VGuid;
			destination.Price = source.Price;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			destination.TolidFormulaIndex = source.TolidFormulaIndex;
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
			WebErrorLog.Instance.StartLog(ex2, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\Tolid\\TolidMavadMapper.cs", 230);
			throw;
		}
	}

	public Task<List<TolidMavadBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<TolidMavadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidMavadBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaMavadBusiness> source, List<TolidMavadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidMavadBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<TolidFormulaMavadBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<TolidFormulaMavadBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<TolidFormulaMavadBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
