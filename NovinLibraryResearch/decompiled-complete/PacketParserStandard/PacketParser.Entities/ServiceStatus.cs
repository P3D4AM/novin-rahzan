namespace PacketParser.Entities;

public enum ServiceStatus : short
{
	[PersianNameAttribute.PersianName("در انتظار ادامه...")]
	ContinuePending,
	[PersianNameAttribute.PersianName("معوق")]
	Paused,
	[PersianNameAttribute.PersianName("در انتظار تعویق...")]
	PausePending,
	[PersianNameAttribute.PersianName("در حال اجرا")]
	Running,
	[PersianNameAttribute.PersianName("در انتظار اجرا...")]
	StartPending,
	[PersianNameAttribute.PersianName("متوقف")]
	Stopped,
	[PersianNameAttribute.PersianName("در انتظار توقف...")]
	StopPending,
	[PersianNameAttribute.PersianName("یافت نشد")]
	NotFound
}
