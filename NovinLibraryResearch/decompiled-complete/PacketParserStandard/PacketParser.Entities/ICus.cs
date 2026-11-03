using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface ICus : ICusCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[PersianNameAttribute.PersianName("آدرس1")]
	string Address1 { get; set; }

	string Address2 { get; set; }

	string CodeMeli { get; set; }

	string Company { get; set; }

	DateTime Date { get; set; }

	string Description { get; set; }

	string EghtesadiNo { get; set; }

	decimal Etebar { get; set; }

	decimal EtebarCheck { get; set; }

	[Obsolete("Please Use AreaGuid", true)]
	int IdPath { get; set; }

	Guid? AreaGuid { get; set; }

	new bool IsBazaryab { get; set; }

	new bool IsSupervisor { get; set; }

	string Job { get; set; }

	string Location { get; set; }

	string Password { get; set; }

	string Email { get; set; }

	Guid SupervisorGuid { get; set; }

	string User { get; set; }

	string Zip { get; set; }

	CustomerType CustType { get; set; }

	short TafsilLevel { get; set; }

	decimal BPercent { get; set; }

	bool InPlacePayment { get; set; }

	TypeTax TaxType { get; set; }

	ReturnedState ChequeBargashtiValidation { get; set; }

	ReturnedState OrderTasvieTimeValidation { get; set; }

	DateTime? SendToServerDate { get; set; }

	Guid? RoleGuid { get; set; }
}
