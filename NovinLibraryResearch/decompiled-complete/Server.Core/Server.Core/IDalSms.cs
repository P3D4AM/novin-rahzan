using System.Threading;
using System.Threading.Tasks;

namespace Server.Core;

public interface IDalSms
{
	Task SendSmsAsync(string text, string mobile, string userPanel, string passwordPanel, string domainPanel, string senderPanel, CancellationToken token);
}
