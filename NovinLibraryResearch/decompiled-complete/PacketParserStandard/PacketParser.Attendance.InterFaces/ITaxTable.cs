using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.InterFaces;

public interface ITaxTable : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	short Year { get; set; }

	string Description { get; set; }

	DateTime StartDateM { get; set; }

	string StartDateSh { get; set; }

	DateTime EndDateM { get; set; }

	string EndDateSh { get; set; }

	decimal FromPrice { get; set; }

	decimal ToPrice { get; set; }

	decimal TaxPercentage { get; set; }

	decimal InitialValue { get; set; }
}
