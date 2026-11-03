using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Accounting;

public interface ILotteryRepository : IRepository<LotteryBusiness>, INoGuidRepository<LotteryBusiness>, IDisposable
{
	Task<IEnumerable<LotteryBusiness>> GetAllAsync(LotteryStatus status, DateTime date1, DateTime date2, Guid cusGuid, CancellationToken token, string tranName);
}
