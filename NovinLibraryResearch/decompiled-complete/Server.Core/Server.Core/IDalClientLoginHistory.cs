using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalClientLoginHistory
{
	Task<ReturnedSaveFuncInfo> InsertLogAsync(ClientLoginHistory logObj, short tryCount = 10);

	Task<List<ClientLoginHistory>> GettAllAsync(CancellationToken? token = null, long? top = null, long? hashId = null, DateTime? startTime = null);

	void InsertDisconnectTime(string ConnectionUniqueId, DateTime DisConnectTime, string reason);
}
