using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IViewCheckM : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	string BankName { get; set; }

	decimal Price { get; set; }

	new ENUMCheck Status { get; set; }

	DateTime DateSanadM { get; set; }

	DateTime DateCheckM { get; set; }

	DateTime DateBargashtORVosolM { get; set; }

	string DateToPlace { get; set; }

	string Place { get; set; }

	string Description { get; set; }

	string Number { get; set; }

	Guid GuidSanad { get; set; }

	Guid PardakhtGuid { get; set; }

	bool Personal { get; set; }

	string HesabNum { get; set; }

	bool Aval2re { get; set; }

	long PoshtNomreh { get; set; }

	Guid VisitorGuid { get; set; }

	Guid PardazandeTafsilGuid { get; set; }

	bool IsEnteghali { get; set; }

	DateTime? DateUtc { get; set; }
}
