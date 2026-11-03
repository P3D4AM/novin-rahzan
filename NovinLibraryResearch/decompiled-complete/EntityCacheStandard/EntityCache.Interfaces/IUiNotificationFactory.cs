using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace EntityCache.Interfaces;

public interface IUiNotificationFactory
{
	Task DisplayAsync(string title, string message, ReturnedState status, CancellationToken token, int duration = 30000);

	Task CancelNotif(int smsId);
}
