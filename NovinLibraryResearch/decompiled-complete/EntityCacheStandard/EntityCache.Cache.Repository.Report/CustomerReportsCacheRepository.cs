using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Customers;
using EntityCache.Core.Repository.Report;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Report;

public class CustomerReportsCacheRepository : ICustomerReportsRepository, IDisposable
{
	private bool _isDisposed;

	private ICustomerReportsRepository _dbRepository;

	public bool IsDisposed => _isDisposed;

	public CustomerReportsCacheRepository(ICustomerReportsRepository dbRepository, int CachingTimeSeconds)
	{
		_dbRepository = dbRepository;
	}

	public void Dispose()
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				_dbRepository?.Dispose();
				_dbRepository = null;
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Report\\CustomerReportsCacheRepository.cs", 30);
		}
	}

	public Task<List<CustomerVisitPriodModel>> GetAllCustomerVisitPriodAsync(CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllCustomerVisitPriodAsync(token, tranName);
	}

	public Task<IEnumerable<OrderReportBusiness>> GetAllLastTransactionAsync(Document factorType, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllLastTransactionAsync(factorType, token, tranName);
	}

	public Task<IEnumerable<RasBedahiBusiness>> GetAllRasBedahi(string contextGuid, DateTime Date1, DateTime Date2, short darsad, CancellationToken token, string tranName = "")
	{
		return _dbRepository.GetAllRasBedahi(contextGuid, Date1, Date2, darsad, token, tranName);
	}

	public Task<List<TafsilCurrencyGardeshModel>> GetAllTafsilCurrencyAsync(Guid tafsilGuid, DateTime? date1, DateTime? date2, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAllTafsilCurrencyAsync(tafsilGuid, date1, date2, token, tranName);
	}
}
