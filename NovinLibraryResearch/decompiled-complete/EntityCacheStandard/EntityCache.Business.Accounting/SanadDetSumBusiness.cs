using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business.Accounting;

public class SanadDetSumBusiness : IHasContext
{
	public string KolName { get; internal set; }

	public long CodeKol { get; internal set; }

	public decimal Debit { get; internal set; }

	public decimal Credit { get; internal set; }

	public int Ordinal { get; internal set; }

	public DateTime StartDate { get; internal set; }

	public DateTime EndDate { get; internal set; }

	public string DateRange { get; internal set; }

	public EnDaftarRoznameReportType TajmieeType { get; internal set; }

	public string ContextGuid { get; internal set; }

	public SanadType SanadType { get; set; }

	public string SumDescription { get; internal set; }

	public decimal SumDebit { get; internal set; }

	public decimal SumCredit { get; internal set; }

	public int Year => StartDate.GetYearOfDateSh();

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

	public int Month => StartDate.GetMonthOfDate();

	public string MonthForDisplay
	{
		get
		{
			if (SanadType == SanadType.Eftetahiye || SanadType == SanadType.Ekhtetamiye || SanadType == SanadType.CostAndBenefit)
			{
				return SanadType.GetDisplay().GetTranslate();
			}
			return Month.GetMonthName();
		}
	}

	public int Day => StartDate.GetDayOfDateSh();

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

	public int Index { get; set; }

	public SanadDetSumBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public static Task<List<SanadDetSumBusiness>> GenerateSumAsync(string contextGuid, List<SanadDetRoznameKolBusiness> listDet, EnDaftarRoznameReportType tajmieeType)
	{
		if (listDet == null || !listDet.Any())
		{
			throw new NullReferenceException("listDet");
		}
		checked
		{
			try
			{
				listDet = listDet.OrderBy((SanadDetRoznameKolBusiness x) => x.DateM).ThenBy((SanadDetRoznameKolBusiness q) => q.CodeKol).ToList();
				List<SanadDetSumBusiness> list = new List<SanadDetSumBusiness>();
				foreach (SanadDetRoznameKolBusiness item2 in listDet)
				{
					int num = 0;
					if (item2.SanadType == SanadType.Automatic)
					{
						item2.SanadType = SanadType.Movaghat;
					}
					DateTime startDate = GetStartDate(item2.DateM, tajmieeType);
					DateTime endDate = GetEndDate(item2.DateM, tajmieeType);
					SanadDetSumBusiness sanadDetSumBusiness = list.SingleOrDefault((SanadDetSumBusiness q) => q.StartDate == startDate && q.EndDate == endDate && q.CodeKol == item2.CodeKol && q.SanadType == item2.SanadType && ((item2.Debit > 0m && q.Debit > 0m) || (item2.Credit > 0m && q.Credit > 0m)));
					if (sanadDetSumBusiness == null)
					{
						List<SanadDetSumBusiness> list2 = list;
						SanadDetSumBusiness obj = new SanadDetSumBusiness(contextGuid)
						{
							CodeKol = item2.CodeKol,
							ContextGuid = contextGuid,
							Credit = item2.Credit,
							DateRange = GetDateRange(item2.DateM, tajmieeType, item2),
							Debit = item2.Debit,
							EndDate = endDate,
							KolName = item2.KolName,
							StartDate = startDate,
							TajmieeType = tajmieeType,
							SanadType = item2.SanadType
						};
						num = (obj.Ordinal = num + 1);
						list2.Add(obj);
					}
					else
					{
						sanadDetSumBusiness.Debit += item2.Debit;
						sanadDetSumBusiness.Credit += item2.Credit;
					}
				}
				var obj2 = list?.OrderBy((SanadDetSumBusiness q) => q.StartDate)?.ThenBy((SanadDetSumBusiness q) => q.SanadType)?.GroupBy((SanadDetSumBusiness q) => new { q.StartDate, q.EndDate, q.SanadType })?.ToList();
				int num3 = 0;
				foreach (var item in obj2)
				{
					num3++;
					decimal sumDebit = item.Sum((SanadDetSumBusiness p) => p.Debit);
					decimal sumCredit = item.Sum((SanadDetSumBusiness p) => p.Credit);
					foreach (SanadDetSumBusiness item3 in list.Where((SanadDetSumBusiness q) => q.StartDate == item.Key.StartDate && q.EndDate == item.Key.EndDate && q.SanadType == item.Key.SanadType)?.ToList())
					{
						item3.SumDebit = sumDebit;
						item3.SumCredit = sumCredit;
						item3.SumDescription = GetDescription(item3.StartDate, tajmieeType, item3);
						item3.Index = num3;
					}
				}
				list = list?.OrderBy((SanadDetSumBusiness q) => q.StartDate)?.ThenBy((SanadDetSumBusiness q) => q.SanadType)?.ThenBy((SanadDetSumBusiness q) => q.Credit > 0m)?.ThenBy((SanadDetSumBusiness q) => q.CodeKol)?.ToList();
				foreach (var item4 in list?.GroupBy((SanadDetSumBusiness q) => new { q.DateRange, q.SanadType })?.ToList())
				{
					int num4 = 1;
					foreach (SanadDetSumBusiness item5 in item4)
					{
						item5.Ordinal = num4++;
					}
				}
				return Task.FromResult(list?.OrderBy((SanadDetSumBusiness q) => q.StartDate)?.ThenBy((SanadDetSumBusiness q) => q.SanadType)?.ThenBy((SanadDetSumBusiness q) => q.Credit > 0m)?.ThenBy((SanadDetSumBusiness q) => q.CodeKol)?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Models\\SanadDetSumBusiness.cs", 143);
				throw;
			}
		}
	}

	private static DateTime GetStartDate(DateTime dateM, EnDaftarRoznameReportType tajmieeType)
	{
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => new DateTime(dateM.Year, dateM.Month, dateM.Day, 0, 0, 0), 
			EnDaftarRoznameReportType.Tajmiee_Week => dateM.GetFirstDayOfWeek(), 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.StartDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static DateTime GetEndDate(DateTime dateM, EnDaftarRoznameReportType tajmieeType)
	{
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => new DateTime(dateM.Year, dateM.Month, dateM.Day, 23, 59, 59), 
			EnDaftarRoznameReportType.Tajmiee_Week => dateM.GetEndDayOfWeek(), 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.EndDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDateRange(DateTime dateM, EnDaftarRoznameReportType tajmieeType, SanadDetRoznameKolBusiness item)
	{
		if (item.SanadType == SanadType.Eftetahiye)
		{
			return dateM.MiladiToCalture();
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => dateM.MiladiToCalture(), 
			EnDaftarRoznameReportType.Tajmiee_Week => "هفته".GetTranslate() + $"({dateM.GetWeekNumber()}){Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			EnDaftarRoznameReportType.Tajmiee_Month => dateM.EndDayOfPersianMonth().MiladiToCalture(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDescription(DateTime dateM, EnDaftarRoznameReportType tajmieeType, SanadDetSumBusiness item)
	{
		if (item.SanadType == SanadType.Eftetahiye || item.SanadType == SanadType.Ekhtetamiye || item.SanadType == SanadType.CostAndBenefit)
		{
			return "بابت سند".GetTranslate() + "  " + item.SanadType.GetDisplay().GetTranslate();
		}
		return tajmieeType switch
		{
			EnDaftarRoznameReportType.Tajmiee_Days => "بابت سند روز".GetTranslate() + " " + dateM.MiladiToCalture(), 
			EnDaftarRoznameReportType.Tajmiee_Week => "بابت سند هفته".GetTranslate() + $"({dateM.GetWeekNumber()}){Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			EnDaftarRoznameReportType.Tajmiee_Month => "بابت سند ماه".GetTranslate() + " " + Calendar.GetMonthName(dateM), 
			_ => throw new NotImplementedException(), 
		};
	}
}
