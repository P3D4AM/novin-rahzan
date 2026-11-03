using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Products;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class KardexTajmieReportBusiness : IHasContext
{
	public ProductCompactBusiness PrdInfo { get; private set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public string DepotName { get; set; }

	public string ProductCode { get; set; }

	public string ProductName { get; set; }

	public string VahedName { get; set; }

	public string GroupName { get; set; }

	public decimal Amount { get; set; }

	public string TechnicalInfo1 { get; set; }

	public string TechnicalInfo2 { get; set; }

	public string TechnicalInfo3 { get; set; }

	public string TechnicalInfo4 { get; set; }

	public string TechnicalInfo5 { get; set; }

	public string TechnicalInfo6 { get; set; }

	public string TechnicalInfo7 { get; set; }

	public string TechnicalInfo8 { get; set; }

	public decimal BPercent1 { get; set; }

	public decimal BPercent2 { get; set; }

	public decimal BPercent3 { get; set; }

	public decimal BPercent4 { get; set; }

	public decimal DiscountPercent { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V3Qt { get; set; }

	public decimal V4Qt { get; set; }

	public string V2Name { get; set; }

	public string V3Name { get; set; }

	public string V4Name { get; set; }

	public Guid? V2Guid { get; set; }

	public Guid? V3Guid { get; set; }

	public Guid? V4Guid { get; set; }

	public decimal Buy1 { get; private set; }

	public decimal Buy2 { get; private set; }

	public decimal Buy3 { get; private set; }

	public decimal Sell1 { get; private set; }

	public decimal Sell2 { get; private set; }

	public decimal Sell3 { get; private set; }

	public decimal Sell4 { get; private set; }

	public decimal Sell5 { get; private set; }

	public decimal Sell6 { get; private set; }

	public decimal Sell7 { get; private set; }

	public decimal Sell8 { get; private set; }

	public decimal BuyLast { get; private set; }

	public decimal SD_Debit { get; set; }

	public decimal SD_Credit { get; set; }

	public decimal SD_Sood { get; set; }

	public double SD_VorodQT { get; set; }

	public double SD_KhorojQT { get; set; }

	public double SD_BuyQT { get; set; }

	public decimal SD_BuyPrice { get; set; }

	public double SD_SellQT { get; set; }

	public decimal SD_SellSum { get; set; }

	public double SD_BBuyQT { get; set; }

	public decimal SD_BBuySum { get; set; }

	public double SD_BSellQT { get; set; }

	public decimal SD_BSellSum { get; set; }

	public double SD_Tolid_in_QT { get; set; }

	public decimal SD_Tolid_in_Sum { get; set; }

	public double SD_Tolid_Out_QT { get; set; }

	public decimal SD_Tolid_Out_Sum { get; set; }

	public double SD_Zayeat_QT { get; set; }

	public decimal SD_Zayeat_Sum { get; set; }

	public decimal SD_Discount_Credit { get; set; }

	public decimal SD_Discount_Debit { get; set; }

	public decimal SD_Keraye_Credit { get; set; }

	public decimal SD_Keraye_Debit { get; set; }

	public decimal SD_Bazaryab_Credit { get; set; }

	public decimal SD_BazaryabDebit { get; set; }

	public decimal DD_Debit { get; set; }

	public decimal DD_Credit { get; set; }

	public decimal DD_Sood { get; set; }

	public double DD_VorodQT { get; set; }

	public double DD_KhorojQT { get; set; }

	public decimal SellSum { get; set; }

	public double SellQt { get; set; }

	public decimal SellSood { get; set; }

	public decimal BackSellSum { get; set; }

	public double BackSellQt { get; set; }

	public decimal BackSellZian { get; set; }

	public decimal BuySum { get; set; }

	public double BuyQt { get; set; }

	public decimal BackBuySum { get; set; }

	public double BackBuyQt { get; set; }

	public decimal ZayeatSum { get; set; }

	public double ZayeatQt { get; set; }

	public decimal TolidVorodSum { get; set; }

	public double TolidVorodQt { get; set; }

	public decimal TolidKhorojSum { get; set; }

	public double TolidKhorojQt { get; set; }

	public decimal PishFactorSum { get; set; }

	public double PishFactorCount { get; set; }

	public decimal DD_Discount_Credit { get; set; }

	public decimal DD_Discount_Debit { get; set; }

	public decimal DD_Keraye_Credit { get; set; }

	public decimal DD_Keraye_Debit { get; set; }

	public decimal DD_Bazaryab_Credit { get; set; }

	public decimal DD_BazaryabDebit { get; set; }

	public decimal ED_Debit { get; set; }

	public decimal ED_Credit { get; set; }

	public decimal ED_Sood { get; set; }

	public double ED_VorodQT { get; set; }

	public double ED_KhorojQT { get; set; }

	public double ED_BuyQT { get; set; }

	public decimal ED_BuyPrice { get; set; }

	public double ED_SellQT { get; set; }

	public decimal ED_SellSum { get; set; }

	public double ED_BBuyQT { get; set; }

	public decimal ED_BBuySum { get; set; }

	public double ED_BSellQT { get; set; }

	public decimal ED_BSellSum { get; set; }

	public double ED_Tolid_in_QT { get; set; }

	public decimal ED_Tolid_in_Sum { get; set; }

	public double ED_Tolid_Out_QT { get; set; }

	public decimal ED_Tolid_Out_Sum { get; set; }

	public double ED_Zayeat_QT { get; set; }

	public decimal ED_Zayeat_Sum { get; set; }

	public decimal ED_Discount_Credit { get; set; }

	public decimal ED_Discount_Debit { get; set; }

	public decimal ED_Keraye_Credit { get; set; }

	public decimal ED_Keraye_Debit { get; set; }

	public decimal ED_Bazaryab_Credit { get; set; }

	public decimal ED_BazaryabDebit { get; set; }

	public decimal Rem_Debit { get; set; }

	public decimal Rem_Credit { get; set; }

	public decimal Rem_Sood { get; set; }

	public double Rem_VorodQT { get; set; }

	public double Rem_KhorojQT { get; set; }

	public double Rem_BuyQT { get; set; }

	public decimal Rem_BuyPrice { get; set; }

	public double Rem_SellQT { get; set; }

	public decimal Rem_SellSum { get; set; }

	public double Rem_BBuyQT { get; set; }

	public decimal Rem_BBuySum { get; set; }

	public double Rem_BSellQT { get; set; }

	public decimal Rem_BSellSum { get; set; }

	public double Rem_Tolid_in_QT { get; set; }

	public decimal Rem_Tolid_in_Sum { get; set; }

	public double Rem_Tolid_Out_QT { get; set; }

	public decimal Rem_Tolid_Out_Sum { get; set; }

	public double Rem_Zayeat_QT { get; set; }

	public decimal Rem_Zayeat_Sum { get; set; }

	public decimal Rem_Discount_Credit { get; set; }

	public decimal Rem_Discount_Debit { get; set; }

	public decimal Rem_Keraye_Credit { get; set; }

	public decimal Rem_Keraye_Debit { get; set; }

	public decimal Rem_Bazaryab_Credit { get; set; }

	public decimal Rem_BazaryabDebit { get; set; }

	public string ContextGuid { get; set; }

	public decimal AmaniVorodSum { get; set; }

	public double AmaniVorodCount { get; set; }

	public decimal AmaniKhorojSum { get; set; }

	public double AmaniKhorojCount { get; set; }

	public decimal PreBuySum { get; set; }

	public double PreBuyCount { get; set; }

	public KardexTajmieReportBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task SetProductAsync(EnSystem system, CancellationToken token = default(CancellationToken))
	{
		_ = 2;
		try
		{
			if (system != EnSystem.UserInterface || !(ProductGuid != Guid.Empty) || !((PrdInfo?.Guid ?? Guid.Empty) != ProductGuid))
			{
				return;
			}
			PrdInfo = await ProductCompactBusiness.GetAsync(ContextGuid, ProductGuid, system, null, "", token, true);
			if (PrdInfo == null)
			{
				return;
			}
			IEnumerable<ProductPriceRelationModel> productPrices = (await PrdInfo.GetAllPricesAsync(system, null, token))?.Where((ProductPriceRelationModel q) => q.Status);
			if (productPrices?.Any() ?? false)
			{
				Buy1 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Buy && q.Ordinal == 1)?.Price ?? 0m;
				Buy2 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Buy && q.Ordinal == 2)?.Price ?? 0m;
				Buy3 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Buy && q.Ordinal == 3)?.Price ?? 0m;
				BuyLast = await ProductPriceRelationBusiness.GetLastBuyDecimalAsync(ContextGuid, system, ProductGuid, token);
				Sell1 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 1)?.Price ?? 0m;
				Sell2 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 2)?.Price ?? 0m;
				Sell3 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 3)?.Price ?? 0m;
				Sell4 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 4)?.Price ?? 0m;
				Sell5 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 5)?.Price ?? 0m;
				Sell6 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 6)?.Price ?? 0m;
				Sell7 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 7)?.Price ?? 0m;
				Sell8 = productPrices.SingleOrDefault((ProductPriceRelationModel q) => q.Type == EnPriceLevelType.Sell && q.Ordinal == 8)?.Price ?? 0m;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetProductAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\KardexTajmieReportBusiness.cs", 211);
			throw;
		}
	}
}
