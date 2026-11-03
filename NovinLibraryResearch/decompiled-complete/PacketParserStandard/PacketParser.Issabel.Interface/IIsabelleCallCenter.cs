using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Issabel.Interface;

public interface IIsabelleCallCenter
{
	string Ip { get; set; }

	int Port { get; set; }

	string UserName { get; set; }

	string Password { get; set; }

	string FriendllyName { get; set; }

	List<IsabelleQueue> Queues { get; set; }

	string LineReleaseCode { get; set; }

	EnConnectionState CallCenterStatus { get; set; }
}
