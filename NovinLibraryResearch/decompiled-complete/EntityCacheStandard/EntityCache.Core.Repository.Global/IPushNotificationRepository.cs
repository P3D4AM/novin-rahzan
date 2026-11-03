using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Global;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Global;

public interface IPushNotificationRepository : IRepository<PushNotificationBusiness>, INoGuidRepository<PushNotificationBusiness>, IDisposable
{
	Task<DateTime> GetLastModifiedDate(CancellationToken token, string tranName);

	Task<IEnumerable<PushNotificationBusiness>> GetActiveNotifcation(ENSource source, CancellationToken token, string tranName);
}
