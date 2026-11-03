namespace PacketParser.Entities;

public enum EnMainNotification : short
{
	[PersianNameAttribute.PersianName("یادآوری اقساط دارای تاخیر")]
	DelayedInstallments = 1,
	[PersianNameAttribute.PersianName("یادآوری چک ها و اقساط")]
	ChequeReminder,
	[PersianNameAttribute.PersianName("یادآوری دوره ویزیت")]
	VisitPriod,
	[PersianNameAttribute.PersianName("یادآوری سررسید اسناد یادداشت")]
	SanadNote,
	[PersianNameAttribute.PersianName("یادآوری وضعیت اسناد")]
	SanadStatusReport,
	[PersianNameAttribute.PersianName("اسناد ارسال نشده به سرور")]
	ServerStatus,
	[PersianNameAttribute.PersianName("بازیابی اسناد و فاکتورها")]
	SaveTemp,
	[PersianNameAttribute.PersianName("یادداشت ها")]
	Notes
}
