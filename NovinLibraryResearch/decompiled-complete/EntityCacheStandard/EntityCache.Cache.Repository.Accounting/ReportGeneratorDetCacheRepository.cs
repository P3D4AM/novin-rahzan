using System;
using EntityCache.Business.Accounting;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting;
using PacketParser.Entities;

namespace EntityCache.Cache.Repository.Accounting;

public class ReportGeneratorDetCacheRepository : MasterCacheRepository<ReportGeneratorDetBusiness>, IReportGeneratorDetRepository, IPersistenceMasterRepository<ReportGeneratorDetBusiness>, IDisposable
{
	private IReportGeneratorDetRepository dbRepository;

	public ReportGeneratorDetCacheRepository(IReportGeneratorDetRepository dbRepository, int cachingTime)
		: base((IPersistenceMasterRepository<ReportGeneratorDetBusiness>)dbRepository, cachingTime)
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		this.dbRepository = dbRepository;
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\ReportGeneratorDetCacheRepository.cs", 31);
		}
	}
}
