using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Interfaces.Sync;

public interface ISendData2Server
{
	SyncStatus Status { set; }

	ReturnedSaveFuncInfo FinalStatus { set; }

	string ShortMessage { set; }

	int TotalCount { set; }

	int Count { set; }

	SyncPart Group { set; }

	void Reset();
}
