using System;
using PacketParser.Entities;

namespace PacketParser.Records.OccasionCalendar;

public record CouponDiscountSmsRecord
{
	public Guid TafsilGuid { get; set; }

	public EnCouponMode Mode { get; set; }
}
