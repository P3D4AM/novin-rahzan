using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IPardakht : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid
{
	bool IsTemp { get; }

	ENSource Source { get; set; }

	string AndroidIme { get; set; }

	string CpuSerial { get; set; }

	DateTime Date { get; set; }

	DateTime DateSabt { get; }

	string Description { get; set; }

	string HddSerial { get; set; }

	Guid MoeinGuid { get; set; }

	Guid UserGuid { get; set; }

	string Ip { get; set; }

	bool IsRead { get; set; }

	string LkSerial { get; set; }

	SanadSt SanadSt { get; set; }

	decimal Sum { get; }

	decimal SumCheckM { get; }

	decimal SumCheckSh { get; }

	decimal SumEnteghal { get; }

	decimal SumBank { get; }

	decimal SumNaghd { get; }

	DateTime? TransferDate { get; set; }

	DateTime? DateUtc { get; set; }

	DateTime? DateMoaser { get; set; }

	string AutoTitle(ENAction act);
}
