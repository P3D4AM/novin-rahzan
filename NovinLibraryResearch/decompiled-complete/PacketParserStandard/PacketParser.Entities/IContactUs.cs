using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IContactUs : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string Address { get; set; }

	string CodeMeli { get; set; }

	DateTime? DateBirth { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	string Email { get; set; }

	string Family { get; set; }

	string Fax { get; set; }

	bool Jensiat { get; set; }

	string Message { get; set; }

	string Mob { get; set; }

	string Mob2 { get; set; }

	string NahveAshnaee { get; set; }

	string Name { get; set; }

	string Ostan { get; set; }

	string SabegheKari { get; set; }

	string Senf { get; set; }

	string Shahrestan { get; set; }

	string Shoghl { get; set; }

	string Subject { get; set; }

	string Tahsilat { get; set; }

	string Tel { get; set; }

	string Tel2 { get; set; }

	TypeContact Type { get; set; }

	string WebSite { get; set; }

	string Zip { get; set; }

	string Ip { get; set; }
}
