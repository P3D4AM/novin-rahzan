using System;
using System.Net;

namespace Riss.Devices;

public class ReceiveEventArg : EventArgs
{
	public Record record;

	public IPEndPoint endPoint;

	public byte[] sn;
}
