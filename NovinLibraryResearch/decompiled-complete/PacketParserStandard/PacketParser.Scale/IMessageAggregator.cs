namespace PacketParser.Scale;

public interface IMessageAggregator : IScaleResolver, IScaleEventReceiver
{
	void Reset();
}
