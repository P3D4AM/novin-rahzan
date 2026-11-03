using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface IBackupLogRepository : INoGuidRepository<BackupLogBusiness>, IDisposable
{
	Task<BackupLogBusiness> GetAsync(Guid fileId, long crc, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> RemoveAllAsync(int tryCount, CancellationToken token, string tranName);
}
