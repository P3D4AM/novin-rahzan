using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities.Sms;

namespace Server.Core.SMS;

public interface ISMS_WhiteList
{
	Task<SMS_WhiteList> GetAsync(string username, string domain, CancellationToken token);

	Task<bool> SaveAsync(SMS_WhiteList sms_white, CancellationToken token);

	Task<List<SMS_WhiteList>> GetAllAsync(CancellationToken token);

	Task<bool> DeleteAsync(SMS_WhiteList sms_white, CancellationToken token);
}
