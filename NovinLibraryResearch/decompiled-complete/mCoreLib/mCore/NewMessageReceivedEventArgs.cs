using System;

namespace mCore;

public class NewMessageReceivedEventArgs : EventArgs, INewMessageEventArgs
{
	private string a;

	private string b;

	private string c;

	private DateTime d;

	private string e;

	private int f;

	private int g;

	private int h;

	private int i;

	public string Phone => a;

	public string TextMessage => b;

	public string SMSC => c;

	public DateTime TimeStamp => d;

	public string TimeStampRFC => e;

	public int TimeZone => f;

	public int TotalParts => g;

	public int ReferenceNumber => h;

	public int SequenceNumber => i;

	internal NewMessageReceivedEventArgs(string A_0, string A_1, string A_2, DateTime A_3, string A_4, int A_5, int A_6, int A_7, int A_8)
	{
		a = "";
		b = "";
		c = "";
		d = default(DateTime);
		e = "";
		f = 0;
		a = A_0;
		b = A_1;
		c = A_2;
		d = A_3;
		e = A_4;
		f = A_5;
		g = A_6;
		h = A_7;
		i = A_8;
	}
}
