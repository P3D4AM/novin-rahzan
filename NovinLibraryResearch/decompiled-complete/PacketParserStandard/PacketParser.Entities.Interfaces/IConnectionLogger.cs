using System.Collections.Generic;

namespace PacketParser.Entities.Interfaces;

public interface IConnectionLogger
{
	long HashId { get; set; }

	List<ConnectionLoggerDets> LogReceivedDets { get; }

	List<ConnectionLoggerDets> LogSendDets { get; }
}
