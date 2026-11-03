using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IBaseKhazaneReport : IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	DateTime DateM { get; set; }

	int Id2 { get; set; }

	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	Guid CashierGuid { get; set; }

	string Description { get; set; }

	decimal SumNaghd { get; set; }

	decimal SumHavale { get; set; }

	decimal SumChequeSh { get; set; }

	decimal SumChequeM { get; set; }

	decimal SumEnteghal { get; set; }

	decimal Sum { get; }

	bool IsChecked { get; set; }

	int StatusId { get; set; }

	byte[] SourceImage { get; }

	byte[] SanadStatusImage { get; }

	byte[] ServerStatusImage { get; }

	ServerStatusForClient ServerStatus { get; set; }

	string HostRefrenced { get; set; }

	int SanadNo { get; set; }

	ENSource Source { get; set; }
}
