using System.Threading;

namespace PacketParser.Mapping;

public interface IItemMapper<TSource, TDestination> where TSource : class where TDestination : class
{
	TDestination Map(string contextGuid, TSource source, TDestination destination = null, string tranName = "", CancellationToken token = default(CancellationToken));
}
