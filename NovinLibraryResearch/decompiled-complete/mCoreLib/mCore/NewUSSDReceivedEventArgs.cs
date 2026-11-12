using System;

namespace mCore;

public class NewUSSDReceivedEventArgs : EventArgs, INewUSSDEventArgs
{
	private string a;

	private USSDStatus b;

	public string Message => a;

	public USSDStatus Status => b;

	internal NewUSSDReceivedEventArgs(string A_0, USSDStatus A_1)
	{
		a = "";
		a = A_0;
		b = A_1;
	}
}
