using System;
using EntityCache.Business.Scale;

namespace EntityCache.Core.Repository.Scale;

public interface IScale_SettingRepository : IRepository<Scale_SettingBusiness>, INoGuidRepository<Scale_SettingBusiness>, IDisposable
{
}
