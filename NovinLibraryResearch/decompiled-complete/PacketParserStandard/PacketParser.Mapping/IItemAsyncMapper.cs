using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PacketParser.Mapping;

public interface IItemAsyncMapper<TSource, TDestination> where TSource : class where TDestination : class
{
	Task<TDestination> MapAsync(string contextGuid, TSource source, TDestination destination = null, string tranName = "", CancellationToken token = default(CancellationToken));

	Task<List<TDestination>> MapListAsync(string contextGuid, IEnumerable<TSource> source, List<TDestination> destination = null, string tranName = "", CancellationToken token = default(CancellationToken));
}
