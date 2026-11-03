using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Crm;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Crm;
using EntityCache.Mapping.BusinessMappers.Crm.Calling;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Crm;

public class CallingLineCacheRepository : CacheRepository<CallingLineBusiness>, ICallingLineRepository, IRepository<CallingLineBusiness>, INoGuidRepository<CallingLineBusiness>, IDisposable
{
	private ICallingLineRepository dbRepository;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<Guid, CallingLineBusiness> TimingCache { get; set; }

	public override IItemAsyncMapper<CallingLineBusiness, CallingLineBusiness> Mapper => CallingLineMapper.Instance;

	public CallingLineCacheRepository(ICallingLineRepository dbRepository, int CachingTime)
		: base((IRepository<CallingLineBusiness>)dbRepository, CachingTime, cachingEnable: true)
	{
		this.dbRepository = dbRepository;
		TimingCache = new TimingCache<Guid, CallingLineBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public Task<CallingLineBusiness> GetAsync(string ComputerName, short DeviceNumber, short LineNumber, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(ComputerName, DeviceNumber, LineNumber, token, tranName);
	}

	public Task<CallingLineBusiness> GetAsync(string number, CancellationToken token, string tranName)
	{
		return dbRepository.GetAsync(number, token, tranName);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Crm\\CallingLineCacheRepository.cs", 43);
		}
	}
}
