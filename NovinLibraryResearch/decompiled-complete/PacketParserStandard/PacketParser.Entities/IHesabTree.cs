using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IHesabTree : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal Account { get; set; }

	string CodeKol { get; set; }

	string CodeMoein { get; set; }

	string CodeTafsil { get; set; }

	Guid GroupGuid { get; set; }

	Guid KolGuid { get; set; }

	string KolName { get; set; }

	HesabType HType { get; set; }

	ENMahiat Mahiat { get; set; }

	Guid MoeinGuid { get; set; }

	string MoeinName { get; set; }

	string Name { get; set; }

	string ParentName { get; set; }

	Guid TafsilGuid { get; set; }

	short TafsilLevel { get; set; }

	bool Tedadi { get; set; }

	bool Special { get; set; }

	Guid VisitorGuid { get; set; }

	Guid ParentGuid { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }

	bool Peygiri { get; set; }

	bool System { get; set; }
}
