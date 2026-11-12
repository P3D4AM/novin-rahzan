using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Message;

namespace Server.Core;

public interface IDalMessage
{
	Task<ReturnedSaveFuncInfoWithValue<long>> InsertAsync(WebMessage webMessage, short depID);

	Task<ReturnedSaveFuncInfoWithValue<bool>> UpdateAsync(long id, short depId, enMessageSender en);

	Task<List<WebMessage>> GetAllAsync(enMessageSender enWeb, CancellationToken cancellationToken, EnTelegramType encontactType);

	Task<List<WebMessage>> GetAllAsync(short depId, enMessageSender enWeb, CancellationToken cancellationToken, EnTelegramType encontactType);

	Task<List<WebMessage>> GetAllAsync(long sender, short depID, enMessageSender enWeb, CancellationToken cancellationToken, EnTelegramType encontactType);

	Task<WebMessage> GetNextMessageAsync(short depID, long senderNumber, CancellationToken cancellationToken, EnTelegramType encontactType);
}
