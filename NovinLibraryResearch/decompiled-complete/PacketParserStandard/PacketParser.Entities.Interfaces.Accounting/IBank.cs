using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IBank : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	bool IsPos { get; set; }

	long Code { get; set; }

	string Name { get; set; }

	string Shobe { get; set; }

	string ShobeCode { get; set; }

	string HesabNo { get; set; }

	string HesabType { get; set; }

	string Tel1 { get; set; }

	string Tel2 { get; set; }

	string Fax { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }

	bool Manfi { get; set; }

	string DarandeName { get; set; }

	string DarandeTel { get; set; }

	string DarandeADD { get; set; }

	string VName { get; set; }

	string VFName { get; set; }

	string VShenas { get; set; }

	string VMeli { get; set; }

	string VPosti { get; set; }

	string VTel { get; set; }

	string VAdd { get; set; }

	string ParentFullCode { get; set; }

	string PortName { get; set; }

	EnumConnectionType ConnectionType { get; set; }

	long PortSpeed { get; set; }

	double PosMultiplier { get; set; }

	Guid TafsilGroupGuid { get; set; }
}
