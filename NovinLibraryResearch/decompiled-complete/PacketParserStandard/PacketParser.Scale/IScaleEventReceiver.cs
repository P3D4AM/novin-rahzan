using System.Threading.Tasks;
using PacketParser.Scale.Entities;
using PacketParser.Scale.EventArguments;
using Serilog.Events;

namespace PacketParser.Scale;

public interface IScaleEventReceiver
{
	Task Handled(CacheEventArgs args);

	Task Unhandled(CacheEventArgs args);

	Task<bool> NewFactor(FactorModel factor, MicroScaleInfo info);

	Task<bool> NewWeight(WeightEventArgs e);

	Task ShowMessageForUser(LogEventLevel level, string title, string message);
}
