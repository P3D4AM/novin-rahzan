using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using EntityCache.Business.Customers;
using PacketParser.BusinessLayer.Models;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Report;

public interface ICustomerReportsRepository : IDisposable
{
	Task<IEnumerable<OrderReportBusiness>> GetAllLastTransactionAsync(Document factorType, CancellationToken token, string tranName);

	Task<IEnumerable<RasBedahiBusiness>> GetAllRasBedahi(string contextGuid, DateTime Date1, DateTime Date2, short darsad, CancellationToken token, string tranName = "");

	Task<List<TafsilCurrencyGardeshModel>> GetAllTafsilCurrencyAsync(Guid tafsilGuid, DateTime? date1, DateTime? date2, CancellationToken token, string tranName);

	Task<List<CustomerVisitPriodModel>> GetAllCustomerVisitPriodAsync(CancellationToken token, string tranName);
}
