using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Interfaces;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Accounting;
using PacketParser.Entities.HesabYar;

namespace EntityCache.Business.Global;

public class SmsParserBusiness
{
	public static async Task<bool> ParseSmsAsync(AndroidCacheFactory androidCacheFactory, SmsParser message, IUiNotificationFactory uiNotificationFactory, CancellationToken token)
	{
		try
		{
			List<SmsParserTemplate> list = (from p in androidCacheFactory.SmsTemplateHelper.GetAll()
				where p.RegexNumbers.Select((string p) => p.FixString().ToLower().Trim()).Contains(message.SmsSender)
				select p).ToList();
			bool result = false;
			foreach (SmsParserTemplate template in list)
			{
				SmsParser parser = new SmsParser(new AndroidSms
				{
					Guid = message.Guid,
					Message = message.SmsMessage,
					ProcessDate = DateTime.Now,
					Sender = message.SmsSender,
					UtcTime = message.Date
				}, template);
				if (!parser.IsMatch || !(parser.Date > DateTime.Now.AddDays(-1.0)))
				{
					continue;
				}
				string contextGuid = await androidCacheFactory.GetContextGuid();
				if (template.IsReception)
				{
					await uiNotificationFactory.DisplayAsync("پردازش پیامک واریز به حساب".GetTranslate(), "پیامک واریز به حساب بانکی در حال پردازش و ذخیره سازی میباشد.".GetTranslate(), ReturnedState.Information, default(CancellationToken));
					ReturnedSaveFuncInfo returnedSaveFuncInfo = await SaveReception(contextGuid, parser);
					if (!returnedSaveFuncInfo.HasError)
					{
						await uiNotificationFactory.DisplayAsync("پردازش پیامک واریز به حساب".GetTranslate(), "پردازش پیامک واریز به حساب بانکی با موفقیت انجام شد.".GetTranslate(), ReturnedState.Information, default(CancellationToken));
					}
					else
					{
						await uiNotificationFactory.DisplayAsync("پردازش پیامک واریز به حساب".GetTranslate(), "خطا در پردازش پیامک دریافت بانکی.".GetTranslate() + "\r\n" + returnedSaveFuncInfo.ErrorMessage, ReturnedState.Information, default(CancellationToken));
					}
				}
				else
				{
					if (template.IsReception)
					{
						throw new NotImplementedException();
					}
					await uiNotificationFactory.DisplayAsync("پردازش پیامک برداشت از حساب".GetTranslate(), "پیامک برداشت از حساب بانکی در حال پردازش و ذخیره سازی می باشد.".GetTranslate(), ReturnedState.Information, default(CancellationToken));
					ReturnedSaveFuncInfo returnedSaveFuncInfo2 = await SavePayment(contextGuid, parser);
					if (!returnedSaveFuncInfo2.HasError)
					{
						await uiNotificationFactory.DisplayAsync("پردازش پیامک برداشت از حساب".GetTranslate(), "پردازش پیامک برداشت از حساب بانکی با موفقیت انجام شد.".GetTranslate(), ReturnedState.Information, default(CancellationToken));
					}
					else
					{
						await uiNotificationFactory.DisplayAsync("پردازش پیامک برداشت از حساب".GetTranslate(), "خطا در پردازش پیامک دریافت بانکی.".GetTranslate() + "\r\n" + returnedSaveFuncInfo2.ErrorMessage, ReturnedState.Information, default(CancellationToken));
					}
				}
				result = true;
				break;
			}
			return result;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ParseSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 177);
			return false;
		}
	}

	private static async Task<ReturnedSaveFuncInfo> SavePayment(string contextGuid, SmsParser parser, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			BankBusiness bankInfo = (await BankBusiness.GetAllAsync(contextGuid, EnSystem.None, "", token, true))?.SingleOrDefault((BankBusiness p) => p.HesabNo == parser.AccountNumber);
			if (bankInfo == null)
			{
				return ret.AddError("حساب بانکی یافت نشد".GetTranslate(), "", "SavePayment", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 190);
			}
			Guid? pardakhtSmsCustomerGuid = currentCashier.PardakhtSmsCustomerGuid;
			if (!pardakhtSmsCustomerGuid.HasValue)
			{
				throw new Exception("cashier default tafsilGuid is null or empty!\r\nsmsMessage does not proccesed!\r\nParser:" + Json.ToStringJson(parser));
			}
			PardakhtBusiness pay = await PardakhtBusiness.GenerateNewWithDefaultsAsync(contextGuid, EnSystem.SmsParser, pardakhtSmsCustomerGuid, "ثبت اتومات با پردازش پیامک دریافتی از موبایل - " + parser.Type);
			pay.Date = parser.Date;
			PardakhtBankBusiness pardakhtBankBusiness = new PardakhtBankBusiness(contextGuid)
			{
				DateM = parser.Date,
				RowDesc = pay.Description,
				Number = parser.Number,
				TafsilGuid = bankInfo.Guid,
				BankRealAccount = parser.Balance
			};
			pardakhtBankBusiness.SetPrice(EnSystem.SmsParser, Math.Abs(parser.Amount));
			pay.AddToDetList(pardakhtBankBusiness, EnSystem.UserInterface, token);
			pay.MetaData.Message = parser.SmsMessage;
			ReturnedSaveFuncInfo result = await pay.SaveAsync(EnSystem.UserInterface, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10);
			if (result.HasError)
			{
				pay.IsTemp = true;
				await pay.SaveAsync(EnSystem.UserInterface, sendToServer: false, Priority.Low, token, awaitOnSemaphore: true, 10);
			}
			ret.AddReturnedValue(result, ReturnedState.None, "SavePayment", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 222);
			await UpdateProcessMessageListAsync(contextGuid, parser);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SavePayment", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 225);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> SaveReception(string contextGuid, SmsParser parser, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			CashierBusiness currentCashier = await CashierBusiness.GetCurrentCashierAsync(contextGuid, token);
			IEnumerable<BankBusiness> enumerable = await BankBusiness.GetAllAsync(contextGuid, EnSystem.None, "", token, true);
			BankBusiness bankInfo = enumerable?.SingleOrDefault((BankBusiness p) => p.HesabNo == parser.AccountNumber);
			if (bankInfo == null && !string.IsNullOrEmpty(parser.BankName))
			{
				bankInfo = enumerable?.SingleOrDefault((BankBusiness p) => p.Name.Contains(parser.BankName));
			}
			if (bankInfo == null)
			{
				return ret.AddError("حساب بانکی یافت نشد", "", "SaveReception", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 239);
			}
			Guid? receptionSmsCustomerGuid = currentCashier.ReceptionSmsCustomerGuid;
			if (!receptionSmsCustomerGuid.HasValue)
			{
				throw new Exception("cashier default tafsilGuid is null or empty!\r\nsmsMessage does not proccesed!\r\nParser:" + Json.ToStringJson(parser));
			}
			ReceptionBusiness obj = await ReceptionBusiness.GenerateNewWithDefaultsAsync(contextGuid, EnSystem.SmsParser, receptionSmsCustomerGuid, "ثبت اتومات با پردازش پیامک دریافتی از موبایل -" + parser.Type);
			obj.Date = parser.Date;
			ReceptionHavaleBusiness receptionHavaleBusiness = new ReceptionHavaleBusiness(contextGuid)
			{
				Date = parser.Date,
				Description = "",
				Number = parser.Number,
				PAN = "",
				RRN = "",
				MerchantId = "",
				MessageId = "",
				STAN = "",
				HavaleType = ENHavaleType.Havale,
				TafsilGuid = bankInfo.Guid,
				BankRealAccount = -parser.Balance
			};
			receptionHavaleBusiness.SetPrice(EnSystem.SmsParser, parser.Amount);
			obj.AddToDetList(receptionHavaleBusiness, EnSystem.None, token);
			obj.MetaData.Message = parser.SmsMessage;
			ReturnedSaveFuncInfo returnedSaveFuncInfo = await obj.SaveAsync(EnSystem.UserInterface, sendToServer: true, Priority.Low, awaitOnSemaphore: true, "", "", 60000, await4send: false, 10, "", token);
			ret.AddReturnedValue(returnedSaveFuncInfo, ReturnedState.None, "SaveReception", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 273);
			returnedSaveFuncInfo.throwExceptionIfError();
			await UpdateProcessMessageListAsync(contextGuid, parser);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SaveReception", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 279);
			ret.AddReturnedValue(ex, "", "SaveReception", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 280);
		}
		return ret;
	}

	private static async Task UpdateProcessMessageListAsync(string contextGuid, SmsParser parser)
	{
		_ = 1;
		try
		{
			List<AndroidSms> list = (await LocalSettingBusiness.GetAsync(contextGuid, "processSmsMessages")).FromJson<List<AndroidSms>>("UpdateProcessMessageListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 288);
			if (list == null)
			{
				list = new List<AndroidSms>();
			}
			list = list.Where((AndroidSms p) => p.ProcessDate > DateTime.Now.AddDays(-3.0)).ToList();
			list.Add(new AndroidSms
			{
				Guid = Guid.NewGuid(),
				Message = parser.SmsMessage,
				Sender = parser.SmsSender
			});
			await LocalSettingBusiness.SaveAsync(contextGuid, "processSmsMessages", Json.ToStringJson(list), awaitOnSemaphore: true, 10);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "UpdateProcessMessageListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Global\\SmsParserBusiness.cs", 299);
		}
	}
}
