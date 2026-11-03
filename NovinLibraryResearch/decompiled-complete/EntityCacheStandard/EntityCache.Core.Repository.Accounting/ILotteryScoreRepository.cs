using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EntityCache.Business.Accounting;
using PacketParser.Entities;

namespace EntityCache.Core.Repository.Accounting;

public interface ILotteryScoreRepository : IRepository<LotteryScoreBusiness>, INoGuidRepository<LotteryScoreBusiness>, IDisposable
{
	Task<IEnumerable<LotteryScoreBusiness>> GetAllAsync(DateTime date1, DateTime date2, LotteryStatus status, Guid cusGuid, CancellationToken token, string tranName);

	Task<List<Guid>> StartLottery(int count, CancellationToken token, string tranName);
}
