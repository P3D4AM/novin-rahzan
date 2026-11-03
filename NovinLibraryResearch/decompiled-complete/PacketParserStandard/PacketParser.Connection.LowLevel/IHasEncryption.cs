using System;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace PacketParser.Connection.LowLevel;

public interface IHasEncryption
{
	IConnection Connection { get; }

	event Func<object, PublicKeyEventArgs, Task> PublicKeyReceived;

	event Func<object, System.EventArgs, Task> Secured;

	event Func<object, System.EventArgs, Task> Reset;

	Task<ReturnedSaveFuncInfo> Send(EnServerMessageInstruction instruction, string data, short TryCount = 2);
}
