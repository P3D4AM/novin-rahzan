namespace PacketParser.Scale.EventArguments;

public class DisconnectEventArgs
{
	public string Reason { get; private set; }

	public string Ip { get; private set; }

	public int Port { get; private set; }

	public DisconnectEventArgs(string reason, string ip, int port)
	{
		Reason = reason;
		Ip = ip;
		Port = port;
	}
}
