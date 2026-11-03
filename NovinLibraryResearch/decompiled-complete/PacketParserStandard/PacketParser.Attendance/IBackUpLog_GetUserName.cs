using System;
using System.Threading.Tasks;

namespace PacketParser.Attendance;

public interface IBackUpLog_GetUserName
{
	Task<string> GetUserNameAsync(string contextGuid, Guid? userGuid, string connectionString);
}
