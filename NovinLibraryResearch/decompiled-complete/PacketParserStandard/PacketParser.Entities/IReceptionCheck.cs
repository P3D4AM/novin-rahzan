using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IReceptionCheck : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	string BankName { get; set; }

	string HesabNum { get; set; }

	string Number { get; set; }

	bool Personal { get; set; }

	long PoshtNomreh { get; set; }

	DateTime Date { get; set; }

	new ENUMCheck Status { get; set; }

	new Guid? MasterGuid { get; set; }

	Guid? A2re_PardazandeTafsilGuid { get; set; }

	Guid? A2re_PardazandeMoeinGuid { get; set; }

	Guid DebitMoeinGuid { get; set; }

	Guid? DebitTafsilGuid { get; set; }

	DateTime? A2re_ReceptionDate { get; set; }

	bool Aval2re { get; set; }

	ServerStatusForClient ServerStatus { get; set; }

	bool IsEnteghali { get; set; }

	Guid? UserGuid { get; set; }

	Guid? CreditA2reMoeinGuid { get; set; }

	Guid? CreditA2reTafsilGuid { get; set; }

	Guid? VisitorGuid { get; set; }

	int? SanadNo { get; set; }

	string SayadiNationalCode { get; set; }

	string SayadiIdentity { get; set; }

	string SayadiName { get; set; }

	bool SayadiRegistered { get; set; }

	DateTime? DateUtc { get; set; }
}
