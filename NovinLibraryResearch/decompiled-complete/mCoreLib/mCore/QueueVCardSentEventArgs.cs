using System;

namespace mCore;

public class QueueVCardSentEventArgs : EventArgs, IQueueVCardSentEventArgs
{
	private string a;

	private bool b;

	private string c;

	private long d;

	private string e;

	private string f;

	public string DestinationNumber => f;

	public long ErrorCode => d;

	public string ErrorDescription => e;

	public string MessageReference => c;

	public string QueueMessageKey => a;

	public bool SendResult => b;

	internal QueueVCardSentEventArgs(string A_0, string A_1, bool A_2, string A_3, long A_4, string A_5)
	{
		a = "";
		b = false;
		c = "";
		e = "";
		f = "";
		a = A_0;
		f = A_1;
		b = A_2;
		d = A_4;
		e = A_5;
		c = A_3;
	}
}
