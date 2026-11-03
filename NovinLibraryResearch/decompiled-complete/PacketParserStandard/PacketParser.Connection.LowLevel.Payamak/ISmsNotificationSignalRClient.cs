using System;
using System.Threading.Tasks;
using PacketParser.Entities.Interfaces;
using PacketParser.Payamak.Entities;

namespace PacketParser.Connection.LowLevel.Payamak;

public interface ISmsNotificationSignalRClient : IGenericSignalRClient, INpAsyncDisposable, IAsyncDisposable
{
	event Func<BaseSmsNotification, Task> DataRecieved;
}
