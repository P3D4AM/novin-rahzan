using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPardakhtCheckSh : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	DateTime DateSaresid { get; set; }

	string Number { get; set; }

	Guid BankTafsilGuid { get; set; }

	new Guid? MasterGuid { get; set; }

	DateTime PardakhtDate { get; set; }

	bool Aval2re { get; set; }

	Guid? A2re_MoeinGuid { get; set; }

	Guid? A2re_TafsilGuid { get; set; }

	Guid CreditMoeinGuid { get; set; }

	Guid? CreditTafsilGuid { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	bool IsEnteghali { get; set; }

	Guid? UserGuid { get; set; }

	Guid? DebitA2reMoeinGuid { get; set; }

	Guid? DebitA2reTafsiGuid { get; set; }

	int? SanadNo { get; set; }

	string SayadiNationalCode { get; set; }

	string SayadiIdentity { get; set; }

	string SayadiName { get; set; }

	bool SayadiRegistered { get; set; }

	DateTime? DateUtc { get; set; }
}
