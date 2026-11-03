using System;

namespace PacketParser.Entities.Interfaces;

public interface IClientLoginHistory
{
	DateTime? LoginDateM { get; set; }

	string LoginDateSh { get; }

	DateTime? DisconnectDateM { get; set; }

	string DisconnectDateSh { get; }

	string DisconnectDescription { get; set; }

	string Ip { get; set; }

	long HashId { get; set; }

	string Username { get; set; }

	string Pass { get; set; }

	short DepId { get; set; }

	string CusUserName { get; set; }

	string CusPass { get; set; }

	Guid CusGuid { get; set; }

	ENSource Source { get; set; }

	string Version { get; set; }

	string ClientVersion { get; set; }

	string CpuSerial { get; set; }

	long AndroidIme { get; set; }

	Guid DbGuid { get; set; }

	string ConnectionUniqueId { get; set; }
}
