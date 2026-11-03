using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Core.Repository;

public interface ICustomerRepository : IDetailRepository<CusBusiness>, IEditableDelivery, IDisposable
{
	Task<CusBusiness> GetByNameAsync(string firstName, string lastName, Guid guid, CancellationToken token, string tranName);

	Task<bool> CheckUserNameAsync(Guid guid, string userName, CancellationToken token, string tranName);

	Task<CusBusiness> GetByEmailAsync(string email, CancellationToken token, string tranName);

	Task<CusBusiness> GetAsync(long code, CancellationToken token, string tranName);

	Task<CusBusiness> GetByNationalCodeAsync(string nationalCode, CancellationToken token, string tranName);

	Task<CusBusiness> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName);

	Task<CusBusiness> GetByUserNameAsync(string userName, string pass, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateGroupAsync(IEnumerable<Guid> customerGuids, Guid groupGuid, CancellationToken token, string tranName);

	void ResetCache(CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeChequeBargashtiValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> ChangeOrderTimeValidationAsync(List<Guid> lst, ReturnedState state, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSendToServerDateAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(EnPriceType priceType, Guid roleGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateServerStatusAsync(Guid guid, ServerStatusForClient serverStatus, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> SaveRangeAsync(List<WebCus> items, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateRoleGuidAsync(List<Guid> customerList, Guid roleGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdatePhoneNumbersAsync(CancellationToken token, string tranName);

	Task<List<Guid>> GetCustomerGuidListInRoleAsync(Guid roleGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfo> UpdateSpecialAsync(Guid groupGuid, bool value, CancellationToken token, string tranName);
}
