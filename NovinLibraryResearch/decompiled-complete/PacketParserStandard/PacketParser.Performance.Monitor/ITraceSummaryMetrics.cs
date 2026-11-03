namespace PacketParser.Performance.Monitor;

public interface ITraceSummaryMetrics
{
	void Observe(double value);
}
