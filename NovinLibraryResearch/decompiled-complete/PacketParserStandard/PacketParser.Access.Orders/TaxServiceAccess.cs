using System.ComponentModel;

namespace PacketParser.Access.Orders;

public class TaxServiceAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال صورت حساب اصلی فروش")]
	public bool SendSellSubject { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال صورت حساب اصلاحی")]
	public bool SendEditSubject { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال صورت حساب ابطالی")]
	public bool SendDeleteSubject { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال صورت حساب مرجوعی")]
	public bool SendBackSellSubject { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش گیری ارسال")]
	public bool ShowReport { get; set; } = true;


	[DefaultValue(false)]
	[PersianNameAttribute.PersianName("تغییر نحوه تسویه")]
	public bool ChangeTasvieMethod { get; set; }
}
