using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core;

public interface IDalUserInFormation
{
	Task<WebUserInformation> LoadDataAsync(string Username, string Password, Guid DbGuid, long LkSerial, string CpuSerial, long ime, Guid cashierGuid, string clientContextGuid, CancellationToken token);

	Task<WebUserInformation> LoadDataAsync(string UserName, string Password, string CustomerUserName, string CustomerPassword, long LkSerial, string CpuSerial, long ime, Guid cashierGuid, string clientContextGuid, CancellationToken token);
}
