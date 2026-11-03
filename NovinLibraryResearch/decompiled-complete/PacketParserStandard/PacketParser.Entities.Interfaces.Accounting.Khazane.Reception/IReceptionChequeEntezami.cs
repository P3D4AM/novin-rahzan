using System;

namespace PacketParser.Entities.Interfaces.Accounting.Khazane.Reception;

public interface IReceptionChequeEntezami : IHasModified
{
	Guid ChequeGuid { get; set; }

	Guid CashierGuid { get; set; }

	Guid DebitMoeinGuid { get; set; }

	Guid? DebitTafsilGuid { get; set; }

	Guid CreditMoeinGuid { get; set; }

	Guid? CreditTafsilGuid { get; set; }

	int SanadNo { get; set; }

	string Description { get; set; }

	DateTime ReceptionDateM { get; set; }
}
