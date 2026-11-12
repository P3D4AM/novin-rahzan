using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Department;
using PacketParser;
using PacketParser.Activation;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Department;
using PacketParser.Entities.Interfaces.Department;
using SharedActivation.LKLogics;
using WebSmsStandard;

namespace SharedActivation;

public class CLSSMS
{
	private const string SMSSenderLine = "30006403868611";

	private const string SMSSenderLine2 = "30006154";

	private static List<string> Mobiles => new List<string>
	{
		"9158980915", "9158941510", "9365740443", "9377373548", "9155249861", "9370385003", "9159222416", "9156590839", "9029624883", "09158980915",
		"09158941510", "09365740443", "09377373548", "09155249861", "09370385003", "09159222416", "09156590839", "09029624883", "989158980915", "989158941510",
		"989365740443", "989377373548", "989155249861", "989370385003", "989159222416", "989156590839", "989029624883"
	};

	public static Task<ReturnedSaveFuncInfo> SendSmsAsync(ILkInfo lkInfo, string SenderNumber, string panel, ICusDal cusDal)
	{
		ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
		int num = 0;
		try
		{
			panel = panel?.FixString()?.Trim();
			SenderNumber = SenderNumber?.FixString()?.Trim();
			num = 1;
			if (string.IsNullOrEmpty(SenderNumber))
			{
				return Task.FromResult(returnedSaveFuncInfo.AddError(EnSystem.None, "sender number is null or empty!", "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 34));
			}
			if (string.IsNullOrEmpty(panel))
			{
				return Task.FromResult(returnedSaveFuncInfo.AddError(EnSystem.None, "panel is null or rmpty!", "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 36));
			}
			num = 3;
			SenderNumber = SenderNumber.RemoveNoNumbers();
			num = 5;
			panel = panel.RemoveNoNumbers();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{num}/SenderNumber:{SenderNumber}/lkInfo:{Json.ToStringJson(lkInfo)}/panel:{panel}", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 44);
			returnedSaveFuncInfo.AddReturnedValue(ex, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 45);
		}
		return Task.FromResult(returnedSaveFuncInfo);
	}

	public static async Task<ReturnedSaveFuncInfo> SendActivationCodeAsync(ILkInfo lkInf, string SoftSerial, string Mobile, ICusDal cusDal)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			if (Mobiles.Contains(Mobile))
			{
				if (lkInf != null && lkInf.ExpireDate > DateTime.Now)
				{
					tasks.Add(SendPrivateMessageAsync(lkInf, SoftSerial, Mobile, cusDal));
				}
				else
				{
					tasks.Add(SendPrivateMessageAsync(lkInf, SoftSerial, Mobile, cusDal));
				}
			}
			tasks.Add(SendActivationCode2EndUserAsync(lkInf, SoftSerial, Mobile, cusDal));
			ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 73);
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			WebErrorLog.Instance.StartLog(ex, "", "SendActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 77);
			ret.AddReturnedValue(ex, "", "SendActivationCodeAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 78);
		}
		return ret;
	}

	public static async Task<ReturnedSaveFuncInfo> SendSmsAsync(string mob, string message, CancellationToken token)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			Task<long[]> sender1 = SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", message, mob, "30006403868611", token);
			SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", message, mob, "30006154", token);
			await Task.WhenAll<long[]>(sender1, sender1);
			if (await sender1 == null)
			{
				return ret.AddError(EnSystem.None, "sendSms result refrencedId is null!!", "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 97);
			}
			if ((await sender1).Any((long p) => p <= 0))
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddError(EnSystem.None, "message has been sent by error\r\nrefremnceIds:" + string.Join(",", await sender1), "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 99);
			}
			else
			{
				ReturnedSaveFuncInfo returnedSaveFuncInfo2 = ret;
				returnedSaveFuncInfo2.AddInformation("message has been sent successfully!\r\nrefrencedIds:" + string.Join(",", await sender1), "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 102);
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			WebErrorLog.Instance.StartLog(ex, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 107);
			ret.AddReturnedValue(ex, "", "SendSmsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 108);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> SendActivationCode2EndUserAsync(ILkInfo lkInfo, string soft, string mob, ICusDal cusDal, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			List<string> msg = ActivationCode(lkInfo, soft, cusDal);
			List<Task> taskList = new List<Task>();
			if (!(msg?.Any() ?? false))
			{
				return ret.AddError(EnSystem.None, "message is null or empty!", "", "SendActivationCode2EndUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 120);
			}
			foreach (string item in msg)
			{
				if (!string.IsNullOrEmpty(item?.Trim()))
				{
					taskList.Add(SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", item, mob, "30006403868611", token));
					ActivationTBL log2 = new ActivationTBL
					{
						LKSerial = lkInfo.LKSerial,
						SoftSerial = soft,
						OutputMsg = item,
						Number = mob,
						ExpiredDateM = (lkInfo?.ExpireDate ?? DateTime.Now),
						RefrencedID = 0L,
						ST = 0
					};
					if (log2.LKSerial > 0)
					{
						await ClsActivationTBL.SaveAsync("SendActivationCode2EndUser", log2);
					}
				}
			}
			if (!(msg?.Any() ?? false))
			{
				return ret.AddError(EnSystem.None, "msg list is null or empty!", "", "SendActivationCode2EndUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 140);
			}
			foreach (string item2 in msg)
			{
				if (!string.IsNullOrEmpty(item2?.Trim()) && item2 != null)
				{
					taskList.Add(SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", item2, mob, "30006154", token));
					ActivationTBL log = new ActivationTBL
					{
						LKSerial = lkInfo.LKSerial,
						SoftSerial = soft,
						OutputMsg = item2,
						Number = mob,
						ExpiredDateM = (lkInfo?.ExpireDate ?? DateTime.Now),
						RefrencedID = 0L,
						ST = 0
					};
					if (log.LKSerial > 0)
					{
						await ClsActivationTBL.SaveAsync("ClsSms.SendActivationCode2EndUser", log);
					}
					await Task.WhenAll(taskList);
				}
			}
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			WebErrorLog.Instance.StartLog(ex, "", "SendActivationCode2EndUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 164);
			ret.AddReturnedValue(ex, "", "SendActivationCode2EndUserAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 165);
		}
		return ret;
	}

	private static async Task<ReturnedSaveFuncInfo> SendPrivateMessageAsync(ILkInfo lk, string soft, string mob, ICusDal cusDal, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			string msg = FullActivationCode(lk, soft, cusDal);
			Task<long[]> t1 = SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", msg, mob, "30006403868611", token);
			Task<long[]> t2 = SmsSender.SendSmsAsync("Novin", "N@vin7590@", "npSMS", msg, mob, "30006154", token);
			await Task.WhenAll<long[]>(t1, t2);
		}
		catch (Exception ex2)
		{
			Exception ex = ex2;
			WebErrorLog.Instance.StartLog(ex, "", "SendPrivateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 183);
			ret.AddReturnedValue(ex, "", "SendPrivateMessageAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedActivation\\Bussiness\\CLSSMS.cs", 184);
		}
		return ret;
	}

	private static string AdminActivationCode_Information(ILkInfo inf, string soft, ICusDal cusDal)
	{
		string text = "";
		if (inf != null && inf.TypeAccounting == ENCode.ForoshgahiSade)
		{
			text += "ساده\r\n";
		}
		else if (inf != null && inf.TypeAccounting == ENCode.ForoshgahiMotavaset)
		{
			text += "متوسط\r\n";
		}
		else if (inf != null && inf.TypeAccounting == ENCode.ForoshgahiPishrafteh)
		{
			text += "پیشرفته\r\n";
		}
		else if (inf != null && inf.TypeAccounting == ENCode.Jame)
		{
			text += "جامع\r\n";
		}
		if (inf != null && inf.Tolid)
		{
			text += "تولید\r\n";
		}
		if (inf != null && inf.Scale)
		{
			text += "ترازو\r\n";
		}
		if (inf != null && inf.BarCode)
		{
			text += "بارکد\r\n";
		}
		if (inf != null && inf.Hozor_Hoghogh == TypeHozor.Level3)
		{
			text += "حضورغیاب+حقوق دستمزد\r\n";
		}
		else if (inf != null && inf.Hozor_Hoghogh == TypeHozor.Level2)
		{
			text += "حقوق دستمزد\r\n";
		}
		else if (inf != null && inf.Hozor_Hoghogh == TypeHozor.Level1)
		{
			text += "حضورغیاب\r\n";
		}
		if (!string.IsNullOrEmpty(inf?.Description))
		{
			text = text + inf.Description + "\r\n";
		}
		return text;
	}

	private static List<string> ActivationCode(ILkInfo lkInfo, string SoftSerialNumber, ICusDal cusDal)
	{
		List<string> list = new List<string>();
		if ((lkInfo?.LKSerial ?? 0) <= 0)
		{
			return list;
		}
		ReturnedSaveFuncInfo returnedSaveFuncInfo = LkInfoBusiness.Check(SoftSerialNumber, lkInfo);
		if (returnedSaveFuncInfo.HasError)
		{
			list.Add(returnedSaveFuncInfo.ErrorMessage);
			return list;
		}
		StringBuilder stringBuilder = new StringBuilder(100000);
		if (lkInfo.TypeAccounting == ENCode.ForoshgahiSade)
		{
			stringBuilder.AppendLine("ساده:" + ClsActivation.Accounting_Sade(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.TypeAccounting == ENCode.ForoshgahiMotavaset)
		{
			stringBuilder.AppendLine("متوسط:" + ClsActivation.Accounting_Motavaset(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.TypeAccounting == ENCode.ForoshgahiPishrafteh)
		{
			stringBuilder.AppendLine("پیشرفته:" + ClsActivation.Accounting_Pishrafteh(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.TypeAccounting == ENCode.Jame)
		{
			stringBuilder.AppendLine("جامع:" + ClsActivation.Accounting_Jame(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.TypeAccounting != 0)
		{
		}
		if (lkInfo.Tolid)
		{
			stringBuilder.AppendLine("تولید:" + ClsActivation.Tolid(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		if (lkInfo.BarCode)
		{
			stringBuilder.AppendLine("بارکد:" + ClsActivation.BarCode(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.AccountingPackage, lkInfo.MaxActivationDays, lkInfo.Net_Accounting, lkInfo.ExpireDate) + "\r\n");
		}
		if (lkInfo.Scale)
		{
			stringBuilder.AppendLine("ترازو:" + ClsActivation.Scale(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.ScaleCount, lkInfo.MaxActivationDays, lkInfo.Net_Scales, lkInfo.ExpireDate) + "\r\n");
		}
		if (lkInfo.Hozor_Hoghogh == TypeHozor.Level1)
		{
			stringBuilder.AppendLine("حقوق1:" + ClsActivation.Level1(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.HozoorType, lkInfo.MaxActivationDays, lkInfo.Net_Hozor, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.Hozor_Hoghogh == TypeHozor.Level2)
		{
			stringBuilder.AppendLine("حقوق2:" + ClsActivation.Level2(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.HozoorType, lkInfo.MaxActivationDays, lkInfo.Net_Hozor, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.Hozor_Hoghogh == TypeHozor.Level3)
		{
			stringBuilder.AppendLine("حقوق3:" + ClsActivation.Level3(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.HozoorType, lkInfo.MaxActivationDays, lkInfo.Net_Hozor, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.Hozor_Hoghogh == TypeHozor.Level4)
		{
			stringBuilder.AppendLine("حقوق4:" + ClsActivation.Level4(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.HozoorType, lkInfo.MaxActivationDays, lkInfo.Net_Hozor, lkInfo.ExpireDate) + "\r\n");
		}
		else if (lkInfo.Hozor_Hoghogh == TypeHozor.Level5)
		{
			stringBuilder.AppendLine("حقوق5:" + ClsActivation.Level5(SoftSerialNumber, lkInfo.LKSerial.ToString(), lkInfo.HozoorType, lkInfo.MaxActivationDays, lkInfo.Net_Hozor, lkInfo.ExpireDate) + "\r\n");
		}
		if (lkInfo.LKSerial == 1234567890)
		{
			stringBuilder.AppendLine("تاریخ انقضاء پشتیبانی شما:" + DateTime.Now.AddDays(10.0).MiladiToShamsi());
		}
		else
		{
			stringBuilder.AppendLine("تاریخ انقضاء پشتیبانی شما:" + lkInfo.ExpireDateSh);
		}
		list.Add(stringBuilder.ToString());
		string text = "";
		if (DateTime.Now > lkInfo.ExpireDate)
		{
			text = "مشترک گرامی مدت زمان قرارداد پشتیبانی شما به پایان رسیده است.\r\nخواهشمند است جهت تمدید قرارداد پشتیبانی هرچه سریع تر اقدام نمایید\r\nبا تشکر نوین پرداز\r\n05137597590\r\nwww.NovinP.com";
		}
		else if (lkInfo.ExpireDate.Subtract(DateTime.Now).Days < 60)
		{
			text = "مشترک گرامی پشتیبانی نرم افزاری شما به زودی به اتمام میرسد\r\nخواهشمند است جهت جلوگیری از قطع خدمات نسبت به تمدید قرارداد هرچه سریع تر اقدام نمایید.\r\nبا تشکر نوین پرداز\r\n05137597590\r\nwww.NovinP.com";
		}
		if (text.Trim() != "")
		{
			list.Add(text);
			string item = "لطفا جهت تمدید به آدرس ذیل مراجعه نمایید:\r\nhttps://crm.novinp.ir/extend/code/" + lkInfo.LKSerial + SoftSerialNumber + "\r\nتاریخ انقضاء پشتیبانی شما:" + lkInfo.ExpireDateSh;
			list.Add(item);
		}
		return list;
	}

	private static string FullActivationCode(ILkInfo lkInfo, string SoftSerialNumber, ICusDal cusDal)
	{
		StringBuilder stringBuilder = new StringBuilder(100000);
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 1;
		int num5 = 1;
		if ((lkInfo?.LKSerial ?? (-1)) > 0)
		{
			if (lkInfo.Net_Accounting > 0)
			{
				num = lkInfo.Net_Accounting;
			}
			if (lkInfo.Net_Hozor > 0)
			{
				num2 = lkInfo.Net_Hozor;
			}
			if (lkInfo.Net_Scales > 0)
			{
				num3 = lkInfo.Net_Scales;
			}
			if (lkInfo.ScaleCount > 0)
			{
				num4 = lkInfo.ScaleCount;
			}
		}
		stringBuilder.AppendLine("پیشرفته:" + ClsActivation.Accounting_Pishrafteh(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("پیشرفته:" + ClsActivation.Accounting_Pishrafteh(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("متوسط:" + ClsActivation.Accounting_Motavaset(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("ساده:" + ClsActivation.Accounting_Sade(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("بارکد:" + ClsActivation.BarCode(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("تولید:" + ClsActivation.Tolid(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("ترازو:" + ClsActivation.Scale(SoftSerialNumber, lkInfo.LKSerial.ToString(), (short)num4, (short)num5, (short)num3, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("حقوق1:" + ClsActivation.Level1(SoftSerialNumber, lkInfo.LKSerial.ToString(), HozoorPackage.General, (short)num5, (short)num2, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("حقوق2:" + ClsActivation.Level2(SoftSerialNumber, lkInfo.LKSerial.ToString(), HozoorPackage.General, (short)num5, (short)num2, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("حقوق3:" + ClsActivation.Level3(SoftSerialNumber, lkInfo.LKSerial.ToString(), HozoorPackage.General, (short)num5, (short)num2, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("حقوق4:" + ClsActivation.Level4(SoftSerialNumber, lkInfo.LKSerial.ToString(), HozoorPackage.General, (short)num5, (short)num2, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("حقوق5:" + ClsActivation.Level5(SoftSerialNumber, lkInfo.LKSerial.ToString(), HozoorPackage.General, (short)num5, (short)num2, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("تمدید:" + ClsActivation.Tamdid(SoftSerialNumber, lkInfo.LKSerial.ToString(), AccountingPackage.General, (short)num5, (short)num, DateTime.Now.AddDays(num5)) + "\r\n");
		stringBuilder.AppendLine("LK:" + lkInfo.LKSerial + "\r\n");
		stringBuilder.AppendLine("Soft:" + SoftSerialNumber + "\r\n");
		return stringBuilder.ToString();
	}
}
