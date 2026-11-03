using System;

namespace PacketParser.Records.OccasionCalendar;

public record CouponDiscountReminderSmsRecord
{
	public Guid CouponGuid { get; set; }

	public Guid MetaDataGuid { get; set; }
}
