using System;
using System.Collections.Generic;
using System.Linq;
using PacketParser;
using PacketParser.Entities;

namespace EntityCache.Ui;

public class PriceTypeForDisplay
{
	public EnPriceType PriceType { get; private set; }

	public short Value => (short)PriceType;

	public string Title => PriceType.GetDisplay().GetTranslate();

	public PriceTypeForDisplay(EnPriceType priceType)
	{
		PriceType = priceType;
	}

	public static List<PriceTypeForDisplay> GetPriceType(Document? orderType, bool isLoadNone, bool loadLastSell = true)
	{
		List<PriceTypeForDisplay> list = new List<PriceTypeForDisplay>();
		try
		{
			if (!orderType.HasValue)
			{
				list = LoadSellPrice(loadLastSell);
				return list;
			}
			list = ((orderType != Document.OrderSell && orderType != Document.OrderBackSell && orderType != Document.PishFactor && orderType != Document.AmaniKhoroj) ? ((orderType != Document.OrderBuy && orderType != Document.OrderBackBuy && orderType != Document.AmaniVorod && orderType != Document.OrderPreBuy) ? LoadSellPrice(loadLastSell) : LoadBuyPrice()) : LoadSellPrice(loadLastSell));
			if (isLoadNone)
			{
				list.Add(new PriceTypeForDisplay(EnPriceType.none));
			}
			list = list?.OrderBy((PriceTypeForDisplay q) => q.Value)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPriceType", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Ui\\PriceTypeForDisplay.cs", 36);
		}
		return list;
	}

	private static List<PriceTypeForDisplay> LoadSellPrice(bool loadLastSell)
	{
		List<PriceTypeForDisplay> list = new List<PriceTypeForDisplay>();
		try
		{
			if (loadLastSell)
			{
				list.Add(new PriceTypeForDisplay(EnPriceType.LastSellToAll));
				list.Add(new PriceTypeForDisplay(EnPriceType.LastSellToCus));
			}
			list.Add(new PriceTypeForDisplay(EnPriceType.sell1));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell2));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell3));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell4));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell5));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell6));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell7));
			list.Add(new PriceTypeForDisplay(EnPriceType.sell8));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadSellPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Ui\\PriceTypeForDisplay.cs", 58);
		}
		return list;
	}

	private static List<PriceTypeForDisplay> LoadBuyPrice()
	{
		List<PriceTypeForDisplay> list = new List<PriceTypeForDisplay>();
		try
		{
			list.Add(new PriceTypeForDisplay(EnPriceType.Buy1));
			list.Add(new PriceTypeForDisplay(EnPriceType.Buy2));
			list.Add(new PriceTypeForDisplay(EnPriceType.Buy3));
			list.Add(new PriceTypeForDisplay(EnPriceType.BuyLast));
			list.Add(new PriceTypeForDisplay(EnPriceType.BuyAverage));
			list.Add(new PriceTypeForDisplay(EnPriceType.BuyLastFromAll));
			list.Add(new PriceTypeForDisplay(EnPriceType.BuyLastFromCus));
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "LoadBuyPrice", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Ui\\PriceTypeForDisplay.cs", 74);
		}
		return list;
	}
}
