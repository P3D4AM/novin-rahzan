using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Models.Accounting;
using PacketParser;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Entities.DepartmentEntitySummery;
using PacketParser.Records;
using PacketParser.Records.Kardex;
using PacketParser.Records.OccasionCalendar;
using PacketParser.Settings.Accounting;

namespace EntityCache.Business.Report;

public class GlobalReportBusiness
{
	public static Task<IEnumerable<AccountTreeModel>> GetAllAccountTreeAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetAllAccountTreeAsync(token, tranName);
	}

	public static Task<PayamakManage> GetPayamakManageAsync(string contextGuid, DateTime d1, DateTime d2, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetPayamakManageAsync(d1, d2, token, tranName);
	}

	public static Task<WebDepartmentEntitySummery> GetDepartmentEntitySummeryAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository?.Reports?.GetDepartmentEntitySummeryAsync(token, tranName) ?? Task.FromResult<WebDepartmentEntitySummery>(null);
	}

	public static async Task<List<OccasionCalendarRecord>> GetOccasionCalendarRecordListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			int mount = Calendar.GetMonthOfDate(DateTime.Now.MiladiToCalture());
			int day = Calendar.GetDayOfDateSh(DateTime.Now.MiladiToCalture());
			if (!((await LocalSettingBusiness.GetSettingAsync(contextGuid, token))?.Payamak?.OccasionCalendar?.Any()).GetValueOrDefault())
			{
				return null;
			}
			List<OccasionCalendarRecord> list = await ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetOccasionCalendarListAsync(token, tranName);
			if (list == null || !list.Any())
			{
				return null;
			}
			list = list.Where((OccasionCalendarRecord q) => !string.IsNullOrEmpty(q.Model?.Value) && Calendar.GetMonthOfDate(q.Model.Value) == mount && Calendar.GetDayOfDateSh(q.Model.Value) == day)?.ToList();
			if (list == null || !list.Any())
			{
				return null;
			}
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOccasionCalendarRecordListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\GlobalReportBusiness.cs", 50);
			throw;
		}
	}

	public static async Task<List<OccasionCalendarSmsRecord>> GetOccasionCalendarListAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 3;
		try
		{
			List<OccasionCalendarRecord> dbList = await GetOccasionCalendarRecordListAsync(contextGuid, token, tranName);
			if (!(dbList?.Any() ?? false))
			{
				return null;
			}
			List<OccasionCalendarSmsRecord> list = await LocalSettingBusiness.GetOccasionCalendarSmsAsync(contextGuid, DateTime.Now);
			if (list != null && list.Any())
			{
				new List<OccasionCalendarSmsRecord>();
				foreach (OccasionCalendarSmsRecord item in list)
				{
					List<OccasionCalendarRecord> list2 = dbList.Where((OccasionCalendarRecord q) => q.ParentGuid == item.ParentGuid && q.Model.Title == item.Title)?.ToList();
					if (list2 == null || !list2.Any())
					{
						continue;
					}
					foreach (OccasionCalendarRecord item2 in list2)
					{
						dbList.Remove(item2);
					}
				}
			}
			if (!(dbList?.Any() ?? false))
			{
				return null;
			}
			AccGlobalSettings setting = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			List<OccasionCalendarSmsRecord> results = new List<OccasionCalendarSmsRecord>();
			foreach (OccasionCalendarRecord item3 in dbList)
			{
				List<OccasionCalendarSmsRecord> list3 = await FillOccasionCalendarListAsync(contextGuid, setting, item3, token, tranName);
				if (list3 != null && list3.Any())
				{
					results.AddRange(list3);
				}
			}
			return results;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\GlobalReportBusiness.cs", 87);
			throw;
		}
	}

	private static async Task<List<OccasionCalendarSmsRecord>> FillOccasionCalendarListAsync(string contextGuid, AccGlobalSettings setting, OccasionCalendarRecord dbRecord, CancellationToken token, string tranName)
	{
		try
		{
			if (setting == null || dbRecord?.Model == null)
			{
				return null;
			}
			OccasionCalendarModel settingOccasion = setting.Payamak.OccasionCalendar.SingleOrDefault((OccasionCalendarModel q) => q.Title == dbRecord.Model.Title);
			if (settingOccasion == null)
			{
				return null;
			}
			if (settingOccasion.StartSendTime.HasValue && settingOccasion.EndSendTime.HasValue)
			{
				DateTime dateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settingOccasion.StartSendTime.Value.Hour, settingOccasion.StartSendTime.Value.Minute, 0);
				DateTime dateTime2 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, settingOccasion.EndSendTime.Value.Hour, settingOccasion.EndSendTime.Value.Minute, 0);
				if (DateTime.Now < dateTime || DateTime.Now > dateTime2)
				{
					return null;
				}
			}
			CusCompactBusiness cusCompactBusiness = await CusCompactBusiness.GetAsync(contextGuid, EnSystem.None, dbRecord.ParentGuid, null, tranName, token);
			if (cusCompactBusiness == null)
			{
				return null;
			}
			string text = OccasionCalendarModel.FillSmsText(settingOccasion.SmsPattern, cusCompactBusiness.Name, cusCompactBusiness.Family, cusCompactBusiness.FullName);
			if (string.IsNullOrEmpty(text))
			{
				return null;
			}
			List<TafsilNumberBusiness> list = (await TafsilNumberBusiness.GetAsync(contextGuid, dbRecord.ParentGuid, token, tranName))?.Where((TafsilNumberBusiness q) => q.Mobile)?.ToList();
			if (list == null || !list.Any())
			{
				return null;
			}
			List<OccasionCalendarSmsRecord> list2 = new List<OccasionCalendarSmsRecord>();
			list2.AddRange(list.Select((TafsilNumberBusiness q) => new OccasionCalendarSmsRecord
			{
				Number = q.CalcedNumber,
				ParentGuid = dbRecord.ParentGuid,
				Text = text,
				Title = dbRecord.Model.Title
			}));
			return list2;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "FillOccasionCalendarListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\GlobalReportBusiness.cs", 127);
			throw;
		}
	}

	public static async Task<List<ServerStatusReportModel>> GetServerStatusAsync(string contextGuid, bool waitForCache, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			ClsCache cache = ClsCache.GetCache(contextGuid);
			if (cache == null || !cache.BufferEnable)
			{
				if (!waitForCache)
				{
					return null;
				}
				await Task.Delay(5000);
				if (token.IsCancellationRequested)
				{
					return null;
				}
				cache = ClsCache.GetCache(contextGuid);
				if (cache == null || !cache.BufferEnable)
				{
					return null;
				}
			}
			List<ServerStatusForClient> serverStatuses = new List<ServerStatusForClient>
			{
				ServerStatusForClient.SendError,
				ServerStatusForClient.DeliveryError,
				ServerStatusForClient.None
			};
			List<ServerStatusReportModel> list = await ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetServerStatusAsync(cache.Source, serverStatuses, token, tranName);
			if (list == null || !list.Any())
			{
				return null;
			}
			int num = 1;
			foreach (ServerStatusReportModel item in list)
			{
				item.Ordinal = checked(num++);
			}
			list.OrderBy((ServerStatusReportModel q) => q.Date);
			return list;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetServerStatusAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\GlobalReportBusiness.cs", 160);
			throw;
		}
	}

	public static async Task<IEnumerable<KardexAndAccountConflictRecord>> GetAllKardexAndAccountConflictAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			AccGlobalSettings accGlobalSettings = await LocalSettingBusiness.GetSettingAsync(contextGuid, token);
			if (accGlobalSettings.CompanyInfo.SanadType == 0)
			{
				return null;
			}
			return await ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetAllKardexAndAccountConflictAsync(accGlobalSettings.CodeSetting.Permanent.ProductMoeinGuid, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllKardexAndAccountConflictAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\GlobalReportBusiness.cs", 174);
			throw;
		}
	}

	public static Task<IEnumerable<DocumentRoundMismatchRecord>> GetAllDocumentRoundMismatchAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetAllDocumentRoundMismatchAsync(token, tranName);
	}
}
