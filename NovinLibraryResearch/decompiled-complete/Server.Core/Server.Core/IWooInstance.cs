using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using Server.Core.Connection;

namespace Server.Core;

public interface IWooInstance : INpDisposable, IDisposable
{
	ClsConnectionData Connection { get; }

	AggregateDal Dal { get; }

	short DepId { get; }

	string FullUrl { get; }

	string HostName { get; }

	new bool IsDisposed { get; }

	string NovinCommerceVersion { get; set; }

	SemaphoreSlim SemaphoreSlime { get; }

	WebTelegramConfig WebTelegramConfig { get; }

	string WooKey { get; }

	string WooSecret { get; }

	new void Dispose();

	void Dispose(bool disposing);

	void SendWpMessage(string message);

	Task<IWooInstance> GetWooInstance2Async(ClsConnectionData connection, AggregateDal dal);
}
