using System.Collections.Concurrent;

namespace PacketParser.Scale;

public static class MessageAggregatorInstance
{
	private static ConcurrentDictionary<int, IMessageAggregator> _dic = new ConcurrentDictionary<int, IMessageAggregator>();

	public static void SetMessageAggregatorInstance(int scaleNumber, IMessageAggregator messageAggregator)
	{
		_dic.TryRemove(scaleNumber, out var _);
		_dic.TryAdd(scaleNumber, messageAggregator);
	}

	public static IMessageAggregator GetInstance(int scaleNumber)
	{
		_dic.TryGetValue(scaleNumber, out var value);
		return value;
	}
}
