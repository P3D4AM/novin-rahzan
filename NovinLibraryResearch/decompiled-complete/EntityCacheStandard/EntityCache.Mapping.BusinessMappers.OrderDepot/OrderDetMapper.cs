using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Accounting;
using EntityCache.Business.Global;
using EntityCache.Business.OrderDepot;
using EntityCache.Business.OrderDepot.OrderCouponCalculation;
using EntityCache.Mapping.BusinessMappers.Accounting;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Mapping;
using PacketParser.Settings.Accounting;

namespace EntityCache.Mapping.BusinessMappers.OrderDepot;

public class OrderDetMapper : IItemAsyncMapper<OrderDetailBusiness, WebOrderDetail>, IItemAsyncMapper<OrderDetailBusiness, OrderDetailBusiness>, IItemAsyncMapper<WebOrderDetail, OrderDetailBusiness>, IItemAsyncMapper<OrderDetailBusiness, OrderDetailCouponCalculationBusiness>
{
	public TaxUnits Setting { get; private set; }

	public OrderDetMapper(TaxUnits _setting)
	{
		Setting = _setting;
	}

	public static OrderDetMapper GetInstance(TaxUnits _setting)
	{
		return new OrderDetMapper(_setting);
	}

	public async Task<WebOrderDetail> MapAsync(string contextGuid, OrderDetailBusiness source, WebOrderDetail destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 6;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new WebOrderDetail();
			}
			destination.ProductGuid = source.ProductGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.Quantity = source.Quantity;
			destination.Price = source.Price;
			destination.Price2 = source.Price2;
			WebOrderDetail webOrderDetail = destination;
			webOrderDetail.DepotGuid = await source.GetDepotGuidAsync();
			destination.Description = source.Description;
			destination.Discount = source.Discount;
			destination.VGuid = source.VGuid;
			destination.V2Qt = source.V2Qt;
			destination.VisitorPrice1 = source.VisitorPrice1;
			destination.VisitorPrice2 = source.VisitorPrice2;
			destination.VisitorPrice3 = source.VisitorPrice3;
			destination.VisitorPrice4 = source.VisitorPrice4;
			destination.TotalHazinePrice = source.TotalHazinePrice;
			destination.Tax = source.Tax;
			destination.Avarez = source.Avarez;
			destination.V2Price = source.V2Price;
			destination.Hazine1Price = source.Hazine1Price;
			destination.Hazine2Price = source.Hazine2Price;
			destination.Hazine3Price = source.Hazine3Price;
			destination.Hazine4Price = source.Hazine4Price;
			destination.Hazine5Price = source.Hazine5Price;
			destination.DecimalGuid = source.DecimalGuid;
			destination.DecimalQt = source.DecimalQt;
			destination.DecimalPrice = source.DecimalPrice;
			destination.DecimalSum = source.DecimalSum;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.OrderDetState = source.OrderDetState;
			destination.IsEdit = source.IsEdit;
			destination.Ordinal = source.Ordinal;
			if (source.SerialzedData != null)
			{
				destination.SerialzedData = new OrderDetailSerializedData
				{
					Visitor1Mode = source.SerialzedData.Visitor1Mode,
					Visitor2Mode = source.SerialzedData.Visitor2Mode,
					Visitor3Mode = source.SerialzedData.Visitor3Mode,
					Visitor4Mode = source.SerialzedData.Visitor4Mode,
					DiscountMode = source.SerialzedData.DiscountMode,
					TaxMode = source.SerialzedData.TaxMode,
					AvarezMode = source.SerialzedData.AvarezMode,
					FeePercent = source.SerialzedData.FeePercent,
					FeeTaxPercent = source.SerialzedData.FeeTaxPercent,
					NetWeight = source.SerialzedData.NetWeight,
					FeeTaxPrice = source.SerialzedData.FeeTaxPrice,
					FeeType = source.SerialzedData.FeeType,
					GrossWeight = source.SerialzedData.GrossWeight,
					ProductTaxPercent = source.SerialzedData.ProductTaxPercent,
					ProductTaxPrice = source.SerialzedData.ProductTaxPrice,
					ProfitPercent = source.SerialzedData.ProfitPercent,
					ProfitTaxPercent = source.SerialzedData.ProfitTaxPercent,
					ProfitTaxPrice = source.SerialzedData.ProfitTaxPrice,
					ProfitType = source.SerialzedData.ProfitType,
					SellPrice = source.SerialzedData.SellPrice,
					PackingHeight = source.SerialzedData.PackingHeight,
					PackingLength = source.SerialzedData.PackingLength,
					PackingWidth = source.SerialzedData.PackingWidth
				};
			}
			destination.DiscountPercentString = source.DiscountPercentString;
			destination.TaxPercent = source.TaxPercent;
			destination.AvarezPercent = source.AvarezPercent;
			destination.ProductName = source.ProductName;
			ProductCompactBusiness prd = await source.GetPrdInfoAsync(EnSystem.Mapper, token, tranName);
			if (prd != null)
			{
				IEnumerable<ProductMetaDataBusiness> obj = await ProductBusiness.GetMetaDataListAsync(contextGuid, prd.Guid, token, tranName);
				ProductMetaDataBusiness productMetaDataBusiness = obj?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.TaxCode);
				ProductMetaDataBusiness productMetaDataBusiness2 = obj?.SingleOrDefault((ProductMetaDataBusiness q) => q.MetaKey == ProductMetaKey.TaxName);
				destination.TaxCode = productMetaDataBusiness?.Value?.Trim();
				destination.TaxName = productMetaDataBusiness2?.Value;
				if (Setting?.UnitList != null)
				{
					PrdVahedBusiness vahed = await prd.GetVahedAsync(token, tranName);
					if ((vahed?.TaxUnitGuid ?? Guid.Empty) != Guid.Empty)
					{
						destination.TaxUnitCode = Setting.UnitList.FirstOrDefault((WebTaxUnit q) => q.Guid == vahed.TaxUnitGuid.Value)?.Code ?? 0;
						destination.TaxUnitName = Setting.UnitList.FirstOrDefault((WebTaxUnit q) => q.Guid == vahed.TaxUnitGuid.Value)?.Name ?? "";
					}
				}
			}
			destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.FeePrice = source.FeePrice;
			destination.ProfitPrice = source.ProfitPrice;
			destination.GrouppingGuid = source.GrouppingGuid;
			destination.GoldPrice = source.GoldPrice;
			destination.DecimalTotalPrice = source.DecimalTotalPrice;
			destination.TotalTaxPrice = source.TotalTaxPrice;
			destination.MetaSellPrice = source.MetaSellPrice;
			destination.Serial = source.Serial;
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			destination.SteppedDiscount = source.SteppedDiscount;
			webOrderDetail = destination;
			webOrderDetail.ProductTafsilGuid = await source.GetProductTafsilGuidAsync(token);
			if ((source?.DecimalGuid ?? Guid.Empty) != Guid.Empty)
			{
				destination.DecimalSign = (await DecimalBusiness.GetAsync(contextGuid, source.DecimalGuid.Value, token, tranName))?.Abstract ?? "";
			}
			if (source.SerialStr?.Any() ?? false)
			{
				webOrderDetail = destination;
				webOrderDetail.SerialStr = await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<WebPrdSerial>(), tranName, token);
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderDetMapper.cs", 155);
			throw;
		}
	}

	public Task<List<WebOrderDetail>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<WebOrderDetail> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, OrderDetailBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new Exception("دوست عزیز لطفا ابتدا destination رو درست کن و بعد بده دست من!".GetTranslate() + "\r\nMap=>OrderDetailBusiness source, OrderDetailBusiness destination");
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
			destination.ChangeInPrice = true;
			destination.ChangeInDiscount = true;
			destination.ChangeInPrice = true;
			await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity);
			destination.Price2 = source.Price2;
			await destination.SetPriceAsync(EnSystem.Mapper, source.Price);
			OrderDetailBusiness orderDetailBusiness = destination;
			await orderDetailBusiness.SetDepotGuidAsync(await source.GetDepotGuidAsync());
			destination.Description = source.Description;
			await destination.SetDiscountAsync(EnSystem.Mapper, source.Discount);
			await destination.SetVGuidAsync(EnSystem.Mapper, source.VGuid);
			await destination.SetV2QtAsync(EnSystem.Mapper, source.V2Qt);
			destination.VisitorPrice1 = source.VisitorPrice1;
			destination.VisitorPrice2 = source.VisitorPrice2;
			destination.VisitorPrice3 = source.VisitorPrice3;
			destination.VisitorPrice4 = source.VisitorPrice4;
			destination.TotalHazinePrice = source.TotalHazinePrice;
			destination.SetTax(EnSystem.Mapper, source.Tax);
			destination.Avarez = source.Avarez;
			destination.V2Price = source.V2Price;
			destination.Hazine1Price = source.Hazine1Price;
			destination.Hazine2Price = source.Hazine2Price;
			destination.Hazine3Price = source.Hazine3Price;
			destination.Hazine4Price = source.Hazine4Price;
			destination.Hazine5Price = source.Hazine5Price;
			destination.DecimalGuid = source.DecimalGuid;
			await destination.SetDecimalQuantityAsync(EnSystem.Mapper, source.DecimalQt, handleSameProducts: false, token);
			destination.DecimalPrice = source.DecimalPrice;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.OrderDetState = source.OrderDetState;
			destination.VisitorPrice1 = source.VisitorPrice1;
			destination.VisitorPrice2 = source.VisitorPrice2;
			destination.VisitorPrice3 = source.VisitorPrice3;
			destination.VisitorPrice4 = source.VisitorPrice4;
			destination.ChangeInDiscount = source.ChangeInDiscount;
			destination.ChangeInPrice = source.ChangeInPrice;
			destination.IsEdit = source.IsEdit;
			destination.Modified = source.Modified;
			destination.Ordinal = source.Ordinal;
			if (source.SerialzedData.TaxMode == PercentageMode.Percent)
			{
				destination.TaxPercent = source.TaxPercent;
			}
			destination.AvarezPercent = source.AvarezPercent;
			List<PrdSerialBusiness> list = await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token);
			if (list != null && list.Any())
			{
				foreach (PrdSerialBusiness item in list)
				{
					await destination.SetSerialAsync(item, EnSystem.Mapper);
				}
			}
			orderDetailBusiness = destination;
			orderDetailBusiness.AbadStr = await OrderAbadMapper.Instance.MapAsync(contextGuid, source.AbadStr, new OrderAbadBusiness(contextGuid), tranName, token);
			if (source.SerialzedData != null)
			{
				OrderDetailSerializedData serialzedDataAsync = new OrderDetailSerializedData
				{
					Visitor1Mode = source.SerialzedData.Visitor1Mode,
					Visitor2Mode = source.SerialzedData.Visitor2Mode,
					Visitor3Mode = source.SerialzedData.Visitor3Mode,
					Visitor4Mode = source.SerialzedData.Visitor4Mode,
					DiscountMode = source.SerialzedData.DiscountMode,
					TaxMode = source.SerialzedData.TaxMode,
					AvarezMode = source.SerialzedData.AvarezMode,
					FeePercent = source.SerialzedData.FeePercent,
					FeeTaxPercent = source.SerialzedData.FeeTaxPercent,
					NetWeight = source.SerialzedData.NetWeight,
					FeeTaxPrice = source.SerialzedData.FeeTaxPrice,
					FeeType = source.SerialzedData.FeeType,
					GrossWeight = source.SerialzedData.GrossWeight,
					ProductTaxPercent = source.SerialzedData.ProductTaxPercent,
					ProductTaxPrice = source.SerialzedData.ProductTaxPrice,
					ProfitPercent = source.SerialzedData.ProfitPercent,
					ProfitTaxPercent = source.SerialzedData.ProfitTaxPercent,
					ProfitTaxPrice = source.SerialzedData.ProfitTaxPrice,
					ProfitType = source.SerialzedData.ProfitType,
					SellPrice = source.SerialzedData.SellPrice,
					PackingHeight = source.SerialzedData.PackingHeight,
					PackingLength = source.SerialzedData.PackingLength,
					PackingWidth = source.SerialzedData.PackingWidth
				};
				await destination.SetSerialzedDataAsync(serialzedDataAsync);
			}
			destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
			destination.ReferenceGuid = source.ReferenceGuid;
			destination.TolidFormulaGuid = source.TolidFormulaGuid;
			await destination.SetProfitPrice(EnSystem.Mapper, source.ProfitPrice, token);
			await destination.SetFeePriceAsync(EnSystem.Mapper, source.FeePrice, token);
			destination.GrouppingGuid = source.GrouppingGuid;
			destination.Serial = source.Serial;
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			orderDetailBusiness = destination;
			orderDetailBusiness.ProductTafsilGuid = await source.GetProductTafsilGuidAsync(token);
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			await destination.SetSteppedDiscountAsync(source.SteppedDiscount, EnSystem.Mapper, token);
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderDetMapper.cs", 273);
			throw;
		}
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public async Task<OrderDetailBusiness> MapAsync(string contextGuid, WebOrderDetail source, OrderDetailBusiness destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		if (destination == null)
		{
			throw new Exception("دوست عزیز لطفا ابتدا destination رو درست کن و بعد بده دست من!".GetTranslate() + "\r\nmap=>WebOrderDetail source, OrderDetailBusiness destination");
		}
		try
		{
			if (destination?.IsDisposed ?? false)
			{
				return null;
			}
			if (source == null)
			{
				destination = null;
				return destination;
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			await destination.SetProductGuidAsync(EnSystem.Mapper, source.ProductGuid, isLoadOriginalPrice: true, tranName, token);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity, token, tranName);
			destination.Price2 = source.Price2;
			await destination.SetPriceAsync(EnSystem.Mapper, source.Price);
			await destination.SetDepotGuidAsync(source.DepotGuid);
			destination.Description = source.Description;
			await destination.SetVGuidAsync(EnSystem.Mapper, source.VGuid);
			await destination.SetV2QtAsync(EnSystem.Mapper, source.V2Qt);
			destination.TotalHazinePrice = source.TotalHazinePrice;
			destination.V2Price = source.V2Price;
			destination.Hazine1Price = source.Hazine1Price;
			destination.Hazine2Price = source.Hazine2Price;
			destination.Hazine3Price = source.Hazine3Price;
			destination.Hazine4Price = source.Hazine4Price;
			destination.Hazine5Price = source.Hazine5Price;
			destination.DecimalGuid = source.DecimalGuid;
			await destination.SetDecimalQuantityAsync(EnSystem.Mapper, source.DecimalQt, handleSameProducts: false, token);
			destination.DecimalPrice = source.DecimalPrice;
			destination.PriceTypeGuid = source.PriceTypeGuid;
			destination.OrderDetState = source.OrderDetState;
			await destination.SetV2QtAsync(EnSystem.Mapper, source.V2Qt);
			await destination.SetQuantityAsync(EnSystem.Mapper, source.Quantity);
			await destination.SetPriceAsync(EnSystem.Mapper, source.Price);
			await destination.SetDiscountAsync(EnSystem.Mapper, source.Discount);
			destination.VisitorPrice1String = source.VisitorPrice1.ToString();
			destination.VisitorPrice2String = source.VisitorPrice2.ToString();
			destination.VisitorPrice3String = source.VisitorPrice3.ToString();
			destination.VisitorPrice4String = source.VisitorPrice4.ToString();
			destination.TaxPercent = source.TaxPercent;
			destination.AvarezPercent = source.AvarezPercent;
			destination.SetTax(EnSystem.Mapper, source.Tax);
			destination.Avarez = source.Avarez;
			destination.ChangeInDiscount = false;
			destination.ChangeInPrice = false;
			destination.IsEdit = source.IsEdit;
			destination.Ordinal = source.Ordinal;
			destination.ContextGuid = contextGuid;
			if (source.SerialzedData != null)
			{
				OrderDetailSerializedData serialzedDataAsync = new OrderDetailSerializedData
				{
					Visitor1Mode = source.SerialzedData.Visitor1Mode,
					Visitor2Mode = source.SerialzedData.Visitor2Mode,
					Visitor3Mode = source.SerialzedData.Visitor3Mode,
					Visitor4Mode = source.SerialzedData.Visitor4Mode,
					DiscountMode = source.SerialzedData.DiscountMode,
					TaxMode = source.SerialzedData.TaxMode,
					AvarezMode = source.SerialzedData.AvarezMode,
					FeePercent = source.SerialzedData.FeePercent,
					FeeTaxPercent = source.SerialzedData.FeeTaxPercent,
					NetWeight = source.SerialzedData.NetWeight,
					FeeTaxPrice = source.SerialzedData.FeeTaxPrice,
					FeeType = source.SerialzedData.FeeType,
					GrossWeight = source.SerialzedData.GrossWeight,
					ProductTaxPercent = source.SerialzedData.ProductTaxPercent,
					ProductTaxPrice = source.SerialzedData.ProductTaxPrice,
					ProfitPercent = source.SerialzedData.ProfitPercent,
					ProfitTaxPercent = source.SerialzedData.ProfitTaxPercent,
					ProfitTaxPrice = source.SerialzedData.ProfitTaxPrice,
					ProfitType = source.SerialzedData.ProfitType,
					SellPrice = source.SerialzedData.SellPrice,
					PackingHeight = source.SerialzedData.PackingHeight,
					PackingLength = source.SerialzedData.PackingLength,
					PackingWidth = source.SerialzedData.PackingWidth
				};
				await destination.SetSerialzedDataAsync(serialzedDataAsync);
			}
			destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
			destination.ReferenceGuid = source.ReferenceGuid;
			await destination.SetProfitPrice(EnSystem.Mapper, source.ProfitPrice, token);
			await destination.SetFeePriceAsync(EnSystem.Mapper, source.FeePrice, token);
			destination.GrouppingGuid = source.GrouppingGuid;
			destination.Serial = source.Serial;
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			await destination.SetSteppedDiscountAsync(source.SteppedDiscount, EnSystem.Mapper, token);
			if (source.SerialStr?.Any() ?? false)
			{
				foreach (PrdSerialBusiness item in await PrdSerialMapper.Instance.MapListAsync(contextGuid, source.SerialStr, new List<PrdSerialBusiness>(), tranName, token))
				{
					await destination.SetSerialAsync(item, EnSystem.Mapper);
				}
			}
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderDetMapper.cs", 384);
			throw;
		}
	}

	public Task<List<OrderDetailBusiness>> MapListAsync(string contextGuid, IEnumerable<WebOrderDetail> source, List<OrderDetailBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<OrderDetailCouponCalculationBusiness> MapAsync(string contextGuid, OrderDetailBusiness source, OrderDetailCouponCalculationBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new OrderDetailCouponCalculationBusiness(contextGuid);
			}
			destination.ContextGuid = contextGuid;
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DicountCouponDetailGuid = source.DicountCouponDetailGuid;
			destination.ProductGuid = source.ProductGuid;
			destination.Quantity = source.Quantity;
			destination.Price = source.Price;
			destination.Discount = source.Discount;
			destination.RowSum = source.RowSum;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\OrderDepot\\OrderDetMapper.cs", 413);
			throw;
		}
	}

	public Task<List<OrderDetailCouponCalculationBusiness>> MapListAsync(string contextGuid, IEnumerable<OrderDetailBusiness> source, List<OrderDetailCouponCalculationBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
