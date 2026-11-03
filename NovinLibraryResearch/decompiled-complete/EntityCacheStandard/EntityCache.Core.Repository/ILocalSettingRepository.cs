using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface ILocalSettingRepository : INoGuidRepository<LocalSettingBusiness>, IDisposable
{
	Task<ReturnedSaveFuncInfo> RemoveAsync(string name, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(string name, string value, short tryCount, CancellationToken token, string tranName);

	Task<string> GetAsync(string name, CancellationToken token, string tranName);

	string Get(string name, CancellationToken token, string tranName);
}
