using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Accounting.Coding;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Extensions;

public static class Hesab
{
	public static async Task<ReturnedSaveFuncInfo> CheckAccountAsync(this IEnumerable<HesabChanges> lstChange, EnSystem system, string contextGuid, CancellationToken token, string tranName, Document asType, bool isDeleteAction)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (system == EnSystem.Transfer || system == EnSystem.None)
			{
				return res;
			}
			if (!(lstChange?.Any() ?? false))
			{
				return res;
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token, tranName);
			foreach (HesabChanges item in lstChange)
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CheckMoeinAccountAsync(contextGuid, system, item, lstChange, tranName, token), ReturnedState.None, "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 29);
				returnedSaveFuncInfo = res;
				returnedSaveFuncInfo.AddReturnedValue(await CheckTafsilAccountAsync(contextGuid, asType, system, item, lstChange, tranName, token, isDeleteAction, setting), ReturnedState.None, "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 30);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 35);
			res.AddReturnedValue(ex, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 36);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> CheckTafsilAccountAsync(string contextGuid, Document asType, EnSystem system, HesabChanges item, IEnumerable<HesabChanges> lstChange, string tranName, CancellationToken token, bool isDeleteAction, AccGlobalSettings setting)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (asType == Document.Pardakht && isDeleteAction)
			{
				return res;
			}
			if (asType == Document.AmaniKhoroj || asType == Document.AmaniVorod)
			{
				return res;
			}
			if (item == null)
			{
				throw new NullReferenceException("item");
			}
			MoeinBusiness moein = await MoeinBusiness.GetAsync(contextGuid, item.MoeinGuid, tranName, token);
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(contextGuid, system, item.TafsilGuid, tranName, token, "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs");
			if (tafsil == null)
			{
				return res;
			}
			decimal balanceChanges = lstChange.Where((HesabChanges p) => p.TafsilGuid == item.TafsilGuid).Sum((HesabChanges p) => p.Account);
			ENSource source = ClsCache.GetCache(contextGuid).Source;
			if (tafsil.HType == HesabType.Customers)
			{
				decimal account = await AccountBusiness.GetAccountSumAsync(contextGuid, item.MoeinGuid, tafsil.Guid, true, tranName, token);
				decimal acc = lstChange.Where((HesabChanges p) => p.TafsilGuid == item.TafsilGuid).Sum((HesabChanges p) => p.Account);
				acc += account;
				if ((asType == Document.Reception || asType == Document.OrderBackSell || asType == Document.OrderBuy) && !isDeleteAction)
				{
					return res;
				}
				CusBusiness cusBusiness = await CusBusiness.GetAsync(contextGuid, system, item.TafsilGuid, token, tranName);
				decimal num = cusBusiness?.Etebar ?? 0m;
				if (num > 0m && acc < 0m && Math.Abs(acc) > num)
				{
					if (setting.Factors.ForceCreditLimit)
					{
						res.AddError(system, " حداکثر اعتبار تعیین شده نقدی".GetTranslate() + (" " + cusBusiness?.FullName).GetTranslate() + ": " + num.NpToString("N0", 0) + " \r\n " + "میزان اعتبار باقی مانده ".GetTranslate() + "  : " + (num - Math.Abs(account)).NpToString("N0", 0) + " \r\n " + "مبلغ تجاوز کرده از حداکثر اعتبار تعیین شده ".GetTranslate() + ": " + (Math.Abs(acc) - num).NpToString("N0", 0), "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 82);
						res.AddError(system, "لطفا اعتبار حساب را افزایش دهید و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 85);
					}
					else if (system != EnSystem.Transfer)
					{
						res.AddWarning("با ثبت این سند، حد اعتبار حساب".GetTranslate() + " (" + tafsil.Name + " _ " + num.NpToString("N0", 0) + ")" + "به میزان".GetTranslate() + " (" + (Math.Abs(acc) - num).NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " عبور خواهد کرد".GetTranslate(), "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 88);
					}
				}
			}
			else if (tafsil.HType == HesabType.Bank && asType != Document.Reception && source != ENSource.Android_HesabYar && moein.Guid == ParentDefaults.MoeinCoding.CLSMoein4)
			{
				balanceChanges = lstChange.Where((HesabChanges p) => p.TafsilGuid == item.TafsilGuid && p.MoeinGuid == moein.Guid).Sum((HesabChanges p) => p.Account);
				BankBusiness bankBusiness = (await BankBusiness.GetAsync(contextGuid, item.TafsilGuid, system, token, tranName)) ?? throw new NullReferenceException("bank is null!");
				if (bankBusiness.Account + balanceChanges > 0m)
				{
					string message = "با ثبت این سند، موجودی بانک".GetTranslate() + " (" + tafsil.Name + " شعبه".GetTranslate() + ":" + (" " + bankBusiness?.Shobe).GetTranslate() + ")" + " به میزان ".GetTranslate() + "(" + (bankBusiness.Account + balanceChanges).NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " منفی خواهد شد".GetTranslate();
					if (bankBusiness.NegativeAmount == ReturnedState.Error)
					{
						res.AddError(system, message, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 100);
						res.AddError(system, "لطفا قابلیت موجودی منفی را برای بانک فعال نموده و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 101);
					}
					else if (system != EnSystem.Transfer && bankBusiness.NegativeAmount == ReturnedState.Warning)
					{
						res.AddWarning(message, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 103);
					}
				}
			}
			else if (tafsil.HType == HesabType.Sandogh && asType != Document.Reception && source != ENSource.Android_HesabYar && moein.Guid == ParentDefaults.MoeinCoding.CLSMoein7)
			{
				SandoghBusiness sandoghBusiness = (await SandoghBusiness.GetAsync(contextGuid, item.TafsilGuid, system, token, tranName)) ?? throw new NullReferenceException("sandouq is null!");
				if (sandoghBusiness.Account + balanceChanges > 0m)
				{
					string message2 = "با ثبت این سند، موجودی صندوق".GetTranslate() + " (" + tafsil.Name + ") " + "به میزان".GetTranslate() + " (" + (sandoghBusiness.Account + balanceChanges).NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " منفی خواهد شد".GetTranslate();
					if (sandoghBusiness.NegativeAmount == ReturnedState.Error)
					{
						res.AddError(system, message2, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 114);
						res.AddError(system, "لطفا قابلیت موجودی منفی را برای صندوق فعال نموده و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 115);
					}
					else if (system != EnSystem.Transfer && sandoghBusiness.NegativeAmount == ReturnedState.Warning)
					{
						res.AddWarning(message2, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 117);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 123);
			res.AddReturnedValue(ex, "", "CheckTafsilAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 124);
		}
		return res;
	}

	private static async Task<ReturnedSaveFuncInfo> CheckMoeinAccountAsync(string contextGuid, EnSystem system, HesabChanges item, IEnumerable<HesabChanges> lstChange, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if ((item?.MoeinGuid ?? Guid.Empty) == Guid.Empty)
			{
				return ret;
			}
			MoeinBusiness moeinBusiness = await MoeinBusiness.GetAsync(contextGuid, item.MoeinGuid, tranName, token);
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await CheckKolAccountAsync(contextGuid, system, item, moeinBusiness.KolGuid, lstChange, tranName, token), ReturnedState.None, "CheckMoeinAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 136);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckMoeinAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 140);
			ret.AddReturnedValue(ex, "", "CheckMoeinAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 141);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> CheckKolAccountAsync(string contextGuid, EnSystem system, HesabChanges item, Guid kolGuid, IEnumerable<HesabChanges> lstChange, string tranName, CancellationToken token)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (item == null)
			{
				throw new NullReferenceException("item");
			}
			KolBusiness kolBusiness = await KolBusiness.GetAsync(contextGuid, kolGuid, token, tranName);
			if (kolBusiness != null)
			{
				decimal num = item.Account + kolBusiness.Account;
				if (kolBusiness.Mahiat == ENMahiat.JustCredit)
				{
					if (num < 0m)
					{
						res.AddError(system, "حساب".GetTranslate() + "  " + kolBusiness.Name + "به میزان".GetTranslate() + " " + num.NpToString("N0", 0) + " بدهکار خواهد در صورتیکه ماهیت این حساب فقط بستانکار است".GetTranslate(), "", "CheckKolAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 159);
					}
				}
				else if (kolBusiness.Mahiat == ENMahiat.JustDebit && num > 0m)
				{
					res.AddError(system, "حساب ".GetTranslate() + " " + kolBusiness.Name + " " + " به میزان".GetTranslate() + "  " + num.NpToString("N0", 0) + " " + " بستانکار خواهد در صورتیکه ماهیت این حساب فقط بدهکار است".GetTranslate(), "", "CheckKolAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 164);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckKolAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 170);
			res.AddReturnedValue(ex, "", "CheckKolAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 171);
		}
		return res;
	}

	public static async Task<ReturnedSaveFuncInfo> CheckAccountAsync(string contextGuid, CheckAccountConfig config, EnSystem system, CancellationToken token, string tranName)
	{
		ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
		try
		{
			if (config.System == EnSystem.Transfer)
			{
				return res;
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token, tranName);
			MoeinBusiness moein = await MoeinBusiness.GetAsync(contextGuid, config.MoeinGuid, tranName, token);
			if (moein == null)
			{
				return res;
			}
			KolBusiness kolBusiness = await moein.GetKolAsync(token, tranName);
			if (kolBusiness != null)
			{
				decimal num = config.Account + kolBusiness.Account;
				if (kolBusiness.Mahiat == ENMahiat.JustCredit)
				{
					if (num < 0m)
					{
						res.AddError("حساب".GetTranslate() + "  " + kolBusiness.Name + " " + " به میزان".GetTranslate() + "  " + num.NpToString("N0", 0) + " " + " بدهکار خواهد در صورتیکه ماهیت این حساب فقط بستانکار است".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 192);
					}
				}
				else if (kolBusiness.Mahiat == ENMahiat.JustDebit && num > 0m)
				{
					res.AddError("حساب".GetTranslate() + "  " + kolBusiness.Name + " " + " به میزان ".GetTranslate() + num.NpToString("N0", 0) + " " + " بستانکار خواهد در صورتیکه ماهیت این حساب فقط بدهکار است".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 197);
				}
			}
			TafsilBusiness tafsil = await TafsilBusiness.GetAsync(contextGuid, config.System, config.TafsilGuid ?? Guid.Empty, tranName, token, "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs");
			if (tafsil == null)
			{
				return res;
			}
			if (tafsil.HType == HesabType.Bank || tafsil.HType == HesabType.Sandogh || tafsil.HType == HesabType.Customers)
			{
				decimal num2 = await AccountBusiness.GetAccountSumAsync(contextGuid, moein.Guid, tafsil.Guid, true, tranName, token);
				decimal acc = config.Account + num2;
				if (acc > 0m && (tafsil.HType == HesabType.Bank || tafsil.HType == HesabType.Sandogh))
				{
					if (tafsil.HType == HesabType.Bank && moein.Guid == ParentDefaults.MoeinCoding.CLSMoein4)
					{
						BankBusiness bankBusiness = await BankBusiness.GetAsync(contextGuid, config.TafsilGuid ?? Guid.Empty, system, token, tranName);
						if (bankBusiness == null)
						{
							return res;
						}
						string message = "با ثبت این سند، موجودی بانک".GetTranslate() + " (" + tafsil.Name + "  " + "شعبه".GetTranslate() + ": " + bankBusiness?.Shobe + ")  " + "به میزان ".GetTranslate() + "(" + acc.NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " " + " منفی خواهد شد".GetTranslate();
						if (bankBusiness.NegativeAmount == ReturnedState.Error)
						{
							res.AddError(message, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 216);
							res.AddError("لطفا قابلیت موجودی منفی را برای بانک فعال نموده و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 217);
						}
						else if (config.System != EnSystem.Transfer && bankBusiness.NegativeAmount == ReturnedState.Warning)
						{
							res.AddWarning(message, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 219);
						}
					}
					else if (tafsil.HType == HesabType.Sandogh && moein.Guid == ParentDefaults.MoeinCoding.CLSMoein7)
					{
						SandoghBusiness sandoghBusiness = await SandoghBusiness.GetAsync(contextGuid, config.TafsilGuid ?? Guid.Empty, system, token, tranName);
						if (sandoghBusiness == null)
						{
							return res;
						}
						string message2 = "با ثبت این سند، موجودی صندوق".GetTranslate() + " (" + tafsil.Name + ") " + " به میزان".GetTranslate() + " (" + acc.NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " " + " منفی خواهد شد".GetTranslate();
						if (sandoghBusiness.NegativeAmount == ReturnedState.Error)
						{
							res.AddError(message2, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 228);
							res.AddError("لطفا قابلیت موجودی منفی را برای صندوق فعال نموده و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 229);
						}
						else if (config.System != EnSystem.Transfer && sandoghBusiness.NegativeAmount == ReturnedState.Warning)
						{
							res.AddWarning(message2, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 231);
						}
					}
				}
				else if (tafsil.HType == HesabType.Customers)
				{
					if ((config.AsType == Document.Reception || config.AsType == Document.OrderBackSell || config.AsType == Document.OrderBuy) && !config.IsDeleteAction)
					{
						return res;
					}
					decimal num3 = (await CusBusiness.GetAsync(contextGuid, config.System, config.TafsilGuid ?? Guid.Empty, token, tranName))?.Etebar ?? 0m;
					if (num3 > 0m && Math.Abs(acc) > num3)
					{
						if (setting.Factors.ForceCreditLimit)
						{
							res.AddError("با ثبت این سند، حد اعتبار حساب ".GetTranslate() + " (" + tafsil.Name + " _ " + num3.NpToString("N0", 0) + ") " + " به میزان".GetTranslate() + "  (" + (Math.Abs(acc) - num3).NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + "  " + "عبور خواهد کرد".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 243);
							res.AddError("لطفا اعتبار حساب را افزایش دهید و یا مبلغ را اصلاح نمایید".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 244);
						}
						else if (config.System != EnSystem.Transfer)
						{
							res.AddWarning("با ثبت این سند، حد اعتبار حساب".GetTranslate() + "  (" + tafsil.Name + " _ " + num3.NpToString("N0", 0) + ") " + " به میزان".GetTranslate() + " (" + (Math.Abs(acc) - num3).NpToString("N0", 0) + ") " + ClsCache.GetCache(contextGuid).MultiLanguageConfig.CurrencyUnit + " " + " عبور خواهد کرد".GetTranslate(), "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 247);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 254);
			res.AddReturnedValue(ex, "", "CheckAccountAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Extensions\\Hesab.cs", 255);
		}
		return res;
	}
}
