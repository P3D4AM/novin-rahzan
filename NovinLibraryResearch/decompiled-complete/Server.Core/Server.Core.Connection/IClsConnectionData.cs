using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Settings.Accounting;
using PacketParser.TransferLayer.Interfaces;

namespace Server.Core.Connection;

public interface IClsConnectionData : IConnectionData, IDisposable
{
	bool IsAccountingFamily { get; }

	bool IsAttendanceFamily { get; }

	string DepName { get; set; }

	bool IsDisposed { get; }

	string Username { get; set; }

	CancellationToken cancellationToken { get; }

	short DepId { get; set; }

	long HashId { get; set; }

	long LkSerial { get; set; }

	ENSource Source { get; set; }

	Guid UserAccGuid { get; set; }

	Guid CustomerTafsilGuid { get; set; }

	short ClientVersion { get; set; }

	Task<ICus> GetCustomerInfoAsync();

	Task<WebDepartment> GetDepartmentAsync();

	Task<IHesabTree> GetHesabInfoAsync();

	Task<AccGlobalSettings> GetSettingsAsync();

	Task<SyncInformation> GetSyncInfoAsync(CancellationToken token = default(CancellationToken));

	Task<WebUser> GetUserAsync();

	Task<WebCashier> GetCashierInfoAsync(CancellationToken token = default(CancellationToken));

	Task RecievePacketAndParse(Packet packet, WebContextHolder context, string ConnectionUniqueId, string ip, StringBuilder sb);

	Task SendPacketAsync(List<Packet> packs, Priority priority, int partitiion, CancellationToken token);

	Task SendPacketAsync(Packet packet, Priority priority, int partitiion, CancellationToken token);

	CancellationToken GetSaveSyncToken();

	Task<WebUserAcc> GetUserAccInfoAsync(CancellationToken token = default(CancellationToken));
}
