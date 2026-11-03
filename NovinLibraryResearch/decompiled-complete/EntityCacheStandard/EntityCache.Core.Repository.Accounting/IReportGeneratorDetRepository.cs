using System;
using EntityCache.Business.Accounting;

namespace EntityCache.Core.Repository.Accounting;

public interface IReportGeneratorDetRepository : IPersistenceMasterRepository<ReportGeneratorDetBusiness>, IDisposable
{
}
