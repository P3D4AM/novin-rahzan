using System;

namespace mCore;

public class NewIncomingCallEventArgs : EventArgs, IIncomingCallEventArgs
{
	private string a;

	public string CallerID => a;

	internal NewIncomingCallEventArgs(string A_0)
	{
		a = "";
		a = A_0;
	}
}
