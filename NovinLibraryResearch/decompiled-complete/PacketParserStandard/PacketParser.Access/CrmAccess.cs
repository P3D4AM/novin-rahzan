namespace PacketParser.Access;

public class CrmAccess
{
	[PersianNameAttribute.PersianName("مدیریت خط ها")]
	public bool Crm_LineMangment { get; set; } = true;


	[PersianNameAttribute.PersianName("لاگ تماس ها")]
	public bool Crm_CallLog { get; set; } = true;


	[PersianNameAttribute.PersianName("پشتیبانی")]
	public bool Crm_Support { get; set; } = true;

}
