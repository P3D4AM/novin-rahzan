using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IViewCheckSh : IHasJustGuid, IHasStatus
{
	string Number { get; set; }

	decimal Price { get; set; }

	ENCheckSHStatus ChequeStatus { get; set; }

	string BankName { get; set; }

	string CustomerFullName { get; set; }

	string PardakhtDate { get; set; }

	Guid GuidSanad { get; set; }

	bool Aval2re { get; set; }

	DateTime DateSarresidM { get; set; }

	string PardakhtDesc { get; set; }

	bool IsEnteghali { get; set; }

	bool SayadiRegistered { get; set; }

	DateTime? DateUtc { get; set; }
}
