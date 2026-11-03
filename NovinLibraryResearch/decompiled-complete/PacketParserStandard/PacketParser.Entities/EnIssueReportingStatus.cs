namespace PacketParser.Entities;

public enum EnIssueReportingStatus : short
{
	[PersianNameAttribute.PersianName("نامشخص")]
	None,
	[PersianNameAttribute.PersianName("بررسی نشده")]
	notChecked,
	[PersianNameAttribute.PersianName("دیده شده")]
	seen,
	[PersianNameAttribute.PersianName("معلق")]
	pending,
	[PersianNameAttribute.PersianName("در حال رفع خطا")]
	working,
	[PersianNameAttribute.PersianName("خطا رفع شده")]
	done
}
