using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Business.Report;

public class SumKolBusiness : IHasContext
{
	public Guid KolGuid { get; set; }

	public int KolCode { get; set; }

	public string KolName { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public DateTime? StartDate { get; internal set; }

	public DateTime? EndDate { get; internal set; }

	public string DateRange { get; internal set; }

	public SanadType SanadType { get; set; }

	public DateTime? DateM { get; set; }

	public string Description
	{
		get
		{
			if (DateM.HasValue)
			{
				return "به شرح دفتر روزنامه";
			}
			return "مانده از قبل";
		}
	}

	public string ContextGuid { get; set; }

	public EnDaftarRoznameReportType TajmieeType { get; private set; }

	public string SumDescription { get; internal set; }

	public decimal SumDebit { get; set; }

	public decimal SumCredit { get; set; }

	public int Year
	{
		get
		{
			if (StartDate.HasValue)
			{
				return StartDate.Value.GetYearOfDateSh();
			}
			return 0;
		}
	}

	public string YearForDisplay
	{
		get
		{
			if (SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye || SanadType == SanadType.CostAndBenefit)
			{
				return "";
			}
			return Year.ToString();
		}
	}

	public int Month
	{
		get
		{
			if (StartDate.HasValue)
			{
				return StartDate.Value.GetMonthOfDate();
			}
			return 0;
		}
	}

	public string MonthForDisplay
	{
		get
		{
			if (SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye || SanadType == SanadType.CostAndBenefit)
			{
				return SanadType.GetDisplay();
			}
			return Month.GetMonthName();
		}
	}

	public int Day
	{
		get
		{
			if (StartDate.HasValue)
			{
				return StartDate.Value.GetDayOfDateSh();
			}
			return 0;
		}
	}

	public string DayForDisplay
	{
		get
		{
			if (SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye || SanadType == SanadType.CostAndBenefit)
			{
				return "";
			}
			switch (TajmieeType)
			{
			case EnDaftarRoznameReportType.Tajmiee_Days:
				return Day.ToString();
			case EnDaftarRoznameReportType.Tajmiee_Week:
			case EnDaftarRoznameReportType.Tajmiee_Month:
				return "";
			default:
				return Day.ToString();
			}
		}
	}

	public string DateSh
	{
		get
		{
			if (!string.IsNullOrEmpty(DateRange))
			{
				return DateRange;
			}
			if (SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye || SanadType == SanadType.CostAndBenefit)
			{
				return SanadType.GetDisplay();
			}
			return DateM.MiladiToCalture();
		}
	}

	public decimal Remain { get; set; }

	public string RemainForDisplay
	{
		get
		{
			try
			{
				if (Remain == 0m)
				{
					return "بی حساب".GetTranslate();
				}
				if (Remain < 0m)
				{
					return Math.Abs(Remain).NpToString("N0", 0);
				}
				return Remain.NpToString("N0", 0);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "RemainForDisplay", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\SumKolBusiness.cs", 92);
				return "";
			}
		}
	}

	public int Ordinal { get; internal set; }

	public SumKolBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static async Task<IEnumerable<SumKolBusiness>> GetSumKolListAsync(string contextGuid, SumKolFilter filter, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		try
		{
			IEnumerable<SumKolBusiness> obj = await ClsCache.GetCache(contextGuid).CacheRepository.Reports.GetSumKolListAsync(filter, token, tranName);
			CalculateRem(obj?.ToList());
			return obj?.OrderBy((SumKolBusiness q) => q.KolCode)?.ThenBy((SumKolBusiness q) => q.DateM)?.ThenBy((SumKolBusiness q) => q.SanadType)?.ThenBy((SumKolBusiness q) => q.Credit > 0m);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetSumKolListAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\SumKolBusiness.cs", 111);
			throw;
		}
	}

	public static Task<List<SumKolBusiness>> GenerateSumAsync(string contextGuid, List<SumKolBusiness> listDet, EnDaftarRoznameReportType tajmieeType)
	{
		if (listDet == null || !listDet.Any())
		{
			throw new NullReferenceException("listDet");
		}
		checked
		{
			try
			{
				listDet = listDet.OrderBy((SumKolBusiness x) => x.DateM).ThenBy((SumKolBusiness q) => q.KolCode).ToList();
				List<SumKolBusiness> list = new List<SumKolBusiness>();
				foreach (SumKolBusiness item2 in listDet)
				{
					int num = 0;
					if (item2.SanadType == SanadType.Automatic)
					{
						item2.SanadType = SanadType.Movaghat;
					}
					DateTime? startDate = GetStartDate(item2.DateM, tajmieeType);
					DateTime? endDate = GetEndDate(item2.DateM, tajmieeType);
					SumKolBusiness sumKolBusiness = list.SingleOrDefault(delegate(SumKolBusiness q)
					{
						if (q.StartDate == startDate)
						{
							DateTime? endDate2 = q.EndDate;
							DateTime? dateTime = endDate;
							if (endDate2.HasValue == dateTime.HasValue && (!endDate2.HasValue || endDate2.GetValueOrDefault() == dateTime.GetValueOrDefault()) && q.KolCode == item2.KolCode && q.SanadType == item2.SanadType)
							{
								if (!(item2.Debit > 0m) || !(q.Debit > 0m))
								{
									if (item2.Credit > 0m)
									{
										return q.Credit > 0m;
									}
									return false;
								}
								return true;
							}
						}
						return false;
					});
					if (sumKolBusiness == null)
					{
						List<SumKolBusiness> list2 = list;
						SumKolBusiness obj = new SumKolBusiness(contextGuid)
						{
							KolCode = item2.KolCode,
							ContextGuid = contextGuid,
							Credit = item2.Credit,
							DateRange = GetDateRange(item2.DateM, tajmieeType, item2),
							Debit = item2.Debit,
							EndDate = endDate,
							KolName = item2.KolName,
							StartDate = startDate,
							TajmieeType = tajmieeType,
							SanadType = item2.SanadType,
							DateM = item2.DateM,
							KolGuid = item2.KolGuid
						};
						num = (obj.Ordinal = num + 1);
						list2.Add(obj);
					}
					else
					{
						sumKolBusiness.Debit += item2.Debit;
						sumKolBusiness.Credit += item2.Credit;
					}
				}
				foreach (var item in (from q in list
					group q by new { q.StartDate, q.EndDate, q.SanadType })?.ToList())
				{
					decimal sumDebit = item.Sum((SumKolBusiness p) => p.Debit);
					decimal sumCredit = item.Sum((SumKolBusiness p) => p.Credit);
					foreach (SumKolBusiness item3 in list.Where(delegate(SumKolBusiness q)
					{
						DateTime? startDate2 = q.StartDate;
						DateTime? startDate3 = item.Key.StartDate;
						return startDate2.HasValue == startDate3.HasValue && (!startDate2.HasValue || startDate2.GetValueOrDefault() == startDate3.GetValueOrDefault()) && q.EndDate == item.Key.EndDate && q.SanadType == item.Key.SanadType;
					})?.ToList())
					{
						item3.SumDebit = sumDebit;
						item3.SumCredit = sumCredit;
						item3.SumDescription = GetDescription(item3.StartDate, tajmieeType, item3);
					}
				}
				list = list?.OrderBy((SumKolBusiness q) => q.StartDate)?.ThenBy((SumKolBusiness q) => q.SanadType)?.ThenBy((SumKolBusiness q) => q.Credit > 0m)?.ThenBy((SumKolBusiness q) => q.KolCode)?.ToList();
				foreach (var item4 in list?.GroupBy((SumKolBusiness q) => new { q.DateRange, q.SanadType })?.ToList())
				{
					int num3 = 1;
					foreach (SumKolBusiness item5 in item4)
					{
						item5.Ordinal = num3++;
					}
				}
				CalculateRem(list);
				return Task.FromResult(list?.OrderBy((SumKolBusiness q) => q.StartDate)?.ThenBy((SumKolBusiness q) => q.SanadType)?.ThenBy((SumKolBusiness q) => q.Credit > 0m)?.ThenBy((SumKolBusiness q) => q.KolCode)?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\SumKolBusiness.cs", 189);
				throw;
			}
		}
	}

	private static void CalculateRem(List<SumKolBusiness> listDet)
	{
		try
		{
			if (listDet == null || !listDet.Any())
			{
				return;
			}
			foreach (IGrouping<Guid, SumKolBusiness> item in from q in listDet
				group q by q.KolGuid)
			{
				decimal num = default(decimal);
				foreach (SumKolBusiness item2 in item.Select((SumKolBusiness q) => q))
				{
					num = (item2.Remain = num + item2.Credit - item2.Debit);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "CalculateRem", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\SumKolBusiness.cs", 210);
		}
	}

	private static DateTime? GetStartDate(DateTime? dateM, EnDaftarRoznameReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return dateM;
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => new DateTime(dateM.Value.Year, dateM.Value.Month, dateM.Value.Day, 0, 0, 0), 
			EnDaftarRoznameReportType.Tajmiee_Week => dateM.Value.GetFirstDayOfWeek(), 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.Value.StartDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static DateTime? GetEndDate(DateTime? dateM, EnDaftarRoznameReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return dateM;
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => new DateTime(dateM.Value.Year, dateM.Value.Month, dateM.Value.Day, 23, 59, 59), 
			EnDaftarRoznameReportType.Tajmiee_Week => dateM.Value.GetEndDayOfWeek(), 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.Value.EndDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDateRange(DateTime? dateM, EnDaftarRoznameReportType tajmieeType, SumKolBusiness item)
	{
		if (!dateM.HasValue)
		{
			return "";
		}
		if (item.SanadType == SanadType.Eftetahiye)
		{
			return dateM.MiladiToCalture();
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => dateM.MiladiToCalture(), 
			EnDaftarRoznameReportType.Tajmiee_Week => "هفته".GetTranslate() + $"({dateM.Value.GetWeekNumber()}){Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.Value.EndDayOfPersianMonth().MiladiToCalture(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDescription(DateTime? dateM, EnDaftarRoznameReportType tajmieeType, SumKolBusiness item)
	{
		if (!dateM.HasValue)
		{
			return "";
		}
		if (item.SanadType == SanadType.Eftetahiye || item.SanadType == SanadType.Ekhtetamiye || item.SanadType == SanadType.CostAndBenefit)
		{
			return "بابت سند".GetTranslate() + item.SanadType.GetDisplay().GetTranslate();
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => "بابت سند روز".GetTranslate() + " " + dateM.MiladiToCalture(), 
			EnDaftarRoznameReportType.Tajmiee_Week => "بابت سند هفته".GetTranslate() + $"({dateM.Value.GetWeekNumber()}){Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			EnDaftarRoznameReportType.Tajmiee_Month => "بابت سند ماه".GetTranslate() + " " + Calendar.GetMonthName(dateM.Value), 
			_ => throw new NotImplementedException(), 
		};
	}
}
