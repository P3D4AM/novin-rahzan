using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Customer;

public interface ICustomerSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfo> SaveAsync(WebCus Customer, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetAsync(Guid cusGuid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetByNameAsync(string firstName, string lastName, Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCus>> GetFromDatabaseAsync(Guid guid, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<bool>> CheckUserNameAsync(Guid guid, string userName, CancellationToken token, string tranName);
}
