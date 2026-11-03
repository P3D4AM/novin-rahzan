using System;
using System.Threading.Tasks;
using PacketParser.EventArgs;
using PacketParser.Scale.EventArguments;

namespace PacketParser.Scale;

public interface IScalePacketParser
{
	IScaleConnection Connection { get; }

	event Func<object, FactorEventArgs, Task> NewFactor;

	event Func<object, PacketEventArgs, Task> NewMessage;

	event Func<object, WeightEventArgs, Task> NewWeight;

	event Func<object, ShowMessageEventArgs, Task> MessageForUser;
}
