namespace PacketParser.Entities.Attendance;

public enum VacationTypeEnum : short
{
	[PersianNameAttribute.PersianName("انتقال کامل مانده مرخصی")]
	VacationTransfer,
	[PersianNameAttribute.PersianName("بازخرید کامل مانده مرخصی")]
	VacationRedeem,
	[PersianNameAttribute.PersianName("ابطال کامل مانده مرخصی")]
	VacationRejection
}
