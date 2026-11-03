using System;

namespace PacketParser.Entities.Interfaces.Global;

public interface ITempBarcodePrint : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ProductGuid { get; set; }

	Guid TafsilGuid { get; set; }

	Guid MasterGuid { get; set; }

	Guid DetailGuid { get; set; }

	string Barcode { get; set; }

	string GuarantyCode { get; set; }

	Document AsType { get; set; }

	string MasterDescription { get; set; }

	string DetailDescription { get; set; }

	DateTime MasterDate { get; set; }

	DateTime? GuarantyStartDate { get; set; }

	DateTime? GuarantyEndDate { get; set; }

	string TempDescription { get; set; }
}
