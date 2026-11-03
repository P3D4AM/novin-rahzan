using System;
using System.Collections.Generic;
using System.Linq;
using EntityCache.Business.Products;
using PacketParser;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;

namespace AccSqlServerPersistence.Ertegha;

public class DefualtPriceLevelObjs
{
	public static List<ProductPriceLevelBusiness> ListAllDefault(string contextGuid)
	{
		try
		{
			return new List<ProductPriceLevelBusiness>
			{
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell1, "[خرده فروشی]".GetTranslate(), 1, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell2, "[جزیی]".GetTranslate(), 2, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell3, "[کلی]".GetTranslate(), 3, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell4, "[همکار درجه]".GetTranslate() + "1", 4, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell5, "[همکار درجه]".GetTranslate() + "2", 5, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell6, "[همکار درجه]".GetTranslate() + "3", 6, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell7, "[فصلی]".GetTranslate(), 7, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Sell8, "[نمایشگاه]".GetTranslate(), 8, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.RegularPrice, "[قیمت مصرف کننده]".GetTranslate(), 9, EnPriceLevelType.Sell, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.LastSellToAll, "[آخرین فروش به تمام مشتریان]".GetTranslate(), 14, EnPriceLevelType.Sell, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.LastSellToTafsil, "[آخرین فروش به مشتری خاص]".GetTranslate(), 15, EnPriceLevelType.Sell, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.CurrentSaved, "[ثبت شده در آخرین ویرایش]".GetTranslate(), 20, EnPriceLevelType.Sell, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Buy1, "[نقدی]".GetTranslate(), 10, EnPriceLevelType.Buy, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Buy2, "یک ماهه]".GetTranslate(), 11, EnPriceLevelType.Buy, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.Buy3, "[سه ماهه]".GetTranslate(), 12, EnPriceLevelType.Buy, isSystem: false, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.LastBuy, "[آخرین خرید]".GetTranslate(), 13, EnPriceLevelType.Buy, isSystem: true, isDef: false),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.LastBuyFromAll, "[آخرین خرید از همه مشتریان]".GetTranslate(), 16, EnPriceLevelType.Buy, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.LastBuyFromTafsil, "[آخرین خرید از مشتری خاص]".GetTranslate(), 17, EnPriceLevelType.Buy, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.BeginPeriod, "[فی ابتدای دوره]".GetTranslate(), 18, EnPriceLevelType.Buy, isSystem: true, isDef: true),
				CreateObject(contextGuid, ParentDefaults.PriceLevel.AverageBuy, "[میانگین خرید]".GetTranslate(), 19, EnPriceLevelType.Buy, isSystem: true, isDef: true)
			}.Where((ProductPriceLevelBusiness p) => p != null).ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ListAllDefault", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtPriceLevelObjs.cs", 45);
			return new List<ProductPriceLevelBusiness>();
		}
	}

	private static ProductPriceLevelBusiness CreateObject(string contextGuid, Guid guid, string name, short ordinal, EnPriceLevelType type, bool isSystem, bool isDef)
	{
		try
		{
			return new ProductPriceLevelBusiness(contextGuid)
			{
				Guid = guid,
				Modified = DateTime.Now,
				Status = true,
				IsDefault = isDef,
				IsSystem = isSystem,
				Name = name,
				Ordinal = ordinal,
				ServerStatus = ServerStatusForClient.None,
				Type = type
			};
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CreateObject", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedAccEntityFramworkPersistence\\Ertegha\\DefualtPriceLevelObjs.cs", 69);
			return null;
		}
	}
}
