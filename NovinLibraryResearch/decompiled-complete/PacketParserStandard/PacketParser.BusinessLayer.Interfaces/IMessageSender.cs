using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Interfaces;

public interface IMessageSender
{
	Task SendSmsAsync(PayamakSendStatus sellAccountStatus, int account_Id, string message, string mobiles, CancellationToken token = default(CancellationToken));
}
