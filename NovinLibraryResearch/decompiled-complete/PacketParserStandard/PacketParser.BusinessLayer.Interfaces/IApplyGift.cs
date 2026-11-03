using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces.Accounting.Gift;

namespace PacketParser.BusinessLayer.Interfaces;

public interface IApplyGift
{
	Task<IEnumerable<IGiftDetail>> SelectGiftAsync(Guid giftGuid, CancellationToken token = default(CancellationToken));
}
