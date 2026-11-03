using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IResid : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	long ID2 { get; set; }

	DateTime DateM { get; set; }

	string Description { get; set; }

	DateTime Date1 { get; set; }

	DateTime Date2 { get; set; }

	decimal Price { get; set; }

	decimal Qt { get; set; }

	decimal Mandeh { get; set; }

	string PrdModel { get; set; }

	string PrdColor { get; set; }

	string PrdSerial { get; set; }

	string Problems { get; set; }

	string Darkhast { get; set; }

	short StatusId { get; set; }

	Guid? CashierGuid { get; set; }

	Guid? ReceptionGuid { get; set; }

	Guid? OrderGuid { get; set; }

	string Reserve { get; set; }
}
