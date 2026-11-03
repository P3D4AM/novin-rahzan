using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace EntityCache.DescriptionParser;

public static class OrderDescParser
{
	public static string Id2 => "[شماره]";

	public static string DateSh => "[تاریخ فاکتور]";

	public static string FactorType => "[نوع فاکتور]";

	public static string Description => "[شرح فاکتور]";

	public static string PrdCode => "[کد کالا]";

	public static string PrdName => "[نام کالا]";

	public static string ServiceCode => "[کد خدمت]";

	public static string ServiceName => "[نام خدمت]";

	public static string DetailDescription => "[شرح سطر]";

	public static async Task<string> GetOrderDescAsync(this OrderBusiness p, AccGlobalSettings setting, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			switch (p.FactorType)
			{
			case Document.OrderSell:
				desc = setting.AutoDescription.SellDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBuy:
				desc = setting.AutoDescription.BuyDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackSell:
				desc = setting.AutoDescription.BackSellDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "فاکتور برگشت از فروش شماره : [شماره] [شرح فاکتور]";
				}
				break;
			case Document.OrderBackBuy:
				desc = setting.AutoDescription.BackBuyDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "فاکتور برگشت از خرید شماره : [شماره] [شرح فاکتور]";
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 62);
		}
		return desc;
	}

	public static async Task<string> GetOrderDetailDescAsync(this OrderDetailBusiness p, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.MasterOrder.FactorType)
			{
			case Document.OrderSell:
				if (p.IsKala)
				{
					desc = accGlobalSettings.AutoDescription.SellDetailProductDescription;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "فروش شماره : ([شماره]) ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر";
					}
				}
				else
				{
					desc = accGlobalSettings.AutoDescription.SellDetailServiceDescription;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "فروش شماره : ([شماره]) ([نام سرویس] - [کد سرویس]) [شرح فاکتور] [شرح سطر]";
					}
				}
				break;
			case Document.OrderBuy:
				desc = accGlobalSettings.AutoDescription.BuyDetailDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "خرید شماره : ([شماره]) ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
				}
				break;
			case Document.OrderBackSell:
				desc = accGlobalSettings.AutoDescription.BackSellDetailDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "برگشت از فروش شماره : [شماره] ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
				}
				break;
			case Document.OrderBackBuy:
				desc = accGlobalSettings.AutoDescription.BackBuyDetailDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "برگشت از خرید شماره : [شماره] ([نام کالا] - [کد کالا]) [شرح فاکتور] [شرح سطر])";
				}
				break;
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.MasterOrder.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.MasterOrder.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.MasterOrder.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.MasterOrder.Description);
			ProductCompactBusiness productCompactBusiness = await p.GetPrdInfoAsync(system, token, tranName);
			if (p.IsKala)
			{
				desc = desc.Replace(PrdCode, productCompactBusiness.IdProduct);
				desc = desc.Replace(PrdName, productCompactBusiness.Name);
			}
			else
			{
				desc = desc.Replace(ServiceCode, productCompactBusiness.IdProduct);
				desc = desc.Replace(ServiceName, productCompactBusiness.Name);
			}
			desc = desc.Replace(DetailDescription, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderDetailDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 122);
		}
		return desc;
	}

	public static async Task<string> GetOrderDiscountDescAsync(this OrderBusiness p, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.FactorType)
			{
			case Document.OrderSell:
				desc = accGlobalSettings.AutoDescription.SellDiscountDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "تخفیف فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBuy:
				desc = accGlobalSettings.AutoDescription.BuyDiscountDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "تخفیف فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackSell:
				desc = accGlobalSettings.AutoDescription.BackSellDiscountDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "تخفیف فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackBuy:
				desc = accGlobalSettings.AutoDescription.BackBuyDiscountDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "تخفیف فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderDiscountDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 163);
		}
		return desc;
	}

	public static async Task<string> GetOrderFareDescAsync(this OrderBusiness p, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.FactorType)
			{
			case Document.OrderSell:
				desc = accGlobalSettings.AutoDescription.SellFareDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "کرایه حمل فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBuy:
				desc = accGlobalSettings.AutoDescription.BuyFareDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "کرایه حمل فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackSell:
				desc = accGlobalSettings.AutoDescription.BackSellFareDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "کرایه حمل فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackBuy:
				desc = accGlobalSettings.AutoDescription.BackBuyFareDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "کرایه حمل فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderFareDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 204);
		}
		return desc;
	}

	public static async Task<string> GetOrderTaxDescAsync(this OrderBusiness p, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.FactorType)
			{
			case Document.OrderSell:
				desc = accGlobalSettings.AutoDescription.SellTaxDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "ارزش افزوده فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBuy:
				desc = accGlobalSettings.AutoDescription.BuyTaxDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "ارزش افزوده فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackSell:
				desc = accGlobalSettings.AutoDescription.BackSellTaxDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "ارزش افزوده فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackBuy:
				desc = accGlobalSettings.AutoDescription.BackBuyTaxDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "ارزش افزوده فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderTaxDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 245);
		}
		return desc;
	}

	public static async Task<string> GetOrderAvarezDescAsync(this OrderBusiness p, EnSystem system, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.FactorType)
			{
			case Document.OrderSell:
				desc = accGlobalSettings.AutoDescription.SellAvarezDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "عوارض قانونی فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBuy:
				desc = accGlobalSettings.AutoDescription.BuyAvarezDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "عوارض قانونی فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackSell:
				desc = accGlobalSettings.AutoDescription.BackSellAvarezDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "عوارض قانونی فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			case Document.OrderBackBuy:
				desc = accGlobalSettings.AutoDescription.BackBuyAvarezDescription;
				if (string.IsNullOrEmpty(desc))
				{
					desc = "عوارض قانونی فاکتور برگشت از خرید شماره : ([شماره]) [شرح فاکتور]";
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderAvarezDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 286);
		}
		return desc;
	}

	public static async Task<string> GetOrderVisitorDescAsync(this OrderBusiness p, EnSystem system, short level, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			switch (p.FactorType)
			{
			case Document.OrderSell:
				switch (level)
				{
				case 1:
					desc = accGlobalSettings.AutoDescription.SellVisitor1Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 2:
					desc = accGlobalSettings.AutoDescription.SellVisitor2Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 2 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 3:
					desc = accGlobalSettings.AutoDescription.SellVisitor3Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 3 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 4:
					desc = accGlobalSettings.AutoDescription.SellVisitor4Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 4 فاکتور فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				}
				break;
			case Document.OrderBackSell:
				switch (level)
				{
				case 1:
					desc = accGlobalSettings.AutoDescription.BackSellVisitor1Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 2:
					desc = accGlobalSettings.AutoDescription.BackSellVisitor2Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 2 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 3:
					desc = accGlobalSettings.AutoDescription.BackSellVisitor3Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 3 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 4:
					desc = accGlobalSettings.AutoDescription.BackSellVisitor4Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه ویزیتوری سطح 4 فاکتور برگشت از فروش شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				}
				break;
			default:
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
				break;
			}
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderVisitorDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 357);
		}
		return desc;
	}

	public static async Task<string> GetOrderHazineDescAsync(this OrderBusiness p, EnSystem system, short level, string contextGuid, string tranName, CancellationToken token = default(CancellationToken))
	{
		string desc = "";
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid);
			if (p.FactorType == Document.OrderBuy)
			{
				switch (level)
				{
				case 1:
					desc = accGlobalSettings.AutoDescription.BuyHazine1Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 2:
					desc = accGlobalSettings.AutoDescription.BuyHazine2Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 3:
					desc = accGlobalSettings.AutoDescription.BuyHazine3Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 4:
					desc = accGlobalSettings.AutoDescription.BuyHazine4Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				case 5:
					desc = accGlobalSettings.AutoDescription.BuyHazine5Description;
					if (string.IsNullOrEmpty(desc))
					{
						desc = "هزینه فاکتور خرید شماره : ([شماره]) [شرح فاکتور]";
					}
					break;
				}
			}
			else
			{
				object display = p.FactorType.GetDisplay();
				desc = $"{display}({await p.GetId2Async(system, tranName, token)}) {p.Description}";
			}
			string text = desc;
			string id = Id2;
			desc = text.Replace(id, (await p.GetId2Async(system, tranName, token)).ToString());
			desc = desc.Replace(DateSh, p.GetDate().MiladiToCalture());
			desc = desc.Replace(FactorType, p.FactorType.GetDisplay());
			desc = desc.Replace(Description, p.Description);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOrderHazineDescAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\DescriptionParser\\OrderDescParser.cs", 408);
		}
		return desc;
	}
}
