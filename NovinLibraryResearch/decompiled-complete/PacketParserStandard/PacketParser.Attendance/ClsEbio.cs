using System;
using PacketParser.Assistance.Net;

namespace PacketParser.Attendance;

public class ClsEbio
{
	public static ClsEbio Instance { get; }

	static ClsEbio()
	{
		Instance = new ClsEbio();
	}

	public void Ebio_OnLogTime(object sender, TimeLogEventLog e)
	{
		throw new NotImplementedException();
	}
}
