using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IReception : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasSanadNo, IHasTafsilGuid
{
	bool? DeliveryStatus { get; set; }

	bool IsChecked { get; set; }

	ENSource Source { get; set; }

	bool IsTemp { get; }

	string AndroidIme { get; set; }

	string CpuSerial { get; set; }

	DateTime Date { get; set; }

	DateTime DateSabt { get; set; }

	string Description { get; set; }

	string HddSerial { get; set; }

	Guid UserGuid { get; set; }

	Guid MoeinGuid { get; set; }

	string Ip { get; set; }

	string LkSerial { get; set; }

	short SanadSt { get; set; }

	decimal Sum { get; }

	decimal SumNaghd { get; }

	decimal SumBank { get; }

	decimal SumCheck { get; }

	decimal SumEnteghal { get; }

	PaymentMethod PaymentMethod { get; set; }

	DependencyType DependencyType { get; set; }

	DateTime? TransferDate { get; set; }

	DateTime? DateMoaser { get; set; }

	DateTime? DateUtc { get; set; }

	Guid? OrderGuid { get; set; }

	Document? OrderType { get; set; }
}
