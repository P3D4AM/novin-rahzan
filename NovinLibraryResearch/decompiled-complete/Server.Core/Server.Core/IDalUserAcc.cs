using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Access;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalUserAcc
{
	Task<List<UserAccessChanges>> SaveAsync(short DepId, long hashId, ENSource source, List<WebUserAcc> lst, CancellationToken token);

	Task<WebUserAcc> GetUserGroupByCashierGuid(short depId, Guid cashierGuid, CancellationToken token);

	Task<WebUserAcc> GetByTokenGuidAsync(short depId, Guid tokenGuid, CancellationToken token);

	Task<WebCashier> GetByCashierAsync(short depId, Guid cashierGuid, CancellationToken token);

	Task<List<WebUserAcc>> GetAllAsync(short DepId, DateTime Modified, CancellationToken cancellationToken);

	[Obsolete("Just for old soap webservices")]
	List<WebUserAcc> GetAll(short DepId, DateTime Modified, CancellationToken cancellationToken);

	Task<List<WebUserAcc>> GetallFromClients_ListForServerAsync(short connectionDepId, CancellationToken connectionCancellationToken);
}
