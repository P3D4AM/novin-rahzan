using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PacketParser.TcpIp;

public static class IpUtility
{
	public static string GetHostName()
	{
		return Dns.GetHostName();
	}

	public static string GetIpAddress()
	{
		return Dns.GetHostEntry(GetHostName()).AddressList[0].ToString();
	}

	public static List<string> GetIpAddresss()
	{
		return Dns.GetHostEntry(GetHostName()).AddressList.Select((IPAddress p) => p.ToString()).ToList();
	}
}
