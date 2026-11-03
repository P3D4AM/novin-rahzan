using System;
using System.Collections.Generic;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface ICashier : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid UserGuid { get; set; }

	string Name { get; set; }

	string UserName { get; set; }

	byte[] HashedPassword { get; set; }

	string Description { get; set; }

	DateTime DateM { get; set; }

	Guid? SandoghGuid { get; set; }

	Guid? DepotGuid { get; set; }

	Guid? VisitorGuid { get; set; }

	Guid? BankGuid1 { get; set; }

	Guid? BankGuid2 { get; set; }

	Guid? BankGuid3 { get; set; }

	Guid? BankGuid4 { get; set; }

	Guid? CusGuid { get; set; }

	Guid? DefaultCusGroup { get; set; }

	bool AllVisitorAccess { get; set; }

	bool AllSandoghsAccess { get; set; }

	bool AllDepotsAccess { get; set; }

	bool AllBanksAccess { get; set; }

	short LoginAccess { get; }

	List<CashierTime> CashierTime { get; set; }

	Guid? ReceptionSmsCustomerGuid { get; set; }

	Guid? PardakhtSmsCustomerGuid { get; set; }

	Guid FareMoeinGuid { get; set; }

	Guid? FareTafsilGuid { get; set; }
}
