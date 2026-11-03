namespace PacketParser.Entities;

public enum EnIssueReportingLevel : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("وارد نیست")]
	Rejected,
	[PersianNameAttribute.PersianName("ویژگی جدید درخواستی")]
	NewOptionRequest,
	[PersianNameAttribute.PersianName("خطای بی اهمیت")]
	LowBug,
	[PersianNameAttribute.PersianName("خطای کم اهمیت")]
	MediumBug,
	[PersianNameAttribute.PersianName("خطای مهم")]
	HighBug,
	[PersianNameAttribute.PersianName("خطای مهلک")]
	CriticalBug
}
