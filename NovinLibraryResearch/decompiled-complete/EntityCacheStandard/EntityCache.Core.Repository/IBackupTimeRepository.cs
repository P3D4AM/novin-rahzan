using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IBackupTimeRepository : IRepository<BackupTimeBusiness>, INoGuidRepository<BackupTimeBusiness>, IDisposable
{
}
