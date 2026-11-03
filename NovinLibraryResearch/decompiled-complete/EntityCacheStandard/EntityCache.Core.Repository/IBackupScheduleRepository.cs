using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IBackupScheduleRepository : IRepository<BackupScheduleBusiness>, INoGuidRepository<BackupScheduleBusiness>, IDisposable
{
}
