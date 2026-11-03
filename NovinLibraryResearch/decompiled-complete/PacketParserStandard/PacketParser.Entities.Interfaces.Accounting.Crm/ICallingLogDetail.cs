using System;

namespace PacketParser.Entities.Interfaces.Accounting.Crm;

public interface ICallingLogDetail
{
	Guid? CashierGuid { get; set; }

	DateTime? PickUpDateM { get; set; }

	DateTime? HangUpDateM { get; set; }

	DateTime DateInsert { get; set; }

	DateTime Modified { get; set; }

	EnumNotification Status { get; set; }

	string Uuid { get; set; }

	string LinkedId { get; set; }

	string CallFrom_Number { get; set; }

	string CallFrom_Name { get; set; }

	string CallTo_Number { get; set; }

	string CallTo_Name { get; set; }

	string Description { get; set; }

	short Rings { get; set; }

	int Version { get; set; }
}
