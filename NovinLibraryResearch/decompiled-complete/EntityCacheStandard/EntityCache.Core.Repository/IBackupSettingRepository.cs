using System;
using EntityCache.Business;

namespace EntityCache.Core.Repository;

public interface IBackupSettingRepository : INoGuidRepository<BackupSettingsBusiness>, IDisposable
{
}
