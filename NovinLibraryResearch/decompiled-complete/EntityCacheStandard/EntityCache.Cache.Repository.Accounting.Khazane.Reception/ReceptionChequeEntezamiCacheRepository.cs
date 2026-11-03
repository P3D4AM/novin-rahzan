using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting.Khazane.Reception;
using EntityCache.Core;
using EntityCache.Core.Repository.Accounting.Khazane.Reception;
using EntityCache.Mapping.BusinessMappers.Accounting.Khazane.Reception;
using PacketParser.Caching;
using PacketParser.Entities;
using PacketParser.Mapping;

namespace EntityCache.Cache.Repository.Accounting.Khazane.Reception;

public class ReceptionChequeEntezamiCacheRepository : CacheRepositoryNoGuiId<ReceptionChequeEntezamiBusiness, string>, IReceptionChequeEntezamiRepository, INoGuidRepository<ReceptionChequeEntezamiBusiness>, IDisposable
{
	private IReceptionChequeEntezamiRepository _dbRepository;

	public override IItemAsyncMapper<ReceptionChequeEntezamiBusiness, ReceptionChequeEntezamiBusiness> Mapper => ReceptionChequeEntezamiMapper.Instance;

	public override bool HasStatus => true;

	public override bool HasModified => true;

	protected override TimingCache<string, ReceptionChequeEntezamiBusiness> TimingCache { get; set; }

	public ReceptionChequeEntezamiCacheRepository(IReceptionChequeEntezamiRepository dbRepository, int CachingTime)
		: base((INoGuidRepository<ReceptionChequeEntezamiBusiness>)dbRepository, CachingTime)
	{
		_dbRepository = dbRepository;
		TimingCache = new TimingCache<string, ReceptionChequeEntezamiBusiness>(base.ContextGuid, Mapper, CachingTime);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Cache\\Repository\\Accounting\\Khazane\\Reception\\ReceptionChequeEntezamiCacheRepository.cs", 37);
		}
	}

	public Task<ReceptionChequeEntezamiBusiness> GetAsync(Guid guid, CancellationToken token, string tranName)
	{
		return _dbRepository.GetAsync(guid, token, tranName);
	}
}
