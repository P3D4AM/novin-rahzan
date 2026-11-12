using System;

namespace mCore;

public class NewDeliveryReportEventArgs : EventArgs, IDeliveryReportEventArgs
{
	private int a;

	private string b;

	private bool c;

	private DateTime d;

	private DateTime e;

	private int f;

	private int g;

	public int MessageReference => a;

	public string Phone => b;

	public bool Status => c;

	public DateTime DeliveryTimeStamp => d;

	public int DeliveryTimeZone => f;

	public DateTime SentTimeStamp => e;

	public int SentTimeZone => g;

	internal NewDeliveryReportEventArgs(int A_0, string A_1, bool A_2, DateTime A_3, int A_4, DateTime A_5, int A_6)
	{
		b = "";
		c = false;
		a = A_0;
		b = A_1;
		c = A_2;
		d = A_3;
		f = A_4;
		e = A_5;
		g = A_6;
	}
}
