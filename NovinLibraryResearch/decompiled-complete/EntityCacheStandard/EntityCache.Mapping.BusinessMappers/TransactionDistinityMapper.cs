using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Mapping;

namespace EntityCache.Mapping.BusinessMappers;

public class TransactionDistinityMapper : IItemAsyncMapper<TaskCompletionSource<ReturnedSaveFuncInfo>, TaskCompletionSource<ReturnedSaveFuncInfo>>
{
	public static TransactionDistinityMapper Instance { get; } = new TransactionDistinityMapper();


	public Task<TaskCompletionSource<ReturnedSaveFuncInfo>> MapAsync(string contextGuid, TaskCompletionSource<ReturnedSaveFuncInfo> source, TaskCompletionSource<ReturnedSaveFuncInfo> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return Task.FromResult(source);
	}

	public Task<List<TaskCompletionSource<ReturnedSaveFuncInfo>>> MapListAsync(string contextGuid, IEnumerable<TaskCompletionSource<ReturnedSaveFuncInfo>> source, List<TaskCompletionSource<ReturnedSaveFuncInfo>> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return Task.FromResult(source.ToList());
	}
}
