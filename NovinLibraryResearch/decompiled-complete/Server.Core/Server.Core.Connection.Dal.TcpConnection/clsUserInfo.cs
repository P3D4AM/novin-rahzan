using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;

namespace Server.Core.Connection.Dal.TcpConnection;

public class clsUserInfo : IDalUserInFormation
{
	private class NestedUserInfo
	{
		internal static readonly clsUserInfo Instance;

		static NestedUserInfo()
		{
			Instance = new clsUserInfo();
		}
	}

	public static clsUserInfo Instance => NestedUserInfo.Instance;

	public Task<WebUserInformation> LoadDataAsync(string Username, string Password, Guid DbGuid, long LkSerial, string CpuSerial, long ime, Guid cashierGuid, string clientContextGuid, CancellationToken token)
	{
		return Task.FromResult(GetDefaultData());
	}

	public Task<WebUserInformation> LoadDataAsync(string UserName, string Password, string CustomerUserName, string CustomerPassword, long LkSerial, string CpuSerial, long ime, Guid cashierGuid, string clientContextGuid, CancellationToken token)
	{
		return Task.FromResult(GetDefaultData());
	}

	private WebUserInformation GetDefaultData()
	{
		return new WebUserInformation
		{
			UserId = 5000,
			DepId = 5000
		};
	}
}
