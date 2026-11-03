using System;
using PacketParser.BusinessLayer;
using PacketParser.BusinessLayer.Interfaces;

namespace EntityCache.Interfaces.Sync;

public interface IMasterSendData2Server : IReturnedSaveFuncInfo
{
	string Description { get; set; }

	DateTime StartSync { get; set; }

	DateTime? EndSync { get; set; }

	ReturnedSaveFuncInfo Result { get; }

	void ResetResults();
}
