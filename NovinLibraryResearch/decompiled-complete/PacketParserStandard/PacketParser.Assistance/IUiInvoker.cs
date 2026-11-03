using System;
using System.Runtime.CompilerServices;

namespace PacketParser.Assistance;

public interface IUiInvoker
{
	void RunOnUiThread(Action action, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "");

	bool InvokeRequired();
}
