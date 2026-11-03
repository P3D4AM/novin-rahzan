using System;
using PacketParser.Entities.Interfaces;

namespace Entities.Interfaces.Accounting.Cheque;

public interface IChequeBook : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string SerialNumber { get; set; }

	Guid BankGuid { get; set; }

	bool IsSaderShode { get; set; }
}
