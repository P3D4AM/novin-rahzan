using System;

namespace PacketParser.Entities;

public interface ISerialMojood
{
	string Description { get; set; }

	int DetID { get; set; }

	DateTime EndDate { get; set; }

	string GNCode { get; set; }

	bool IsMojod { get; set; }

	Guid PrdGuid { get; set; }

	string Serial { get; set; }

	bool SMS { get; set; }

	string SMSMob { get; set; }

	DateTime StartDate { get; set; }

	short Type { get; set; }

	int TypeID { get; set; }
}
