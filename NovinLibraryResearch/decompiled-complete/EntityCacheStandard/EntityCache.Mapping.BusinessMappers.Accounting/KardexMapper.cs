using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers.Accounting;

public class KardexMapper : IItemAsyncMapper<KardexBusiness, WebKardex>, IItemAsyncMapper<KardexBusiness, KardexBusiness>, IItemAsyncMapper<WebKardex, KardexBusiness>
{
	public static KardexMapper Instance { get; private set; } = new KardexMapper();


	public Task<WebKardex> MapAsync(string contextGuid, KardexBusiness source, WebKardex destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
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
				destination = new WebKardex();
			}
			destination.Guid = source.Guid;
			destination.Modified = source.Modified;
			destination.DepotGuid = source.DepotGuid;
			destination.DateM = source.DateM;
			destination.Discount = source.Discount;
			destination.DiscountForDisplay = source.DiscountForDisplay;
			destination.DiscountPercent = source.DiscountPercent;
			destination.DiscountPercentString = source.DiscountPercentString;
			destination.Fare = source.Fare;
			destination.VisitorPrice = source.VisitorPrice;
			destination.VisitorPriceForDisplay = source.VisitorPriceForDisplay;
			destination.Tax = source.Tax;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.DetailGuid = source.DetailGuid;
			destination.VorodQT = source.VorodQT;
			destination.VorodQtForDisplay = source.VorodQtForDisplay;
			destination.KhorojQT = source.KhorojQT;
			destination.KhorojQtForDisplay = source.KhorojQtForDisplay;
			destination.UnitPrice = source.UnitPrice;
			destination.UnitPriceForDisplay = source.UnitPriceForDisplay;
			destination.RemQT = source.RemQT;
			destination.RemQtForDisplay = source.RemQtForDisplay;
			destination.UnitSood = source.UnitSood;
			destination.UnitSoodForDisplay = source.UnitSoodForDisplay;
			destination.TotalSood = source.TotalSood;
			destination.TotalSoodForDisplay = source.TotalSoodForDisplay;
			destination.RowSood = source.RowSood;
			destination.RowSoodForDisplay = source.RowSoodForDisplay;
			destination.RemUnitPrice = source.RemUnitPrice;
			destination.RemUnitPriceForDisplay = source.RemUnitPriceForDisplay;
			destination.RowSum = source.RowSum;
			destination.RemTotalPrice = source.RemTotalPrice;
			destination.RemTotalPriceForDisplay = source.RemTotalPriceForDisplay;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.DescForDisplay = source.DescForDisplay;
			destination.RDescription = source.RDescription;
			destination.ID2 = source.ID2;
			destination.TotalHazine = source.TotalHazine;
			destination.Serial = source.Serial;
			destination.Avarez = source.Avarez;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.PrdCode = source.prdCode;
			destination.PrdName = source.PrdName;
			destination.Ordinal = source.Ordinal;
			destination.DepotName = source.DepotName;
			destination.IsChecked = source.IsChecked;
			destination.AmaniCount = source.AmaniCount;
			destination.V2Guid = source.V2Guid;
			destination.V2Qt = source.V2Qt;
			destination.Vahed2Name = source.Vahed2Name;
			destination.KardexAffective = source.KardexAffective;
			destination.SanadAffective = source.SanadAffective;
			destination.ProductGuid = source.GetProductGuid();
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			return Task.FromResult(destination);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\KardexMapper.cs", 89);
			throw;
		}
	}

	public async Task<KardexBusiness> MapAsync(string contextGuid, KardexBusiness source, KardexBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new KardexBusiness(contextGuid, null);
			}
			destination.ContextGuid = contextGuid;
			destination.Modified = source.Modified;
			await destination.SetProductGuidAsync(source.GetProductGuid(), EnSystem.Mapper, token, tranName);
			destination.DepotGuid = source.DepotGuid;
			destination.DateM = source.DateM;
			destination.Discount = source.Discount;
			destination.Fare = source.Fare;
			destination.VisitorPrice = source.VisitorPrice;
			destination.Tax = source.Tax;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.DetailGuid = source.DetailGuid;
			destination.VorodQT = source.VorodQT;
			destination.KhorojQT = source.KhorojQT;
			destination.UnitPrice = source.UnitPrice;
			destination.RemQT = source.RemQT;
			destination.UnitSood = source.UnitSood;
			destination.TotalSood = source.TotalSood;
			destination.RowSood = source.RowSood;
			destination.RemUnitPrice = source.RemUnitPrice;
			destination.RowSum = source.RowSum;
			destination.RemTotalPrice = source.RemTotalPrice;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.RDescription = source.RDescription;
			destination.ID2 = source.ID2;
			destination.TotalHazine = source.TotalHazine;
			destination.Serial = source.Serial;
			destination.Avarez = source.Avarez;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.IsChecked = source.IsChecked;
			destination.AmaniCount = source.AmaniCount;
			await destination.SetV2GuidAsync(source.V2Guid, token, tranName);
			destination.V2Qt = source.V2Qt;
			destination.DiscountPercentString = source.DiscountPercentString;
			destination.KardexAffective = source.KardexAffective;
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\KardexMapper.cs", 148);
			throw;
		}
	}

	public async Task<KardexBusiness> MapAsync(string contextGuid, WebKardex source, KardexBusiness destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			if (destination == null)
			{
				destination = new KardexBusiness(contextGuid, null);
			}
			destination.ContextGuid = contextGuid;
			await destination.SetProductGuidAsync(source.ProductGuid, EnSystem.Mapper, token, tranName);
			destination.DepotGuid = source.DepotGuid;
			destination.DateM = source.DateM;
			destination.Discount = source.Discount;
			destination.Fare = source.Fare;
			destination.VisitorPrice = source.VisitorPrice;
			destination.Tax = source.Tax;
			destination.SanadGuid = source.SanadGuid;
			destination.SanadType = source.SanadType;
			destination.DetailGuid = source.DetailGuid;
			destination.VorodQT = source.VorodQT;
			destination.KhorojQT = source.KhorojQT;
			destination.UnitPrice = source.UnitPrice;
			destination.RemQT = source.RemQT;
			destination.UnitSood = source.UnitSood;
			destination.TotalSood = source.TotalSood;
			destination.RowSood = source.RowSood;
			destination.RemUnitPrice = source.RemUnitPrice;
			destination.RowSum = source.RowSum;
			destination.RemTotalPrice = source.RemTotalPrice;
			destination.UserGuid = source.UserGuid;
			destination.Description = source.Description;
			destination.RDescription = source.RDescription;
			destination.ID2 = source.ID2;
			destination.TotalHazine = source.TotalHazine;
			destination.Serial = source.Serial;
			destination.Avarez = source.Avarez;
			destination.TafsilGuid = source.TafsilGuid;
			destination.MoeinGuid = source.MoeinGuid;
			destination.AmaniCount = source.AmaniCount;
			await destination.SetV2GuidAsync(source.V2Guid, token, tranName);
			destination.V2Qt = source.V2Qt;
			destination.DiscountPercentString = source.DiscountPercentString;
			destination.KardexAffective = source.KardexAffective;
			destination.IsTaxImpactOnCost = source.IsTaxImpactOnCost;
			destination.CostPrice = source.CostPrice;
			destination.TotalCostPrice = source.TotalCostPrice;
			destination.ProductTafsilGuid = source.ProductTafsilGuid;
			return destination;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Mapping\\BusinessMappers\\Accounting\\KardexMapper.cs", 205);
			throw;
		}
	}

	public Task<List<WebKardex>> MapListAsync(string contextGuid, IEnumerable<KardexBusiness> source, List<WebKardex> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<KardexBusiness>> MapListAsync(string contextGuid, IEnumerable<KardexBusiness> source, List<KardexBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public Task<List<KardexBusiness>> MapListAsync(string contextGuid, IEnumerable<WebKardex> source, List<KardexBusiness> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}
}
