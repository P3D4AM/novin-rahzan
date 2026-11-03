using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Filters;

namespace EntityCache.Core.Repository.Accounting;

public interface IReportGeneratorRepository : IRepository<ReportGeneratorBusiness>, INoGuidRepository<ReportGeneratorBusiness>, IDisposable
{
	Task<List<ReportGeneratorBusiness>> GetAllAsync(ReportGeneratorFilter filter, CancellationToken token, string tranName);
}
