using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository.Accounting;

public interface ICashierRepository : ISyncRepository<CashierBusiness>, IEditableDelivery, IRepository<CashierBusiness>, INoGuidRepository<CashierBusiness>, IDisposable
{
	Task<IEnumerable<CashierBusiness>> GetAllAsync(Guid userGuid, bool? status, CancellationToken token, string tranName);

	Task<bool> CheckUserNameAsync(string uName, Guid _guid, CancellationToken token, string tranName);

	Task<CashierBusiness> GetByUsernameAsync(string username, CancellationToken token, string tranName);

	Task<CashierBusiness> GetAsync(string uName, CancellationToken token, string tranName);

	Task<CashierBusiness> GetByNameAsync(string name, CancellationToken token, string tranName);

	void ResetCache(CancellationToken token, string tranName);
}
