using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Khazane;

namespace PacketParser.Entities;

public interface IReceptionHavale : IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasWage
{
	Guid TafsilGuid { get; set; }

	Guid MoeinGuid { get; set; }

	string Number { get; set; }

	string TerminalId { get; set; }

	string PAN { get; set; }

	string STAN { get; set; }

	string RRN { get; set; }

	string MerchantId { get; set; }

	string MessageId { get; set; }

	DateTime DateTasviye { get; set; }

	string FullDescription { get; }

	decimal Qt { get; set; }

	Guid? DecimalGuid { get; set; }

	DateTime Date { get; set; }

	DateTime? DateUtc { get; set; }
}
