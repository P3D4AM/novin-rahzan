using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Pardakht;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Khazane.Pardakht;
using EntityCache.Mapping.BusinessMappers.Accounting.Khazane.Pardakht;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Khazane.Pardakht;

public class PardakhtChequeEntezamiCacheRepository : CacheRepositoryNoGuiId<PardakhtChequeEntezamiBusiness, string>, IPardakhtChequeEntezamiRepository, INoGuidRepository<PardakhtChequeEntezamiBusiness>, IDisposable
{
	private IPardakhtChequeEntezamiRepository _dbRepository;

	public override IItemAsyncMapper<PardakhtChequeEntezamiBusiness, PardakhtChequeEntezamiBusiness> Mapper => PardakhtChequeEntezamiMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, PardakhtChequeEntezamiBusiness> TimingCache { get; set; }

	public PardakhtChequeEntezamiCacheRepository(IPardakhtChequeEntezamiRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<PardakhtChequeEntezamiBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, PardakhtChequeEntezamiBusiness>(base.ContextGuid, Mapper, CachingTime);
	}

	public override void Dispose()
	{
		try
		{
			if (!base.IsDisposed)
			{
				_dbRepository = null;
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Pardakht\\PardakhtChequeEntezamiCacheRepository.cs", 37);
		}
	}

	public Task<PardakhtChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}
}
