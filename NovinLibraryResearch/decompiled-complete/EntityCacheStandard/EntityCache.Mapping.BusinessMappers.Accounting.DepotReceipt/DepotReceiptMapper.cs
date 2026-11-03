using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting.DepotReceipt;
using EntityCache.Business.Customers;
using EntityCache.Business.OrderDepot.OrderComparer;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting.DepotReceipt;

public class DepotReceiptMapper : IItemAsyncMapper<DepotReceiptBusiness, DepotReceiptBusiness>, IItemAsyncMapper<DepotReceiptBusiness, OrderBusiness>, IItemAsyncMapper<OrderBusiness, DepotReceiptBusiness>
{
	public List<OrderDetailComparerWrapper> Wrapper { get; private set; }

	public static DepotReceiptMapper GetInstance(List<OrderDetailComparerWrapper> wrapper = null)
	{
		return new DepotReceiptMapper
		{
			Wrapper = wrapper
		};
	}

	public Task<DepotReceiptBusiness> MapAsync(string contextGuid, DepotReceiptBusiness source, DepotReceiptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new DepotReceiptBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.Id2 = source.Id2;
			destination.DateM = source.DateM;
			destination.TafsilGuid = source.TafsilGuid;
			destination.Description = source.Description;
			destination.IsTemp = source.IsTemp;
			destination.CashierGuid = source.CashierGuid;
			destination.CarNumber = source.CarNumber;
			destination.DriverName = source.DriverName;
			destination.Type = source.Type;
			destination.BarnameNo = source.BarnameNo;
			destination.DriverPhone = source.DriverPhone;
			destination.RefrencedNumber = source.RefrencedNumber;
			destination.OrderGuid = source.OrderGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptMapper.cs", 57);
			throw;
		}
	}

	public async Task<OrderBusiness> MapAsync(string contextGuid, DepotReceiptBusiness source, OrderBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 9;
		try
		{
			Guid? priceTypeGuid = null;
			if (source == null)
			{
				destination = null;
				return destination;
			}
			Document type = ((source.Document == Document.DepotHavale) ? Document.OrderSell : Document.OrderBuy);
			if (destination == null)
			{
				destination = await OrderBusiness.GenerateNewWithDefaultsAsync(contextGuid, EnSystem.Mapper, type, ClsCache.GetCache(contextGuid).Source, source.TafsilGuid, source.Description);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			destination.SetId2(source.Id2);
			destination.ChangeStatus = EntityChangeSt.New;
			await destination.SetDateAsync(source.DateM, EnSystem.Mapper, token, tranName);
			if ((source.TafsilGuid ?? Guid.Empty) != Guid.Empty)
			{
				await destination.SetTafsilGuid(source.TafsilGuid.Value, EnSystem.Mapper, updateVisitorGuid: true, token, tranName);
				CusBusiness cusBusiness = await CusBusiness.GetAsync(contextGuid, EnSystem.Mapper, source.TafsilGuid.Value, token, tranName);
				if ((cusBusiness?.RoleGuid ?? Guid.Empty) != Guid.Empty)
				{
					CustomerRoleBusiness customerRoleBusiness = await cusBusiness.GetRoleAsync(token, tranName);
					if ((customerRoleBusiness?.PriceTypeGuid ?? Guid.Empty) != Guid.Empty)
					{
						priceTypeGuid = (destination.PriceTypeGuid = customerRoleBusiness.PriceTypeGuid);
					}
				}
			}
			destination.Description = source.Description;
			destination.UserAccGuid = source.CashierGuid;
			destination.FactorType = type;
			destination.BarnameNo = source.BarnameNo;
			if (type == Document.OrderBuy)
			{
				destination.SellerFactorNo = source.RefrencedNumber;
			}
			if (source.DetailList?.Any() ?? false)
			{
				foreach (DepotReceiptDetailBusiness item in source.DetailList)
				{
					if (!((item.ReferenceGuid ?? Guid.Empty) != Guid.Empty))
					{
						OrderDetailBusiness orderDet = await OrderDetailBusiness.OrderDetailFactoryAsync(contextGuid, destination, token, tranName);
						OrderDetailBusiness mappedOrderDet = await DepotReceiptDetailMapper.Instance.MapAsync(contextGuid, item, orderDet, tranName, token);
						List<ProductPriceRelationModel> list = await mappedOrderDet.GetAllPricesAsync(EnSystem.Mapper, token, tranName);
						mappedOrderDet.Guid = Guid.NewGuid();
						if ((priceTypeGuid ?? Guid.Empty) != Guid.Empty)
						{
							orderDet.PriceTypeGuid = priceTypeGuid;
						}
						await mappedOrderDet.SetPriceAsync(EnSystem.Mapper, (list?.FirstOrDefault()?.Price).GetValueOrDefault(), token, tranName);
						await destination.AddDetAsync(mappedOrderDet, EnSystem.Mapper, awaitOnSemaphor: true, token);
					}
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptMapper.cs", 125);
			throw;
		}
	}

	public async Task<DepotReceiptBusiness> MapAsync(string contextGuid, OrderBusiness source, DepotReceiptBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 4;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			switch (source.FactorType)
			{
			case Document.OrderBuy:
			case Document.OrderBackSell:
			case Document.AmaniVorod:
				destination.Type = EnDepotReceiptType.Receipt;
				break;
			case Document.OrderSell:
			case Document.OrderBackBuy:
			case Document.AmaniKhoroj:
				destination.Type = EnDepotReceiptType.Havale;
				break;
			}
			if (destination == null)
			{
				destination = await DepotReceiptBusiness.GenerateNewDepotReceiptAsync(contextGuid, destination.Type);
			}
			destination.ContextGuid = contextGuid;
			destination.ChangeStatus = EntityChangeSt.New;
			destination.Guid = Guid.NewGuid();
			destination.Modified = source.Modified;
			destination.Status = source.Status;
			DepotReceiptBusiness depotReceiptBusiness = destination;
			depotReceiptBusiness.Id2 = await DepotReceiptBusiness.GetNextNumberAsync(contextGuid, destination.Type, token, tranName);
			destination.DateM = source.GetDate();
			destination.TafsilGuid = source.TafsilGuid;
			destination.Description = source.Description;
			destination.CashierGuid = source.UserAccGuid;
			destination.IsTemp = !source.KardexAffective;
			destination.BarnameNo = source.BarnameNo;
			destination.OrderGuid = source.Guid;
			if (source.FactorType == Document.OrderBuy)
			{
				destination.RefrencedNumber = source.SellerFactorNo;
			}
			if (Wrapper?.Any() ?? false)
			{
				foreach (OrderDetailComparerWrapper item in Wrapper)
				{
					DepotReceiptDetailBusiness destination2 = await DepotReceiptDetailBusiness.GetFactoryAsync(contextGuid, destination, token, tranName);
					await destination.AddToDetailListAsync(await DepotReceiptDetailMapper.Instance.MapAsync(contextGuid, item, destination2, tranName, token), token);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\DepotReceipt\\DepotReceiptMapper.cs", 180);
			throw;
		}
	}

	public Task<List<DepotReceiptBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptBusiness> source, List<DepotReceiptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<OrderBusiness>> MapListAsync(string contextGuid, IEnumerable<DepotReceiptBusiness> source, List<OrderBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<DepotReceiptBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderBusiness> source, List<DepotReceiptBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
