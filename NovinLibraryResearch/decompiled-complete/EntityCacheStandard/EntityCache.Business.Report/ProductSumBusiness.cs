using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Filters;

namespace EntityCache.Business.Report;

public class ProductSumBusiness : IHasContext
{
	public string ContextGuid { get; internal set; }

	public DateTime DateM { get; internal set; }

	public DateTime? StartDate { get; internal set; }

	public DateTime? EndDate { get; internal set; }

	public string DateRange { get; internal set; }

	public decimal EnterQuantity { get; internal set; }

	public decimal VisitorSum { get; internal set; }

	public decimal EnterDiscount { get; internal set; }

	public decimal EnterAverage
	{
		get
		{
			if (!(EnterSum <= 0m))
			{
				return Math.Round(EnterSum / EnterQuantity, DigitForRound);
			}
			return 0m;
		}
	}

	public decimal EnterSum { get; internal set; }

	public decimal ExitQuantity { get; internal set; }

	public decimal ExitDiscount { get; internal set; }

	public decimal ExitAverage
	{
		get
		{
			if (!(ExitSum <= 0m))
			{
				return Math.Round(ExitSum / ExitQuantity, DigitForRound);
			}
			return 0m;
		}
	}

	public decimal ExitSum { get; internal set; }

	public string DateSh
	{
		get
		{
			if (!string.IsNullOrEmpty(DateRange))
			{
				return DateRange;
			}
			return DateM.MiladiToCalture();
		}
	}

	public string Description { get; internal set; }

	public int DigitForRound { get; private set; }

	public ProductSumBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
		DigitForRound = (ClsCache.GetCache(contextGuid)?.MultiLanguageConfig?.DigitForRound).GetValueOrDefault(3);
	}

	public static async Task<List<KardexTajmieReportBusiness>> GetAllAsync(string contextGuid, CancellationToken token, TajmieeProductFilter filter, string tranName = "")
	{
		return await ClsCache.GetCache(contextGuid).CacheRepository.Ikardex.GetAllTajmieProductAsync(token, filter, tranName);
	}

	public static async Task<List<ProductSumBusiness>> GenerateSumAsync(string contextGuid, Guid productGuid, TajmieeProductFilter filter, EnAggregateReportType aggregateType, CancellationToken token)
	{
		if (productGuid == Guid.Empty)
		{
			throw new NullReferenceException("productGuid");
		}
		try
		{
			List<ProductSumBusiness> list = new List<ProductSumBusiness>();
			KardexFilter kardexFilter = new KardexFilter
			{
				PrdGuid = productGuid,
				Date1 = filter.Date1,
				Date2 = filter.Date2,
				DepotGuid = filter.DepotGuid,
				TafsilGuid = filter.TafsilGuid,
				FactorType = filter.Type,
				Description = filter.Description,
				KardexAffective = filter.KardexAffective,
				PrdGroupGuid = filter.ProductGroupGuidList?.FirstOrDefault(),
				UnitPrice1 = filter.Price1,
				UnitPrice2 = filter.Price2,
				UserGuid = filter.CashierGuid
			};
			kardexFilter.PrdGroupGuid = default(Guid);
			ReturnedSaveFuncInfoWithValue<IEnumerable<KardexBusiness>> returnedSaveFuncInfoWithValue = await KardexBusiness.GetAllAsync(contextGuid, kardexFilter, token);
			if (returnedSaveFuncInfoWithValue?.HasError ?? true)
			{
				return null;
			}
			List<KardexBusiness> list2 = returnedSaveFuncInfoWithValue.value?.OrderBy((KardexBusiness x) => x.DateM)?.ThenBy((KardexBusiness q) => q.Ordinal)?.ToList();
			if (list2 == null || !list2.Any())
			{
				return null;
			}
			if ((filter?.DocumentList?.Any()).GetValueOrDefault())
			{
				list2 = list2?.Where((KardexBusiness q) => filter.DocumentList.Contains(q.SanadType))?.ToList();
			}
			if (list2 == null || !list2.Any())
			{
				return null;
			}
			foreach (KardexBusiness item in list2)
			{
				DateTime? startDate = GetStartDate(item.DateM, aggregateType);
				DateTime? endDate = GetEndDate(item.DateM, aggregateType);
				ProductSumBusiness productSumBusiness = list.SingleOrDefault(delegate(ProductSumBusiness q)
				{
					if (q.StartDate == startDate)
					{
						DateTime? endDate2 = q.EndDate;
						DateTime? dateTime = endDate;
						if (endDate2.HasValue != dateTime.HasValue)
						{
							return false;
						}
						if (!endDate2.HasValue)
						{
							return true;
						}
						return endDate2.GetValueOrDefault() == dateTime.GetValueOrDefault();
					}
					return false;
				});
				if (productSumBusiness == null)
				{
					list.Add(new ProductSumBusiness(contextGuid)
					{
						ContextGuid = contextGuid,
						DateRange = GetDateRange(item.DateM, startDate, endDate, aggregateType),
						EndDate = endDate,
						StartDate = startDate,
						EnterQuantity = item.VorodQT,
						ExitQuantity = item.KhorojQT,
						DateM = item.DateM,
						Description = GetDescription(startDate, endDate, aggregateType),
						EnterSum = ((item.VorodQT > 0m) ? item.RowSumForDiplay.GetValueOrDefault() : 0m),
						ExitSum = ((item.KhorojQT > 0m) ? item.RowSumForDiplay.GetValueOrDefault() : 0m),
						EnterDiscount = ((item.VorodQT > 0m) ? item.Discount : 0m),
						ExitDiscount = ((item.KhorojQT > 0m) ? item.Discount : 0m),
						VisitorSum = item.VisitorPrice
					});
				}
				else
				{
					productSumBusiness.EnterQuantity += item.VorodQT;
					productSumBusiness.ExitQuantity += item.KhorojQT;
					productSumBusiness.EnterSum += ((item.VorodQT > 0m) ? item.RowSumForDiplay.GetValueOrDefault() : 0m);
					productSumBusiness.ExitSum += ((item.KhorojQT > 0m) ? item.RowSumForDiplay.GetValueOrDefault() : 0m);
					productSumBusiness.EnterDiscount += ((item.VorodQT > 0m) ? item.Discount : 0m);
					productSumBusiness.ExitDiscount += ((item.KhorojQT > 0m) ? item.Discount : 0m);
					productSumBusiness.VisitorSum += item.VisitorPrice;
				}
			}
			return list?.OrderBy((ProductSumBusiness q) => q.StartDate)?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GenerateSumAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\ProductSumBusiness.cs", 118);
			throw;
		}
	}

	private static DateTime? GetStartDate(DateTime? dateM, EnAggregateReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return dateM;
		}
		return tajmieeType switch
		{
			EnAggregateReportType.Dailly => new DateTime(dateM.Value.Year, dateM.Value.Month, dateM.Value.Day, 0, 0, 0), 
			EnAggregateReportType.Weeklly => dateM.Value.GetFirstDayOfWeek(), 
			EnAggregateReportType.Monthlly => dateM.Value.StartDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static DateTime? GetEndDate(DateTime? dateM, EnAggregateReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return dateM;
		}
		return tajmieeType switch
		{
			EnAggregateReportType.Dailly => new DateTime(dateM.Value.Year, dateM.Value.Month, dateM.Value.Day, 23, 59, 59), 
			EnAggregateReportType.Weeklly => dateM.Value.GetEndDayOfWeek(), 
			EnAggregateReportType.Monthlly => dateM.Value.EndDayOfPersianMonth(), 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDateRange(DateTime? dateM, DateTime? startDate, DateTime? endDate, EnAggregateReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return "";
		}
		return tajmieeType switch
		{
			EnAggregateReportType.Dailly => dateM.MiladiToCalture(), 
			EnAggregateReportType.Weeklly => "هفته".GetTranslate() + $"({dateM.Value.GetWeekNumber()}){Calendar.GetYearOfDateSh(dateM.MiladiToCalture())} (" + "از".GetTranslate() + " " + startDate.MiladiToCalture() + " الی".GetTranslate() + " " + endDate.MiladiToCalture() + ")", 
			EnAggregateReportType.Monthlly => $"{Calendar.GetMonthName(dateM.MiladiToCalture())} {Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			_ => throw new NotImplementedException(), 
		};
	}

	private static string GetDescription(DateTime? dateM, DateTime? endDate, EnAggregateReportType tajmieeType)
	{
		if (!dateM.HasValue)
		{
			return "";
		}
		return tajmieeType switch
		{
			EnAggregateReportType.Dailly => "بابت فاکتور روز".GetTranslate() + " " + dateM.MiladiToCalture(), 
			EnAggregateReportType.Weeklly => "بابت فاکتور هفته".GetTranslate() + $" ({dateM.Value.GetWeekNumber()}) {Calendar.GetYearOfDateSh(dateM.MiladiToCalture())}", 
			EnAggregateReportType.Monthlly => "بابت فاکتور ".GetTranslate() + " " + Calendar.GetMonthName(dateM.Value) + " " + "ماه".GetTranslate(), 
			_ => throw new NotImplementedException(), 
		};
	}
}
