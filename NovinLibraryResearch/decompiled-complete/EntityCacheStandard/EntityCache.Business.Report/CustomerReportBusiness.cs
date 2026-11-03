using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using EntityCache.Business.Customers;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;
using PacketParser.Filters;

namespace EntityCache.Business.Report;

public class CustomerReportBusiness
{
	public static async Task<IEnumerable<OrderReportBusiness>> GetAllLastTransactionAsync(string contextGuid, Document factorType, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			IEnumerable<OrderReportBusiness> enumerable = await ClsCache.GetCache(contextGuid).CacheRepository.CustomerReports.GetAllLastTransactionAsync(factorType, token, tranName);
			if (enumerable == null || !enumerable.Any())
			{
				return null;
			}
			return await ApplyAccessForLastTransactionAsync(contextGuid, enumerable, token, tranName);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetAllLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\CustomerReportBusiness.cs", 27);
			throw;
		}
	}

	public static Task<IEnumerable<RasBedahiBusiness>> GetAllRasBedahi(string contextGuid, DateTime Date1, DateTime Date2, short darsad, CancellationToken token, string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CustomerReports.GetAllRasBedahi(contextGuid, Date1, Date2, darsad, token, tranName);
	}

	public static Task<List<TafsilCurrencyGardeshModel>> GetAllTafsilCurrencyAsync(string contextGuid, Guid tafsilGuid, DateTime? date1 = null, DateTime? date2 = null, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		return ClsCache.GetCache(contextGuid).CacheRepository.CustomerReports.GetAllTafsilCurrencyAsync(tafsilGuid, date1, date2, token, tranName);
	}

	public static async Task<List<CustomerVisitPriodModel>> GetAllCustomerVisitPriodAsync(string contextGuid, CancellationToken token = default(CancellationToken), string tranName = "")
	{
		_ = 1;
		try
		{
			List<CustomerVisitPriodModel> list = await ClsCache.GetCache(contextGuid).CacheRepository.CustomerReports.GetAllCustomerVisitPriodAsync(token, tranName);
			if (!(list?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<CusCompactBusiness> customers = await CusCompactBusiness.GetAllAsync(contextGuid, EnSystem.UserInterface, token, null, null, true, tranName);
			if (!(customers?.Any() ?? false))
			{
				return null;
			}
			return list.Where((CustomerVisitPriodModel q) => customers.Select((CusCompactBusiness w) => w.Guid).Contains(q.TafsilGuid))?.ToList();
		}
		catch (OperationCanceledException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "GetAllCustomerVisitPriodAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\CustomerReportBusiness.cs", 49);
			throw;
		}
	}

	private static async Task<IEnumerable<OrderReportBusiness>> ApplyAccessForLastTransactionAsync(string contextGuid, IEnumerable<OrderReportBusiness> orders, CancellationToken token, string tranName)
	{
		try
		{
			if (!(orders?.Any() ?? false))
			{
				return null;
			}
			IEnumerable<TafsilBusiness> tafsilList = await TafsilBusiness.GetAllAsync(contextGuid, EnSystem.UserInterface, new TafsilFilter
			{
				Status = true
			}, token, tranName, "ApplyAccessForLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\CustomerReportBusiness.cs");
			if (!(tafsilList?.Any() ?? false))
			{
				return null;
			}
			orders = orders?.Where((OrderReportBusiness p) => (p?.TafsilGuid ?? Guid.Empty) != Guid.Empty && tafsilList.Select((TafsilBusiness q) => q.Guid).Contains(p.TafsilGuid));
			return orders;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessForLastTransactionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Report\\CustomerReportBusiness.cs", 65);
			throw;
		}
	}
}
