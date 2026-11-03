using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.WordPress;

namespace PacketParser.Connection.LowLevel.Accounting.Entities.Customer;

public interface ICustomerRoleSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	Task<ReturnedSaveFuncInfoWithValue<List<WebCustomerRole>>> GetAllAsync(bool? status, CancellationToken token, string tranName);

	Task<ReturnedSaveFuncInfoWithValue<WebCustomerRole>> GetAsync(Guid guid, CancellationToken token, string tranName);
}
