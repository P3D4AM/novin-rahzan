using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ClientHub.AccountingHubs;
using EntityCache.Business;
using EntityCache.Business.Attendance;
using PacketParser;
using PacketParser.Attendance;
using PacketParser.Attendance.Entities;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Models.Attendance;
using PacketParser.Entities;
using PacketParser.Entities.Attendance;
using PacketParser.Payamak.Entities;
using Serilog;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.Device;
using TimeAttendanceDAL.Dal.FLRGharardad;
using TimeAttendanceDAL.Dal.FLRPerson;
using TimeAttendanceDAL.Dal.FLRShift;
using TimeAttendanceDAL.Dal.Sms;
using TimeAttendanceDAL.Dal.Taradod;
using TimeAttendanceDAL.Settings;
using TimeAttendanceDAL.View.Taradod;

namespace TimeAttendanceDAL.Dal.Other;

[Serializable]
[DoNotPruneType]
public class ClsSms
{
	private static CancellationTokenSource _sendSmsToken;

	private static TaskCompletionSource<bool> _forceToSendSms = new TaskCompletionSource<bool>();

	public static void ForceToSendSms()
	{
		_forceToSendSms.TrySetResult(result: true);
		_forceToSendSms = new TaskCompletionSource<bool>();
	}

	private static string UpdateTemplate(string template, string parameter, string replacement)
	{
		string oldValue = "[" + Utilities.GetTranslate(parameter) + "]";
		template = template.Replace(oldValue, replacement);
		return template;
	}

	public static async Task StartSendSmsAsync(string contextGuid)
	{
		try
		{
			_sendSmsToken?.Cancel();
			_sendSmsToken = new CancellationTokenSource();
			CancellationToken token = _sendSmsToken.Token;
			List<DeviceInfo> devices = await DeviceInfo.GetAllAsync(contextGuid);
			await Task.Delay(60000, token);
			Task.Run(() => SendSmsAbsentAsync(contextGuid, token));
			Task.Run(() => SendSmsIncompleteAsync(contextGuid, token));
			Task.Run(() => SendSmsTaradodAsync(contextGuid, devices, token));
			Task.Run(() => SendSmsVacationAsync(contextGuid, token));
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			Log.Error(ex2, "");
		}
	}

	private static async Task<List<int>> SendSmsAsync(string contextGuid, TimeAttendanceSettings setting, string template, string mobs, DateTime dateM, string trafficName, Person personInfo, int ekInt, int kkInt, CancellationToken token = default(CancellationToken))
	{
		_ = 1;
		try
		{
			PayamakClientHubInstances payamakService = PayamakClientHubInstances.GetInstance(contextGuid);
			List<int> ret = new List<int>();
			if (string.IsNullOrEmpty(mobs))
			{
				return ret;
			}
			template = template.Replace("<", "[");
			template = template.Replace(">", "]");
			string newValue = LanguageAndCalendar.Instance.CaltureCalender.MiladiToCalendar(dateM);
			string newValue2 = Calendar.DateToFormatTime(dateM);
			string newValue3 = Time.IntToTime(kkInt);
			string newValue4 = Time.IntToTime(ekInt);
			template = template.Replace("[تاریخ]", newValue);
			template = template.Replace("[زمان]", newValue2);
			template = template.Replace("[نام شخص]", ((PersonBase)personInfo).Name);
			template = template.Replace("[فامیل]", ((PersonBase)personInfo).Family);
			template = template.Replace("[کد معین]", ((PersonBase)personInfo).CodeMoein.ToString());
			template = template.Replace("[کد تفصیل]", ((PersonBase)personInfo).CodeTafsil.ToString());
			template = template.Replace("[نوع تردد]", trafficName);
			template = template.Replace("[اضافه کار]", newValue4);
			template = template.Replace("[کسرکار]", newValue3);
			ReturnedSaveFuncInfoWithValue<BaseSmsNetAccount> smsAccount = await payamakService.SmsNetAccount.GetByIdAsync(((AttDepartmentAdminSms)setting.Payamak).IdSamane);
			if (((ReturnedSaveFuncInfo)smsAccount).HasError)
			{
				Log.Error($"{DateTime.Now}: smsAccount has error in method SendSmsAsync \r\n IdSamane = {((AttDepartmentAdminSms)setting.Payamak).IdSamane}");
				return null;
			}
			((ReturnedSaveFuncInfo)smsAccount).throwExceptionIfError();
			List<long> list = (from number in Text.SplitString(mobs, " /\\\r\n\t)(-_=+%$#@*|><,`~'\".?؟،;:").Distinct().Select((Func<string, WebTafsilNumbers>)((string mob) => new WebTafsilNumbers(mob)))
				where number.IsValid && number.Mobile
				select number into p
				select p.CalcedNumber).ToList();
			if (list == null || !list.Any())
			{
				return ret;
			}
			foreach (long mob2 in list)
			{
				BaseSmsOutBox val = new BaseSmsOutBox
				{
					Message = template,
					Number = $"0{mob2}",
					SenderNumber = smsAccount.value.SenderNumber,
					AccountId = ((AttDepartmentAdminSms)setting.Payamak).IdSamane
				};
				ReturnedSaveFuncInfoWithValue<List<int>> val2 = await payamakService.SmsOutBox.SaveAsync(val, default(CancellationToken));
				((ReturnedSaveFuncInfo)val2).throwExceptionIfError();
				_ = $"[ارسال پیام به]: {mob2}\r\n template = {template}";
				ret.AddRange(val2.value);
			}
			return ret;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private static async Task SendSmsTaradodAsync(string contextGuid, List<DeviceInfo> devices, CancellationToken token)
	{
		bool dataExists = false;
		DateTime lastSendLog = DateTime.Now.AddMinutes(-30.0);
		Task sendTask = null;
		StringBuilder sb = null;
		while (!token.IsCancellationRequested)
		{
			try
			{
				await Task.Delay(30000);
				sb = new StringBuilder().AppendLine($"{DateTime.Now} start send sms.");
				sendTask = _forceToSendSms.Task;
				if (sendTask == null)
				{
					sb.AppendLine("داخل متود ارسال پیامک پراپرتی ");
					sb.AppendLine("sendTask");
					sb.AppendLine("نال بود. یک دقیقه انتظار و مجدد ادامه میدم");
					Log.Error(sb.ToString());
					await Task.Delay(60000, token);
					continue;
				}
				if (lastSendLog.AddMinutes(5.0) < DateTime.Now)
				{
					lastSendLog = DateTime.Now;
					Log.Information($"SendSmsTaradodAsync is running at {DateTime.Now}");
				}
				TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
				if (!CanSendSmsNow(setting, (StatuseTaradodEnumType)0))
				{
					sb.AppendLine("با توجه به تنظیمات نرم افزار ، اجازه ارسال پیامک نداشتم");
					sb.AppendLine("یک دقیقه انتظار و بعد مجدد سعی می کنم ");
					Log.Error(sb.ToString());
					await Task.Delay(60000);
					continue;
				}
				List<LogOnPersonel> list = await LogOnPersonel.GetAllForSendSmsTaradodAsync(contextGuid, DateTime.Now.Date.AddDays(-1.0), 100, token);
				if (list == null || !list.Any())
				{
					sb.AppendLine("پیامکی برای ارسال نداشتم");
					sb.AppendLine("یک دقیقه انتظار و بعد مجدد چک می کنم");
					Log.Error(sb.ToString());
					dataExists = false;
					await Task.Delay(60000);
					continue;
				}
				dataExists = true;
				sb.AppendLine($"count taradod:{list.Count}");
				foreach (LogOnPersonel taradod in list)
				{
					bool flag = await SendSmsTaradodAsync_(contextGuid, setting, taradod, devices, token);
					sb.AppendLine($"for person({taradod.PersonNo}) {taradod.DateTimeLogM} result:{flag}");
				}
			}
			catch (OperationCanceledException)
			{
				sb.AppendLine("توکن داخل متود ارسال پیامک تردد کنسل شد");
				Log.Warning(sb.ToString());
			}
			catch (Exception ex2)
			{
				sb.AppendLine("داخل متود اسال پیامک به اکسپشن خوردم . یک دقیقه انتظار و مجدد ادامه میدم");
				Log.Error(sb.ToString());
				Log.Error(ex2, "");
				await Task.Delay(60000);
			}
			finally
			{
				if (!dataExists && sendTask != null)
				{
					await Task.WhenAny(new Task[2]
					{
						Task.Delay(30000),
						sendTask
					});
				}
				sb.AppendLine("finally in sendSmsTaradod");
				Log.Warning(sb.ToString());
			}
		}
	}

	private static async Task<bool> SendSmsTaradodAsync_(string contextGuid, TimeAttendanceSettings setting, LogOnPersonel pLogOnPerson, List<DeviceInfo> devices, CancellationToken token = default(CancellationToken))
	{
		_ = 12;
		try
		{
			PayamakClientHubInstances payamakService = PayamakClientHubInstances.GetInstance(contextGuid);
			if (!string.IsNullOrEmpty(pLogOnPerson.SmsRefrenceIDs))
			{
				throw new NotImplementedException("مگه میشه ! مگه داریم!؟");
			}
			if (DateTime.Now > pLogOnPerson.DateTimeLogM.AddMinutes((int)((AttDepartmentAdminSms)setting.Payamak).SendTaradodSmsMaxMinutes))
			{
				await pLogOnPerson.SaveSmsRefrencedIdsAsync("-1", token, 20);
				return false;
			}
			if (pLogOnPerson.PersonId <= 0)
			{
				return false;
			}
			Person personInfo = await Person.GetByPersonIdAsync(contextGuid, pLogOnPerson.PersonId, token);
			if (personInfo == null)
			{
				return false;
			}
			string mobPerson = ((PersonBase)personInfo).Mob1 + "," + ((PersonBase)personInfo).Mob2;
			string mobManagments = ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs;
			if (string.IsNullOrEmpty(mobManagments.Trim()) && string.IsNullOrEmpty(mobPerson.Trim()))
			{
				return false;
			}
			List<int> SmsRefrenceIds = new List<int>();
			int ek = 0;
			int kk = 0;
			string statusname = "تردد شخص";
			_ = new CoupleTaradod[8];
			GharardadNew gharardadNew = await GharardadNew.GetAsync(contextGuid, ((PersonBase)personInfo).Id, pLogOnPerson.DateTimeLogM);
			CoupleTaradod[] coupleTaradodCurDate;
			if (gharardadNew != null)
			{
				TaradodFullDetView taradodDet = await TaradodFullDet.GetAsync_(contextGuid, devices, pLogOnPerson.DateTimeLogM.Date, personInfo, gharardadNew, checkValidationVorodAndKHoroj: false);
				ek = await taradodDet.GetEzafeKarAsync(token);
				kk = await taradodDet.GetKasrKarAsync(token);
				statusname = taradodDet.TaradodStatusName;
				coupleTaradodCurDate = taradodDet.CoupleTaradodCurrentDay;
			}
			else
			{
				coupleTaradodCurDate = TaradodFullDet.CreateCoupleTaradods(contextGuid, await LogOnPersonel.GetAllByPersonIdAsync(contextGuid, pLogOnPerson.PersonId, pLogOnPerson.DateTimeLogM, token), pLogOnPerson.DateTimeLogM, pLogOnPerson.PersonId);
			}
			if (((ReturnedSaveFuncInfo)(await payamakService.SmsNetAccount.GetByIdAsync(((AttDepartmentAdminSms)setting.Payamak).IdSamane))).HasError)
			{
				Log.Error($"{DateTime.Now}: smsAccount has error in method SendSmsTaradodAsync_ \r\n IdSamane = {((AttDepartmentAdminSms)setting.Payamak).IdSamane}");
				return false;
			}
			for (int i = 0; i < 8; i++)
			{
				if (coupleTaradodCurDate[i].Vtaradod.Id == pLogOnPerson.Id)
				{
					if (setting.Payamak.SendVorodValed && mobPerson.Trim() != "")
					{
						List<int> list = await SendSmsAsync(contextGuid, setting, setting.Payamak.PayamakValedVorod, mobPerson, pLogOnPerson.DateTimeLogM, statusname, personInfo, ek, kk);
						if (list != null && list.Any())
						{
							SmsRefrenceIds.AddRange(list);
						}
					}
					if (((AttDepartmentAdminSms)setting.Payamak).SendVorodManage && mobManagments.Trim() != "")
					{
						List<int> list2 = await SendSmsAsync(contextGuid, setting, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageVorod, mobManagments, pLogOnPerson.DateTimeLogM, statusname, personInfo, ek, kk);
						if (list2 != null && list2.Any())
						{
							SmsRefrenceIds.AddRange(list2);
						}
					}
					break;
				}
				if (coupleTaradodCurDate[i].Ktaradod.Id != pLogOnPerson.Id)
				{
					continue;
				}
				if (setting.Payamak.SendKhorojValed && mobPerson.Trim() != "")
				{
					List<int> list3 = await SendSmsAsync(contextGuid, setting, setting.Payamak.PayamakValedKhoroj, mobPerson, pLogOnPerson.DateTimeLogM, statusname, personInfo, ek, kk);
					if (list3 != null && list3.Any())
					{
						SmsRefrenceIds.AddRange(list3);
					}
				}
				if (((AttDepartmentAdminSms)setting.Payamak).SendKhorojManage && mobManagments.Trim() != "")
				{
					List<int> list4 = await SendSmsAsync(contextGuid, setting, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageKhoroj, mobManagments, pLogOnPerson.DateTimeLogM, statusname, personInfo, ek, kk);
					if (list4 != null && list4.Any())
					{
						SmsRefrenceIds.AddRange(list4);
					}
				}
				break;
			}
			await pLogOnPerson.SaveSmsRefrencedIdsAsync(string.Join(",", SmsRefrenceIds).Trim(), token, 20);
			return true;
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Log.Error<string>(ex2.Message, "");
			throw;
		}
	}

	private static async Task SendSmsAbsentAsync(string contextGuid, CancellationToken token)
	{
		await Task.Delay(300000);
		while (!token.IsCancellationRequested)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 14;
				try
				{
					await Task.Delay(60000);
					TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
					if (setting == null)
					{
						Log.Error($"variable setting is null at {DateTime.Now}. therefor can not send sms absent ");
						await Task.Delay(300000);
						continue;
					}
					if (!CanSendSmsNow(setting, (StatuseTaradodEnumType)3))
					{
						await Task.Delay(300000);
						continue;
					}
					int minMinuteAfterShiftForSendSms = (int)((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMinMinutesAfterShift;
					int maxMinuteAfterShiftForSendSms = (int)((AttDepartmentAdminSms)setting.Payamak).SendAbsentSmsMaxMinutesAfterShift;
					if (minMinuteAfterShiftForSendSms < 10 || maxMinuteAfterShiftForSendSms < 15 || minMinuteAfterShiftForSendSms + 5 > maxMinuteAfterShiftForSendSms)
					{
						Log.Error($"minMinuteAfterShiftForSendSms ({minMinuteAfterShiftForSendSms}) should be >10 minute and maxMinuteAfterShiftForSendSms ({maxMinuteAfterShiftForSendSms}) should be >15. therefor can not send sms absent at {DateTime.Now} ");
						await Task.Delay(300000);
						continue;
					}
					SentSmsLog log = await GetSmsLogAsync(contextGuid, DateTime.Now, (StatuseTaradodEnumType)3);
					List<Person> list = await Person.GetAllAsync(contextGuid, new List<int>(), personStatus: true, token);
					if (list == null || !list.Any())
					{
						Log.Error($"variable persons is null or empty in method sent sms absent at {DateTime.Now} ");
						await Task.Delay(300000);
						continue;
					}
					TaradodFull fullTaradod = await TaradodFull.CreateAsync(contextGuid, calcAwaitableProperties: true, list.Select((Person p) => ((PersonBase)p).Id).ToList(), DateTime.Now, checkValidationVorodAndKHoroj: false, token);
					if (!(fullTaradod?.ListTaradodDets?.Any()).GetValueOrDefault())
					{
						continue;
					}
					List<SmsCalendar> notSendGroups = (await SmsCalendar.SelectAllAsync(contextGuid, token)).Where((SmsCalendar p) => p.DateM.Date == DateTime.Now.Date).ToList();
					foreach (TaradodFullDetView taradod in fullTaradod.ListTaradodDets)
					{
						if (taradod.CoupleTaradodCurrentDay == null)
						{
							Log.Error($"(CoupleTaradodCurrentDay) is null in metheod send sms absent at {DateTime.Now}");
							continue;
						}
						if (taradod.CoupleTaradodCurrentDay.Count() <= 0)
						{
							Log.Error($"(CoupleTaradodCurrentDay) is empty in metheod send sms absent at {DateTime.Now} for {taradod.FullName}");
						}
						if (log.ExitsForPersonId(((PersonBase)taradod.PersonInfo).Id) || taradod?.ShiftDetViews == null || taradod.ShiftDetViews.Count() <= 0)
						{
							continue;
						}
						List<ShiftCodeing> list2 = taradod.ShiftDetViews.Select((ShiftDetView p) => p.ClsShiftCoding).ToList();
						if (list2 == null || list2.Count() <= 0)
						{
							continue;
						}
						DateTime dateTime = list2.Min((ShiftCodeing p) => p.VorodStart);
						if (DateTime.Now < dateTime.AddMinutes(minMinuteAfterShiftForSendSms) || DateTime.Now > dateTime.AddMinutes(maxMinuteAfterShiftForSendSms))
						{
							continue;
						}
						token.ThrowIfCancellationRequested();
						if ((int)(await taradod.GetStatuseTaradodAsync(token)) != 3)
						{
							continue;
						}
						if (taradod.TaradodsCurrentDay == null)
						{
							Log.Error("ترددهای روز جاری نال بود لذا چون نمیشه تصمیم درست بگیریم نباید پیامک غیبت ارسال میشد\r\n user family:" + ((PersonBase)taradod.PersonInfo).FullName);
							continue;
						}
						if (taradod.TaradodsCurrentDay.Count > 0)
						{
							Log.Error("با اینکه حداقل یک تردد داشته ولی وضعیت غیبت بود و نباید پیامک غیبت ارسال میشد\r\n taradods:" + string.Join(",", taradod.TaradodsCurrentDay) + "\r\n user family:" + ((PersonBase)taradod.PersonInfo).FullName);
							continue;
						}
						if (setting.Payamak.IsSendSmsAbsentPerson)
						{
							if (notSendGroups == null || notSendGroups.Count <= 0)
							{
								await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)3, setting.Payamak.SmsAbsentPerson, ((PersonBase)taradod.PersonInfo).AllNumbers, taradod, log, 0, 0, token);
							}
							else if (notSendGroups.FirstOrDefault((SmsCalendar p) => p.DateM.Date == DateTime.Now.Date && p.GroupId == ((PersonBase)taradod.PersonInfo).GroupId) == null)
							{
								await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)3, setting.Payamak.SmsAbsentPerson, ((PersonBase)taradod.PersonInfo).AllNumbers, taradod, log, 0, 0, token);
							}
						}
						if (((AttDepartmentAdminSms)setting.Payamak).IsSendSmsAbsentManager)
						{
							if (notSendGroups == null || notSendGroups.Count <= 0)
							{
								await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)3, ((AttDepartmentAdminSms)setting.Payamak).SmsAbsentManager, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs, taradod, log, 0, 0, token);
							}
							else if (notSendGroups.FirstOrDefault((SmsCalendar p) => p.DateM.Date == DateTime.Now.Date && p.GroupId == ((PersonBase)taradod.PersonInfo).GroupId) == null)
							{
								await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)3, ((AttDepartmentAdminSms)setting.Payamak).SmsAbsentManager, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs, taradod, log, 0, 0, token);
							}
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SendSmsAbsentAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\ClsSms.cs", 407);
				}
			}
			finally
			{
				await Task.Delay(300000);
			}
		}
	}

	private static async Task SendSmsIncompleteAsync(string contextGuid, CancellationToken token)
	{
		await Task.Delay(600000);
		while (!token.IsCancellationRequested)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 8;
				try
				{
					await Task.Delay(60000);
					TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
					if (CanSendSmsNow(setting, (StatuseTaradodEnumType)2))
					{
						SentSmsLog log = await GetSmsLogAsync(contextGuid, DateTime.Now.Date.AddDays(-1.0), (StatuseTaradodEnumType)2);
						foreach (Person person in await Person.GetAllAsync(contextGuid, new List<int>(), personStatus: true, token))
						{
							if (log.ExitsForPersonId(((PersonBase)person).Id))
							{
								continue;
							}
							TaradodFull taradods = await TaradodFull.CreateAsync(contextGuid, DateTime.Now.Date.AddDays(-1.0), DateTime.Now.Date.AddMilliseconds(-1.0), person, null, checkValidationVorodAndKHoroj: false, token);
							bool flag = taradods.ListTaradodDets.SingleOrDefault() != null;
							if (flag)
							{
								flag = (int)(await taradods.ListTaradodDets.SingleOrDefault().GetStatuseTaradodAsync()) == 2;
							}
							if (flag)
							{
								if (setting.Payamak.IsSendSmsIncompletePerson)
								{
									await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)2, setting.Payamak.SmsIncompletePerson, ((PersonBase)person).AllNumbers, taradods.ListTaradodDets.SingleOrDefault(), log, 0, 0, token);
								}
								if (((AttDepartmentAdminSms)setting.Payamak).IsSendSmsIncompleteManager)
								{
									await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)2, ((AttDepartmentAdminSms)setting.Payamak).SmsIncompleteManager, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs, taradods.ListTaradodDets.SingleOrDefault(), log, 0, 0, token);
								}
							}
						}
						goto end_IL_0137;
					}
					await Task.Delay(600000);
					goto end_IL_00ac;
					end_IL_0137:;
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SendSmsIncompleteAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\ClsSms.cs", 442);
				}
				end_IL_00ac:;
			}
			finally
			{
				await Task.Delay(600000);
			}
		}
	}

	private static async Task SendSmsVacationAsync(string contextGuid, CancellationToken token)
	{
		await Task.Delay(1200000);
		while (!token.IsCancellationRequested)
		{
			try
			{
				int num;
				_ = num - 1;
				_ = 7;
				try
				{
					await Task.Delay(60000);
					TimeAttendanceSettings setting = await TimeAttendanceSettings.GeTimeAttendanceSettingsAsync(contextGuid, token);
					if (!CanSendSmsNow(setting, (StatuseTaradodEnumType)5))
					{
						await Task.Delay(600000);
						continue;
					}
					SentSmsLog log = await GetSmsLogAsync(contextGuid, DateTime.Now.Date, (StatuseTaradodEnumType)5);
					if (log.Persons.Count <= 0)
					{
						IEnumerable<AttPersonLeaveBusiness> enumerable = await AttPersonLeaveBusiness.GetAllAsync(contextGuid, default(CancellationToken), "");
						if (enumerable != null)
						{
							IEnumerable<AttPersonLeaveBusiness> source = enumerable.Where((AttPersonLeaveBusiness p) => p.DateFrom <= DateTime.Now.Date && p.DateTo >= DateTime.Now.Date && ((int)p.Kind == 2 || (int)p.Kind == 1 || (int)p.Kind == 3 || (int)p.Kind == 5) && (int)p.LeaveStatus == 2);
							if (source.Any())
							{
								IEnumerable<AttPersonLeaveBusiness> vacationDaily = source.Where((AttPersonLeaveBusiness p) => (int)p.Type == 1);
								IEnumerable<AttPersonLeaveBusiness> vacationHorly = source.Where((AttPersonLeaveBusiness p) => (int)p.Type == 0);
								List<Person> persons = await Person.GetAllAsync(contextGuid, new List<int>(), personStatus: true, token);
								if (persons != null && persons.Count > 0)
								{
									TaradodFullDet taradodFake = new TaradodFullDet
									{
										PersonInfo = new Person(contextGuid)
									};
									if (vacationDaily.Any())
									{
										string text = "\ud83d\ude00لیست مرخصی روزانه تاریخ " + Calendar.ToShamsiDate(DateTime.Now.Date) + "\r\n";
										List<string> list = new List<string>();
										foreach (AttPersonLeaveBusiness vacation2 in vacationDaily)
										{
											Person? person = persons.FirstOrDefault((Person p) => ((PersonBase)p).Guid == vacation2.PersonGuid);
											string text2 = ((person != null) ? ((PersonBase)person).FullName : null) ?? "نامشخص";
											if (vacation2.DateTo.Date == DateTime.Now.Date)
											{
												list.Add("\ud83d\udccc " + text2 + ": امروز\r\n");
												continue;
											}
											int days = (vacation2.DateTo.Date - DateTime.Now.Date).Days;
											switch (days)
											{
											case 1:
												list.Add("\ud83d\udccc\ud83d\udccc " + text2 + ": تا فردا\r\n");
												break;
											case 2:
												list.Add("\ud83d\udccc\ud83d\udccc\ud83d\udccc " + text2 + ": تا پس فردا\r\n");
												break;
											default:
												list.Add("\ud83d\udccc\ud83d\udccc\ud83d\udccc\ud83d\udccc " + text2 + $": تا {days} روز دیگه\r\n");
												break;
											}
										}
										if (list.Count > 1)
										{
											list.Sort();
										}
										foreach (string item in list)
										{
											text += item;
										}
										await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)5, text, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs, taradodFake, log, 0, 0, token);
									}
									if (vacationHorly.Any())
									{
										string text3 = "لیست مرخصی ساعتی تاریخ " + Calendar.ToShamsiDate(DateTime.Now.Date) + "\r\n";
										foreach (AttPersonLeaveBusiness vacation in vacationHorly)
										{
											Person? person2 = persons.FirstOrDefault((Person p) => ((PersonBase)p).Guid == vacation.PersonGuid);
											string text4 = ((person2 != null) ? ((PersonBase)person2).FullName : null) ?? "نامشخص";
											text3 = text3 + text4 + $": از {vacation.DateFrom.TimeOfDay} تا {vacation.DateTo.TimeOfDay}\r\n";
										}
										await SendSmsGenericAsync(contextGuid, setting, (StatuseTaradodEnumType)9, text3, ((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs, taradodFake, log, 0, 0, token);
									}
								}
							}
						}
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.ErrorLogInstance.StartLog(ex2, "", "SendSmsVacationAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\ClsSms.cs", 531);
				}
			}
			finally
			{
				await Task.Delay(1800000);
			}
			int num2;
			if (num2 == 2)
			{
				break;
			}
		}
	}

	private static async Task SendSmsGenericAsync(string contextGuid, TimeAttendanceSettings setting, StatuseTaradodEnumType status, string template, string numbers, TaradodFullDet taradod, SentSmsLog log, int ek, int kk, CancellationToken token)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		_ = 2;
		try
		{
			if (((ReturnedSaveFuncInfo)(await PayamakClientHubInstances.GetInstance(contextGuid).SmsNetAccount.GetByIdAsync(((AttDepartmentAdminSms)setting.Payamak).IdSamane))).HasError)
			{
				Log.Error($"{DateTime.Now}: smsAccount has error in method SendSmsGenericAsync \r\n IdSamane = {((AttDepartmentAdminSms)setting.Payamak).IdSamane}");
				return;
			}
			List<int> list = await SendSmsAsync(contextGuid, setting, template, numbers, Utilities.CalendarToMiladi(log.DateSh), Utilities.GetDisplay<StatuseTaradodEnumType>(status), taradod.PersonInfo, ek, kk, token);
			if (list != null && list.Any())
			{
				log.Persons.Add(new SentSmsLogDet
				{
					Date = DateTime.Now,
					Desc = "",
					Message = "",
					Mobiles = ((PersonBase)taradod.PersonInfo).AllNumbers,
					PersonId = ((PersonBase)taradod.PersonInfo).Id,
					RefrencedIds = string.Join(",", list)
				});
				await SetSmsLogAsync(contextGuid, log, status);
			}
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private static async Task<SentSmsLog> GetSmsLogAsync(string contextGuid, DateTime date, StatuseTaradodEnumType status)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			string text = $"{status}_{Calendar.MiladiToCalture(date)}";
			string text2 = await LocalSettingBusiness.GetAsync(contextGuid, text, true, default(CancellationToken), "");
			return (SentSmsLog)((!string.IsNullOrEmpty(text2)) ? ((object)Json.FromJson<SentSmsLog>(text2, true, "GetSmsLogAsync", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\Other\\ClsSms.cs", 582)) : ((object)new SentSmsLog
			{
				DateSh = Calendar.MiladiToCalture(date)
			}));
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private static async Task SetSmsLogAsync(string contextGuid, SentSmsLog log, StatuseTaradodEnumType status)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			log.LastUpdate = DateTime.Now;
			string text = $"{status}_{log.DateSh}";
			await LocalSettingBusiness.SaveAsync(contextGuid, text, Json.ToStringJson((object)log, false), true, (short)10, default(CancellationToken), "", false);
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}

	private static bool CanSendSmsNow(TimeAttendanceSettings setting, StatuseTaradodEnumType status)
	{
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected I4, but got Unknown
		try
		{
			if (setting == null)
			{
				Log.Error("variable (setting) is null therefore can not send sms");
				return false;
			}
			if (!((AttDepartmentAdminSms)setting.Payamak).EnablePayamak)
			{
				return false;
			}
			if ((decimal)(int)DateTime.Now.TimeOfDay.TotalMinutes < ((AttDepartmentAdminSms)setting.Payamak).SendSmsFromClockOfDay * 60m)
			{
				return false;
			}
			if ((decimal)(int)DateTime.Now.TimeOfDay.TotalMinutes > ((AttDepartmentAdminSms)setting.Payamak).SendSmsToClockOfDay * 60m)
			{
				return false;
			}
			switch ((int)status)
			{
			case 0:
				if (!setting.Payamak.SendVorodValed && !setting.Payamak.SendKhorojValed && !((AttDepartmentAdminSms)setting.Payamak).SendVorodManage && !((AttDepartmentAdminSms)setting.Payamak).SendKhorojManage)
				{
					return false;
				}
				break;
			case 2:
				if ((decimal)(int)DateTime.Now.TimeOfDay.TotalMinutes < ((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsFromClock * 60m)
				{
					return false;
				}
				if ((decimal)(int)DateTime.Now.TimeOfDay.TotalMinutes > ((AttDepartmentAdminSms)setting.Payamak).SendIncompleteSmsToClock * 60m)
				{
					return false;
				}
				if (!setting.Payamak.IsSendSmsIncompletePerson && !((AttDepartmentAdminSms)setting.Payamak).IsSendSmsIncompleteManager)
				{
					return false;
				}
				break;
			case 3:
				if (!((AttDepartmentAdminSms)setting.Payamak).IsSendSmsAbsentManager && !setting.Payamak.IsSendSmsAbsentPerson)
				{
					return false;
				}
				break;
			case 5:
				if (string.IsNullOrEmpty(((AttDepartmentAdminSms)setting.Payamak).PayamakManageAdminMobs))
				{
					return false;
				}
				if (!setting.Payamak.IsSendSmsVacationManager)
				{
					return false;
				}
				break;
			}
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(ex, "");
			throw;
		}
	}
}
