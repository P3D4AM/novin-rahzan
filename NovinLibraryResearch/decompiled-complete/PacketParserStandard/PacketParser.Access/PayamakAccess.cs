using System.ComponentModel;

namespace PacketParser.Access;

public class PayamakAccess
{
	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("پیامک")]
	public bool Payamak_Payamak { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("افزودن مخاطب به دفترچه تلفن")]
	public bool Payamak_Mokhatab { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("دفترچه تلفن")]
	public bool Payamak_Daftarche { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("ارسال پیامک")]
	public bool Payamak_Send { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش پیامک های ارسال")]
	public bool Payamak_Ersali { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("گزارش پیامک های در حال ارسال")]
	public bool Payamak_DarErsal { get; set; } = true;


	[DefaultValue(true)]
	[PersianNameAttribute.PersianName("لیست پیامک های دریافتی")]
	public bool Payamak_Daryafti { get; set; } = true;

}
