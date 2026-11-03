using System.Collections.Generic;
using PacketParser.Entities;
using PacketParser.Issabel.Interface;

namespace PacketParser.Issabel;

public class IsabelleCallCenterBase : IIsabelleCallCenter
{
	public string Ip { get; set; }

	public int Port { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string FriendllyName { get; set; } = "مرکز تماس ایزابل";


	public List<IsabelleQueue> Queues { get; set; }

	public string LineReleaseCode { get; set; }

	public EnConnectionState CallCenterStatus { get; set; } = EnConnectionState.Disconnected;

}
