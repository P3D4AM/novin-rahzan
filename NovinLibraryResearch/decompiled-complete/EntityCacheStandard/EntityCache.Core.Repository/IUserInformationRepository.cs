using System;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;

namespace EntityCache.Core.Repository;

public interface IUserInformationRepository : INoGuidRepository<UserInformationBusiness>, IDisposable
{
	Task<UserInformationBusiness> GetAsync(CancellationToken token, string tranName);

	Task<UserInformationBusiness> GetByCredentialAsync(string username, string password, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateAsync(UserInformationBusiness item, string username, string password, short tryCount, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveAsync(UserInformationBusiness item, CancellationToken token, short tryCount, string tranName);
}
