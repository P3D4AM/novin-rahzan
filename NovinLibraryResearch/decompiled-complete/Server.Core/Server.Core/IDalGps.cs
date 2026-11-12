using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;

namespace Server.Core;

public interface IDalGps
{
	Task<ReturnedSaveFuncInfoWithValue<List<SaveResponse>>> SaveAsync(List<WebGPS> lst, int depUserId, short depId, long hashId, long ime, Guid UserAccGuid, Guid userDepGuid, short tryCount = 10);
}
